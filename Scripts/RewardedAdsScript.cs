using Authentication;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class RewardedAdsScript : WebRequest//, IUnityAdsListener
{

   static string gameIdAndroid = "4165637";
    static string gameIdIOS= "4165636";
    static string mySurfacingId = "rewardedVideo";
    bool testMode = true;

    // Initialize the Ads listener and service:
    void OnEnable()
    {
#if UNITY_ANDROID


       // Advertisement.AddListener(this);
      //  Advertisement.Initialize(gameIdAndroid, testMode);
#elif UNITY_IOS
Advertisement.AddListener(this);
        Advertisement.Initialize(gameIdIOS, testMode);
#endif
    }

    public static void ShowRewardedVideo()
    {
        // Check if UnityAds ready before calling Show method:
       // if (Advertisement.IsReady(mySurfacingId))
        {
         //   Advertisement.Show(mySurfacingId);
        }
       // else
        {
       //     Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    // Implement IUnityAdsListener interface methods:
    public void OnUnityAdsDidFinish(string surfacingId)//, ShowResult showResult)
    {
        // Define conditional logic for each ad completion status:
       // if (showResult == ShowResult.Finished)
        {
            UpdateCoins();
            // Reward the user for watching the ad to completion.
        }
      //  else if (showResult == ShowResult.Skipped)
        {
            // Do not reward the user for skipping the ad.
        }
       // else if (showResult == ShowResult.Failed)
        {
            Debug.LogWarning("The ad did not finish due to an error.");
        }
    }

    public  void OnUnityAdsReady(string surfacingId)
    {
        // If the ready Ad Unit or legacy Placement is rewarded, show the ad:
        if (surfacingId == mySurfacingId)
        {
            // Optional actions to take when theAd Unit or legacy Placement becomes ready (for example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        // Log the error.
    }

    public void OnUnityAdsDidStart(string surfacingId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }

    // When the object that subscribes to ad events is destroyed, remove the listener:
    public void OnDestroy()
    {
      //  Advertisement.RemoveListener(this);
    }

    public void UpdateCoins()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
            {
              
                 {"id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID)},
             {"coins", 100}
            
            };

        StartCoroutine(PostNetworkRequest(AuthenticationConstants.UPDATECOINS, data, UpdateCoinsCallBack, Error, false));
       
    }

    public void UpdateCoinsCallBack(string callback)
    {
        Debug.Log("LOGIN CALLS" + callback);
      
        LobbyData.DefaultAUth data = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
        if (data.status == 200)
        {

            RoomContoller.UIManager.instance.EnablePanel(RoomContoller.UIManager.instance.createJoinScreen);
        }
        else
        {
            RoomContoller.UIManager.instance.ShowError(data.error);
            Debug.Log("MESSAGE ERROR " + data.error);
        }
    }


}