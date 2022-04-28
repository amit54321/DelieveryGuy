using System;
using System.Collections;
using System.Collections.Generic;
using Authentication;
using LitJson;
using RoomContoller;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class JoinRoom : WebRequest
    {
        [SerializeField] private Button joinRoom;
        [SerializeField] private InputField roomName, roomCode;
        [SerializeField] private CoinSelector coinSelector;

        private void OnEnable()
        {
            SetCurrentScreen();
            GetUserData();
            roomName.text = "";
            roomCode.text = "";
            joinRoom.onClick.AddListener(JoinRoomRequest);
        }

        private void OnDisable()
        {
            joinRoom.onClick.RemoveListener(JoinRoomRequest);
        }


        private void JoinRoomRequest()
        {
            Debug.LogError("joinroom" + roomCode.text);
            if (String.IsNullOrEmpty(roomCode.text))
            {
                UIManager.instance.ShowError("Type Room Code");
                return;
            }

            if (coinSelector.current == -1)
            {
                UIManager.instance.ShowError("No coins for play");
                return;
            }

            UIManager.instance.ToggleLoader(true);
            LobbyData.JoinRoomData roomData;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.JOINROOM,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.LogError("joinroom" + JsonMapper.ToJson(args[0]));
                        UIManager.instance.ToggleLoader(false);
                        JoinRoomCallBack(
                            JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                    }
                },
                roomData = new LobbyData.JoinRoomData()
                {
                    _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                    roomCode = this.roomCode.text,
                    bet = 10
                });


            joinRoom.enabled = false;
        }

        private void JoinRoomCallBack(LobbyData.RoomDataCallBack callbackdata)
        {
            joinRoom.enabled = true;
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

            StartCoroutine(PostNetworkRequest(AuthenticationConstants.GETUSER, data, GetUserDataCallBack, Error,
                false));
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

            coinSelector.SetData(bets);
        }

        public void Error(string error)
        {
        }
    }
}