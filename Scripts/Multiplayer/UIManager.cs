using System.Collections.Generic;
using System.Net.Mime;
using Authentication;
using UnityEngine;
using UnityEngine.UI;


namespace RoomContoller
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public static Dictionary<string, Texture> profilePictures = new Dictionary<string, Texture>();
        public Transform currentPanel;

        public Transform
            loader, internetConnectionPopUp;

        public Transform createRoomScreen,
            profileScreen,
            joinRoomScreen,
            alreadyJoinedRoomScreen,
            createJoinScreen,
            joinPublicScreen,
            selectPegScreen,
            inAppScreen,
            missionScreen,
            missionCompleteScreen,
            quitPopUp;

        public WatchAdsPopUp watchAdsPopUp;
        public FullVersionPopUp fullVersionPopUp;
        public DailyRewardPopUp dailyRewardPopUp;
        public SharePopUp sharePopUp;
        [SerializeField] private Text error;
        [SerializeField] public Analytics analytics;
        void Awake()
        {
            if (instance == null)
                instance = this;
            SocketMaster.userType = "game";
        }

        public void ShowError(string error)
        {
            this.error.text = error;
            Invoke("RemoveError", 2);
        }

        void RemoveError()
        {
            this.error.text = "";
        }

        public void ToggleLoader(bool toggle)
        {
            loader.gameObject.SetActive(toggle);
        }

        public void EnablePanel(Transform panel)
        {
            if (currentPanel != null)
            {
                currentPanel.gameObject.SetActive(false);
            }
            Analytics.SendAnalytics(AnalyticsEvents.SCREENOPEN, new Dictionary<string, object>()
            {
                { "screen",panel.name },
                {"id",PlayerPrefsData.ID }

            });
         //   analytics.SetEvents(panel.name + "_Open", "");
          //  analytics.SendAnalytics();
            currentPanel = panel;
            currentPanel.gameObject.SetActive(true);
        }

        public void EnablePopUp(Transform popUp)
        {
            popUp.gameObject.SetActive(true);
        }

        public void DisablePopUp(Transform popUp)
        {
            popUp.gameObject.SetActive(false);
        }

        public bool CheckInternetConnection()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                return false;
            }

            return true;
        }

        public void QuitGame()
        {
            quitPopUp.gameObject.SetActive(true);
        }

        void DeactivateAllPopUps()
        {
            watchAdsPopUp.gameObject.SetActive(false);
            dailyRewardPopUp.gameObject.SetActive(false);
            fullVersionPopUp.gameObject.SetActive(false);
            sharePopUp.gameObject.SetActive(false);
        }

        public void OpenPopUpFromServer(LobbyData.ShowPopUp popUp)
        {
            ToggleLoader(false);
            DeactivateAllPopUps();
            switch (popUp.name)
            {
                case "WatchAds":
                    watchAdsPopUp.SetData(popUp.desc);
                    break;
                case "DailyReward":
                    dailyRewardPopUp.SetData(popUp.desc);
                    break;
                case "FullVersion":
                    fullVersionPopUp.SetData(popUp.desc);
                    break;
                case "Share":
                    sharePopUp.SetData(popUp.desc);
                    break;
            }
        }

        void Update()
        {
            if (internetConnectionPopUp == null)
            {
                return;
            }

            if (CheckInternetConnection())
            {
                internetConnectionPopUp.gameObject.SetActive(false);
            }
            else
            {
                internetConnectionPopUp.gameObject.SetActive(true);
            }
        }
    }
}