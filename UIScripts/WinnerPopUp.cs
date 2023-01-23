using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RoomContoller;
using Authentication;

public class WinnerPopUp : BasePOpUp
{
    [SerializeField]
    Text winner;

   
    // Start is called before the first frame update
    public void OnEnable()
    {
        string id = SocketMaster.instance.gamePlay.winnerId;
      
        if (!string.IsNullOrEmpty(id) && !PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID).Equals(id))
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("status","lost");
            Analytics.SendAnalytics(Analytics.GameEndStatus, d);
            winner.text = "YOU LOST";
        }
        else
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("status", "win");
            Analytics.SendAnalytics(Analytics.GameEndStatus, d);
           // SocketMaster.instance.StartCoroutine(SocketMaster.instance.SendMissions(new List<int>() { 8,9,10,11 }));
            
            winner.text = "YOU WIN";

        }
    }
    

    public void Lobby()
    {
        SceneManager.LoadScene("Lobby");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
