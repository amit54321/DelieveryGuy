using System.Collections;
using System.Collections.Generic;
using System.Net;
using Authentication;
using RoomContoller;
using UnityEngine;
using UnityEngine.UI;
using WebRequest = Authentication.WebRequest;

public class WinnerPopUpApi : WebRequest
{
    [SerializeField]
    Text coinsText; 
    // Start is called before the first frame update
    void OnEnable()
    {
        GetUserData();
    }

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
       
        LobbyData.UserMinimumDataCallBack deafultData = JsonUtility.FromJson<LobbyData.UserMinimumDataCallBack>(callback);
        if (deafultData.status == 200)
        {
            SocketMaster.instance.profileData.coins = deafultData.message.coins;
            SocketMaster.instance.profileData.matches = deafultData.message.matches;
            SocketMaster.instance.profileData.wins = deafultData.message.wins;
            SocketMaster.instance.profileData.delievery = deafultData.message.delievery;

            if (coinsText != null)
            {
                coinsText.text = SocketMaster.instance.profileData.coins.ToString();
            }
        }
        Leaderboard.Instance.ReportScore(SocketMaster.instance.profileData.wins, "CgkIifiEkucOEAIQAQ");
        Leaderboard.Instance.ReportScore(SocketMaster.instance.profileData.delievery, "CgkIifiEkucOEAIQAA");
    }
}
