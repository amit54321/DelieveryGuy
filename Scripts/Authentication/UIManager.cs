using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Authentication
{
    public class UIManager : MonoBehaviour
    {
        public string firebaseMessage;
        public static UIManager instance;
        [SerializeField] public Analytics analytics;
        [SerializeField] Text errorText;

        public GameObject loginScreen, signUpScreen, forgetPasswordScreen, verifyOTPscreen, resetPasswordScreen;

        public GameObject currentScreen;

        public Transform loadingPanel;
        
        public static Dictionary<string,Texture> profilePictures = new Dictionary<string, Texture>();

        public void Awake()
        {
            if (instance == null)
                instance = this;
        }

        public void ShowError(string error)
        {
            errorText.text = error;
            loadingPanel.gameObject.SetActive(false);
            Invoke("HideError", 2);
        }

        public void ToggleLoadingPanel(bool toogle)
        {
            loadingPanel.gameObject.SetActive(toogle);
        }

        public void EnableScreen(GameObject screen)
        {
            if (currentScreen != null)
                currentScreen.SetActive(false);
            currentScreen = screen;
            Analytics.SendAnalytics(AnalyticsEvents.SCREENOPEN, new Dictionary<string, object>()
            {
                { "screen",currentScreen },
                {"id",PlayerPrefsData.ID }

            });
            //  analytics.SetEvents(currentScreen+"_Open", "");
            //   analytics.SendAnalytics();
            currentScreen.SetActive(true);
            HideError();
        }

        public void HideError()
        {
            errorText.text = "";
        }
    }
}