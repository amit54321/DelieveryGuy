using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using RoomContoller;

public class WinnerPopUp : BasePOpUp
{
    [SerializeField]
    Text winner;

   
    // Start is called before the first frame update
    public void OnEnable()
    {
        string id = RoomContoller.SocketMaster.instance.gamePlay.winnerId;
        if (!PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID).Equals(id))
        {
            winner.text = "YOU LOST";
        }
        else
        {
            SocketMaster.instance.StartCoroutine(SocketMaster.instance.SendMissions(new List<int>() { 8,9,10,11 }));
            
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
