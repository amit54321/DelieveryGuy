using System.Collections;
using System.Collections.Generic;
using LitJson;
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
        private void CheckRoom(Transform nextScreen)
        {
            UIManager.instance.ToggleLoader(true);

            LobbyData.CheckRoom roomData;
            SocketMaster.instance.socketMaster.Socket.Emit(
                LobbyConstants.CHECKROOM,
                (socket, packet, args) =>
                {
                    if (args != null && args.Length > 0)
                    {
                        Debug.LogError("checkroom" + JsonMapper.ToJson(args[0]));
                        UIManager.instance.ToggleLoader(false);
                        CheckRoomCallBack(
                            JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])), nextScreen);
                    }
                },
                roomData = new LobbyData.CheckRoom()
                {
                    _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID)
                });
        }


        private void CheckRoomCallBack(LobbyData.RoomDataCallBack callbackdata, Transform nextScreen)
        {
            if (callbackdata.status == 200)
            {
                SendAnalytics("set");
                Authentication.Authentication.status = STATUS.SET;
                ButtonSOund.instance.Play();
                UIManager.instance.ToggleLoader(true);
                SceneManager.LoadScene("GameScene");
            }
            else if (callbackdata.status == 400)
            {
                UIManager.instance.ShowError("Currently searching a match."); ;
            }
        }
        private void OnEnable()
        {
            SetCoinsText();
        }

        public void SetCoinsText()
        {
            Debug.LogError("COINS  " + SocketMaster.instance.profileData.coins.ToString());
            coinsText.text = SocketMaster.instance.profileData.coins.ToString();
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
           if (RoomContoller.SocketMaster.instance.profileData.restaurants.Count < 10)
            {
              UIManager.instance.ShowError("Construct all 10 restaurants.");
            }
           else if (SocketMaster.instance.profileData.coins < 100)
            {
                UIManager.instance.ShowError("Coins should be greater than 100.");
            }
            else
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
            CheckRoom(UIManager.instance.createRoomScreen);

     
    }

        public void SendAnalytics(string typeOfRoom)
        {
            Dictionary<string, object> d = new Dictionary<string, object>();
            d.Add("gameType", typeOfRoom);
            Analytics.SendAnalytics(Analytics.GameType, d);

        }

    }
}
