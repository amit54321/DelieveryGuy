using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Authentication;
namespace RoomContoller
{
    
    public class WatchAdsPopUp : WebRequest
    {
        [SerializeField] private Text desc; 
        
        public  void SetData(string desc)
        {
            gameObject.SetActive(true);
            this.desc.text = desc;
           
        }
        
    public void GetUserDataCallBack(string callback)
    {
        gameObject.SetActive(false);
    }
    public void Error(string error)
    {
        
    }
        
    public void AdWatched()
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"id", PlayerPrefs.GetString(PlayerPrefsData.ID)}
            
        };

        StartCoroutine(PostNetworkRequest(AuthenticationConstants.WATCHADS, data, GetUserDataCallBack, Error, false));
    }
      
    }
}
