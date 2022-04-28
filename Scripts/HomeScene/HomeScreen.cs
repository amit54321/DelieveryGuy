using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum STATUS
{
    PLAY,
    SET
}
namespace RoomContoller
{ 

public class HomeScreen : MonoBehaviour
{
    [SerializeField]
    Button play, set;
    public static STATUS status;
    private void Start()
    {
        // if (RoomContoller.SocketMaster.instance.profileData.restaurants.Count < 10)
        {
            // show warning first construct all restaurants
            //  play.enabled = false;
            //  play.GetComponent<Image>().color = Color.black;
        }
        // else
        {
            play.enabled = true;
            play.GetComponent<Image>().color = Color.white;
        }
        play.onClick.AddListener(SetPlayStatus);
        set.onClick.AddListener(SetSetStatus);
    }
    // Start is called before the first frame update
    public void SetPlayStatus()
    {
        UIManager.instance.EnablePanel(UIManager.instance.createJoinScreen);
       status = STATUS.PLAY;
     //   SceneManager.LoadScene("GameScene");
    }

    public void SetSetStatus()
    {
        status = STATUS.SET;
        SceneManager.LoadScene("GameScene");
    }

}
}
