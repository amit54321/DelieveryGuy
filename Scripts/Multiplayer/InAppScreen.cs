using Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAppScreen : WebRequest
{

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void AddCoins(int coins)
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
           {

               {"id",PlayerPrefs.GetString(PlayerPrefsData.ID) },// SystemInfo.deviceUniqueIdentifier},
                 {"coins", coins},
            };
       
        StartCoroutine(PostNetworkRequest(AuthenticationConstants.ADDCOINS, data, RegisterCallBack, Error, false));
    }
    public void RegisterCallBack(string callback)
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
