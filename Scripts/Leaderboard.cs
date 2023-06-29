using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomContoller;
using UnityEngine.UI;
#if UNITY_ANDROID
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GooglePlayGames.BasicApi;
#endif
public class Leaderboard : MonoBehaviour
{
    public static Leaderboard Instance { get; private set; }

    // Start is called before the first frame update
    public void ReportScore(int score, string leaderBoard)
    {
        /*PlayGamesPlatform.Instance.ReportScore(score, leaderBoard, (bool success) => {
            if (success)
            {
                result.text = "Success";
            }
            else
            {
                result.text = "Fail";
                Debug.Log("Add Score Fail");
            }
            // handle success or failure
        });
*/
        Social.localUser.Authenticate((bool success) =>
        {
           
            if (success)
            {
                Social.ReportScore(score,
                    leaderBoard,
                    (bool success2) =>
                    {
                     
                        //Handle Report Success
                    });
            }
        });
    
    
    }


    
    private void Awake()
    {
        Instance = this;
    }
    public Text result;
    public void Deliveries()
    {
        result.text = "Del";
        ReportScore(SocketMaster.instance.profileData.delievery, "CgkIifiEkucOEAIQAA");

    }

    public void Wins()
    {
        result.text = "Win";
        ReportScore(SocketMaster.instance.profileData.wins, "CgkIifiEkucOEAIQAQ");

    }

    public void ShowLeaderboardEmpty()
    {

#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
        //  Social.ShowLeaderboardUI();
#endif

    }
    IEnumerator SendScore()
    {
        yield return new WaitForSeconds(0.5f);
        ReportScore(SocketMaster.instance.profileData.wins, "CgkIifiEkucOEAIQAQ");
        yield return new WaitForSeconds(0.5f);
        ReportScore(SocketMaster.instance.profileData.delievery, "CgkIifiEkucOEAIQAA");
    }

    // Update is called once per frame
    public void ShowLeaderboard()
    {
        StartCoroutine(SendScore());
#if UNITY_ANDROID
        ((PlayGamesPlatform)Social.Active).ShowLeaderboardUI();
#endif
        //  Social.ShowLeaderboardUI();
    }

#if UNITY_ANDROID
    public void Start()
    {

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Sucess");
            }
            else
            {
                Debug.Log("Login failed");
            }
        });
        //  DontDestroyOnLoad(this);

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
