using System.Collections;
using System.Collections.Generic;
using Authentication;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class AlreadyRoomJoined : WebRequest
    {
        [SerializeField] private Button leaveRoom;

        [SerializeField] private Text roomName, timeLLeft;

        [SerializeField] private PlayerData[] players;

        private LobbyData.RoomData roomData;

        [SerializeField] private InputField roomCode;
        [SerializeField] List<Sprite> profilePictures;
        // Start is called before the first frame update
        void OnEnable()
        {
            PlayerPrefs.SetInt("PORTAL", 0);
            SetCurrentScreen();
            roomName.text = "";
            roomCode.text = "";
            leaveRoom.onClick.AddListener(LeaveRoom);
        }

        void DisableAllPlayers()
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].gameObject.SetActive(false);
            }
        }
       public void CopyToClipboard()
        {
            TextEditor textEditor = new TextEditor();
            textEditor.text = "Play with me by joining the room with code " + roomCode.text;
            textEditor.SelectAll();
            textEditor.Copy();
        }
        IEnumerator RunTimer(int timer)
        {
            Debug.LogError("TIMER   " + timer);
            while (timer > 0)
            {
                int min = Mathf.FloorToInt(timer / 60);
                int sec = Mathf.FloorToInt(timer % 60);
                timeLLeft.text = min.ToString("00") + ":" + sec.ToString("00");
                yield return new WaitForSeconds(1);
                timer--;
            }
        }

        public void SetData(LobbyData.RoomData roomData, int timeLeft)
        {
            this.roomData = roomData;
            StopAllCoroutines();
            DisableAllPlayers();
            PlayerPrefs.SetString("RoomId", roomData._id);
            roomName.text = roomData.name.ToString();
            roomCode.text = roomData.code.ToString();
            if (roomData._public == 1)
            {
                roomCode.gameObject.SetActive(false);
                roomName.gameObject.SetActive(false);
            }
            else
            {
                roomCode.gameObject.SetActive(true);
                roomName.gameObject.SetActive(true);
            }
            int numberOfPlayers = roomData.no_of_players;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                players[i].gameObject.SetActive(true);
                if (i < roomData.players_joined.Count)
                {
                    players[i].SetData(roomData.players_joined[i],profilePictures[int.Parse(roomData.players_joined[i].avatar)]);
                }
                else
                {
                    players[i].SetDataForWaiting();
                }
            }

            StartCoroutine(RunTimer(timeLeft / 1000));
        }

        private void LeaveRoom()
        {
            LobbyData.LeaveRoom leaveRoomData;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.LEAVEROOM,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");

                        UIManager.instance.ToggleLoader(false);
                        LeaveRoomCallBack(
                            JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                    }
                },
                leaveRoomData = new LobbyData.LeaveRoom()
                {
                    _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                    room_id = roomData._id
                });
        }


        private void LeaveRoomCallBack(LobbyData.RoomDataCallBack callbackdata)
        {
            if (callbackdata.status == 200)
            {
                UIManager.instance.EnablePanel(UIManager.instance.createJoinScreen);
            }
            else
            {
                UIManager.instance.ShowError(callbackdata.message);
            }
        }
    }
}