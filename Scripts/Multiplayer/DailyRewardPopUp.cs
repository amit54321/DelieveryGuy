using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class DailyRewardPopUp : WebRequest
    {
        [SerializeField] private Text desc;

        [SerializeField] private Button dailyReward;
        // Start is called before the first frame update
      public  void SetData(string desc)
      {
          gameObject.SetActive(true);
          this.desc.text = desc;
          dailyReward.onClick.AddListener(DilyReward);
      }

      public void GetUserDataCallBack(string callback)
      {
          LobbyData.DefaultAUth data = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
          if (data.status == 200)
          {
                RoomContoller.UIManager.instance.EnablePanel(RoomContoller.UIManager.instance.createJoinScreen);
            }

          gameObject.SetActive(false);
      }
      public void Error(string error)
      {
        
      }
        
      public void DilyReward()
      {
          Dictionary<string, object> data = new Dictionary<string, object>()
          {
              {"id", PlayerPrefs.GetString(PlayerPrefsData.ID)}
            
          };

          StartCoroutine(PostNetworkRequest(AuthenticationConstants.DAILYREWARD, data, GetUserDataCallBack, Error, false));
      }
      
    
    }
}
