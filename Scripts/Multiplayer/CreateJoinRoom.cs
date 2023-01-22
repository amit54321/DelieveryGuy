using System.Collections;
using System.Collections.Generic;
using Authentication;
using LitJson;
using RoomContoller;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class CreateJoinRoom : WebRequest
    {
        [SerializeField] private Button createRoom, joinRoom, joinPublicRoom, profileButton, inAppButton, missionButton;
        [SerializeField] private RawImage picture;
        [SerializeField] Text name, coins;
        public void GetUserData()
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"id", PlayerPrefs.GetString(PlayerPrefsData.ID)}
            };

            StartCoroutine(PostNetworkRequest(AuthenticationConstants.GETUSERMINIMUMDATA, data, GetUserDataCallBack, Error,
                false));
        }

        public void GetUserDataCallBack(string callback)
        {
          //  LobbyData.DefaultAUth deafult = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
       //     LobbyData.UserProfile data = deafult.message;
            LobbyData.UserMinimumDataCallBack data = JsonUtility.FromJson<LobbyData.UserMinimumDataCallBack>(callback);
            name.text = data.message.name;
            coins.text = data.message.coins.ToString();
            StartCoroutine(DownloadImage.LoadRawImage(data.message.avatar, picture));
        }

        private void OnDisable()
        {
            joinRoom.onClick.RemoveListener(JoinRoom);
            createRoom.onClick.RemoveListener(CreateRoom);
            profileButton.onClick.RemoveListener(ProfileScreen);
            joinPublicRoom.onClick.RemoveListener(JoinPublicRoom);
            inAppButton.onClick.RemoveListener(InappScreen);
            missionButton.onClick.RemoveListener(MissionScreen);
        }

        private void OnEnable()
        {
           
            SetCurrentScreen();
            GetUserData();
            joinRoom.onClick.AddListener(JoinRoom);
            createRoom.onClick.AddListener(CreateRoom);
            joinPublicRoom.onClick.AddListener(JoinPublicRoom);
            profileButton.onClick.AddListener(ProfileScreen);
            inAppButton.onClick.AddListener(InappScreen);
            missionButton.onClick.AddListener(MissionScreen);
           
        }

       

        private void ProfileScreen()
        {
            UIManager.instance.EnablePanel(UIManager.instance.profileScreen);
        }

        private void InappScreen()
        {
            RewardedAdsScript.ShowRewardedVideo();
           // UIManager.instance.EnablePanel(UIManager.instance.inAppScreen);
        }

        private void MissionScreen()
        {
            UIManager.instance.EnablePanel(UIManager.instance.missionScreen);
        }

        public void SendAnalytics(string typeOfRoom)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("room", typeOfRoom);
            Analytics.SendAnalytics(Analytics.RoomSelected, d);

        }

        private void CreateRoom()
        {
            CheckRoom(UIManager.instance.createRoomScreen);
            SendAnalytics("create");
        }

        private void JoinPublicRoom()
        {
            CheckRoom(UIManager.instance.joinPublicScreen);
            SendAnalytics("join");
        }

        private void JoinRoom()
        {
            CheckRoom(UIManager.instance.joinRoomScreen);
            SendAnalytics("joinpublic");
        }

        private void CheckRoom(Transform nextScreen)
        {
            UIManager.instance.ToggleLoader(true);
        
            LobbyData.CheckRoom roomData;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.CHECKROOM,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.LogError("checkroom" + JsonMapper.ToJson(args[0]));
                        UIManager.instance.ToggleLoader(false);
                        CheckRoomCallBack(
                            JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])), nextScreen);
                    }
                },
                roomData = new LobbyData.CheckRoom()
                {
                    _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID)
                });
        }


        private void CheckRoomCallBack(LobbyData.RoomDataCallBack callbackdata, Transform nextScreen)
        {
            if (callbackdata.status == 200)
            {
                PlayerPrefs.SetString("RoomId", callbackdata.room._id);
                UIManager.instance.EnablePanel(nextScreen);
            }
            else if (callbackdata.status == 400)
            {
                UIManager.instance.EnablePanel(UIManager.instance.alreadyJoinedRoomScreen);
                UIManager.instance.alreadyJoinedRoomScreen.GetComponent<RoomContoller.AlreadyRoomJoined>()
                    .SetData(callbackdata.room, callbackdata.timeLeft);
            }
        }
    }
}