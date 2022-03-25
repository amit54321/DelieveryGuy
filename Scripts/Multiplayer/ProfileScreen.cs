using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class ProfileScreen : WebRequest
    {
        [SerializeField] private RawImage picture;
        [SerializeField] Text name,coins;

        void OnEnable()
        {
            SetCurrentScreen();
            GetUserData();
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

            name.text = data.name;
            coins.text = data.coins.ToString();
            StartCoroutine(DownloadImage.LoadRawImage(data.avatar, picture));
        }

        public void Error(string error)
        {
        }
    }
}