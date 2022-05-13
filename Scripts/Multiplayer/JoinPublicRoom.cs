using System.Collections;
using System.Collections.Generic;
using Authentication;
using LitJson;
using RoomContoller;
using UnityEngine;
using UnityEngine.UI;
using UIManager = RoomContoller.UIManager;

public class JoinPublicRoom : WebRequest
{
    [SerializeField] private Button[] playerSelection;
    [SerializeField] private Button createRoom;
    [SerializeField] private CoinSelector coinSelector;
    private int numberOfPlayers;

    private void OnEnable()
    {
        GetUserData();
        createRoom.onClick.AddListener(CreateRoomRequest);

        {
            playerSelection[0].onClick.AddListener(delegate { PlayerSelection(1); });
            playerSelection[1].onClick.AddListener(delegate { PlayerSelection(2); });
            playerSelection[2].onClick.AddListener(delegate { PlayerSelection(3); });
        }

        PlayerSelection(1);
    }

    private void OnDisable()
    {
        playerSelection[0].onClick.RemoveListener(delegate { PlayerSelection(1); });
        playerSelection[1].onClick.RemoveListener(delegate { PlayerSelection(2); });
        playerSelection[2].onClick.RemoveListener(delegate { PlayerSelection(3); });
        createRoom.onClick.RemoveListener(CreateRoomRequest);
    }

    private void CreateRoomRequest()
    {
        UIManager.instance.ToggleLoader(true);
        LobbyData.CreateRoomData roomData;

        if (coinSelector.current == -1)
        {
            UIManager.instance.ShowError("No coins for play");
            return;
        }

        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.JOINPUBLICROOM,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.LogError("createroom" + JsonMapper.ToJson(args[0]));
                    UIManager.instance.ToggleLoader(false);
                    CreateRoomCallBack(
                        JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            roomData = new LobbyData.CreateRoomData()
            {
                _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                numberOfPlayers = this.numberOfPlayers,
                _public = 1,
                roomName = "public room",
                time = 30000,
                bet = 10
               // bet = coinSelector.GetCurrentBet()
            });


        createRoom.enabled = false;
    }

    private void CreateRoomCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            PlayerPrefs.SetString("RoomId", callbackdata.room._id);
            UIManager.instance.EnablePanel(UIManager.instance.alreadyJoinedRoomScreen);
            UIManager.instance.alreadyJoinedRoomScreen.GetComponent<RoomContoller.AlreadyRoomJoined>()
                .SetData(callbackdata.room, callbackdata.timeLeft);
        }
        else
        {
            UIManager.instance.ShowError(callbackdata.message);
        }
    }

    public void GetUserData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"id", PlayerPrefs.GetString(PlayerPrefsData.ID)}
        };

        StartCoroutine(PostNetworkRequest(AuthenticationConstants.GETUSER, data, GetUserDataCallBack, Error, false));
    }

    public void GetUserDataCallBack(string callback)
    {
        LobbyData.DefaultAUth deafult = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
        LobbyData.UserProfile data = deafult.message;
        List<int> bets = new List<int>();
        if (data.coins >= data.level * 100)
        {
            bets.Add(data.level * 100);
        }

        if (data.coins >= data.level * 200)
        {
            bets.Add(data.level * 200);
        }

        if (data.coins >= data.level * 300)
        {
            bets.Add(data.level * 300);
        }

      //  coinSelector.SetData(bets);
    }

    public void Error(string error)
    {
    }


    void PlayerSelection(int selection)
    {
        for (int i = 0; i < playerSelection.Length; i++)
        {
            playerSelection[i].GetComponent<Image>().color = Color.black;
        }

        playerSelection[selection - 1].GetComponent<Image>().color = Color.white;
        numberOfPlayers = selection + 1;
    }
}