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
           if (RoomContoller.SocketMaster.instance.profileData.restaurants.Count < 10)
            {
               UIManager.instance.ShowError("Construct all 10 buildings.");
            }
            else
            {
                UIManager.instance.EnablePanel(UIManager.instance.createJoinScreen);
                status = STATUS.PLAY;

            }
                //   SceneManager.LoadScene("GameScene");
            }

            public void SetSetStatus()
    {
        status = STATUS.SET;
        UIManager.instance.ToggleLoader(true);
        SceneManager.LoadScene("GameScene");
    }


        
}
}
