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
  public  void ReportScore(int score)
    {
        Social.ReportScore(score, "Cfji293fjsie_QA", (bool success) => {
            // handle success or failure
        });

    }
    private void Awake()
    {
        Instance = this;
    }



    // Update is called once per frame
   public  void ShowLeaderboard()
    {
        Social.ShowLeaderboardUI();
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
