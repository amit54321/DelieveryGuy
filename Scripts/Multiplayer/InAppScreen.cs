using Authentication;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InAppScreen : WebRequest
{

    [SerializeField]
    Text priceText, productIdText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddTimerPacks()
    {
      

        SocketMaster.instance.AddTimerPack();
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("cost", priceText.text);
        d.Add("producId", productIdText.text);
        Analytics.SendAnalytics(Analytics.InappBuy, d);
    }
    public void AddCoins(int coins)
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
           {

               {"id",PlayerPrefs.GetString(PlayerPrefsData.ID) },// SystemInfo.deviceUniqueIdentifier},
               {"coins", coins},
            };
       
        StartCoroutine(PostNetworkRequest(AuthenticationConstants.ADDCOINS, data, AddCoinsCallBack, Error, false));
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("cost", priceText.text);
        d.Add("producId", productIdText.text);
        Analytics.SendAnalytics(Analytics.InappBuy, d);
    }
    public void AddCoinsCallBack(string callback)
    {
        Debug.Log("LOGIN CALLS" + callback);
       

        RegisterCallback data = JsonUtility.FromJson<RegisterCallback>(callback);

        if (data.status == 200)
        {

            RoomContoller.SocketMaster.instance.profileData = data.message;
        }

    }
            // Update is called once per frame
            void Update()
    {
        
    }
}
