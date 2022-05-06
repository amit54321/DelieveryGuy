using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class QuitData
{
    public string _id;
    public string game_id;
}

public class QuitPopUp : BasePOpUp
{
    // Start is called before the first frame update
   public void Quit()
    {
        QuitData constructRestaurant;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.QUITGAME,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");

                }
            },
            constructRestaurant = new QuitData()
            {
                _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),

                game_id = RoomContoller.SocketMaster.instance.gamePlay.game_id
            }) ;
        SceneManager.LoadScene("Lobby");
    }

    // Update is called once per frame
  public  void No()
    {
        InGame.UIManager.Instance.DisablePopUp();
    }
}
