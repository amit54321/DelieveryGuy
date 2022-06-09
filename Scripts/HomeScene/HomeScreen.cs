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
  
        [SerializeField]
        Image picture;
        [SerializeField] List<Sprite> profilePictures;
        [SerializeField]
        Text coinsText;

        private void OnEnable()
        {
            coinsText.text =  SocketMaster.instance.profileData.coins.ToString();
        }
        private void Start()
    {
            picture.sprite = profilePictures[int.Parse(SocketMaster.instance.profileData.avatar)];

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
          // if (RoomContoller.SocketMaster.instance.profileData.restaurants.Count < 10)
            {
          //     UIManager.instance.ShowError("Construct all 10 buildings.");
            }
         //   else
            {
                
                SendAnalytics("play");
                ButtonSOund.instance.Play();
                UIManager.instance.EnablePanel(UIManager.instance.createJoinScreen);
               Authentication.Authentication.status = STATUS.PLAY;

            }
                //   SceneManager.LoadScene("GameScene");
            }

            public void SetSetStatus()
    {
          
            SendAnalytics("set");
            Authentication.Authentication.status = STATUS.SET;
            ButtonSOund.instance.Play();
            UIManager.instance.ToggleLoader(true);
        SceneManager.LoadScene("GameScene");
    }

        public void SendAnalytics(string typeOfRoom)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("gameType", typeOfRoom);
            Analytics.SendAnalytics(Analytics.GameType, d);

        }

    }
}
