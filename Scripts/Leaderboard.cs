using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANDROID
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
#endif
public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    // Start is called before the first frame update
  public  void ReportScore(int score,string leaderBoard)
    {
        Social.ReportScore(score, leaderBoard, (bool success) => {
            if (success)
            {
             
            }
            else
            {
                Debug.Log("Add Score Fail");
            }
            // handle success or failure
        });

    }
    private void Awake()
    {
        Instance = this;
    }

    public void ShowLeaderboardEmpty()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
        //  Social.ShowLeaderboardUI();
    }


    // Update is called once per frame
    public void ShowLeaderboard()
    {
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
      //  Social.ShowLeaderboardUI();
    }

#if UNITY_ANDROID
    public void Start()
    {
        DontDestroyOnLoad(this);

        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);

    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            PlayGamesPlatform.Activate();
            // Continue with Play Games Services
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
        }
    }
#endif

}
