using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
public class Analytics :MonoBehaviour
{
   
    public static string RoomSelected = "ROOMSELECTED";
    public static string Construction = "BUILD";
    public static string Upgrade = "UPGRADE";
    public static string GameEndStatus = "GAMEENDSTATUS";
    public static string GameType = "GAMETYPE";
    public static string InappBuy = "INAPPBUY";
    public static string PlayerQuit = "PLAYERQUIT";
    public static string OPENPOPUP = "OPEN";
    public static string Tutorial = "TUTORIAL";
    public static void SendAnalytics(string customEventName, Dictionary<string, object> eventData)
    {
        eventData.Add("user_id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID, "first"));
        eventData.Add("deviceId", SystemInfo.deviceUniqueIdentifier);
       // foreach (KeyValuePair<string, object> d in eventData)
            {
         //         Debug.Log(d.Key+"  "+d.Value);
            //       values.Add(d.Value);
               }
            UnityEngine.Analytics.Analytics.CustomEvent(customEventName, eventData);
     //   List<string> keys = new List<string>();
     //   List<object> values = new List<object>();
     //   foreach (KeyValuePair<string,object> d in eventData)
     //   {
     //       keys.Add(d.Key);
     //       values.Add(d.Value);
     //   }
     //   if (keys.Count == 0)
     //   {
     //       Firebase.Analytics.FirebaseAnalytics.LogEvent(
     //customEventName,
     // new Firebase.Analytics.Parameter(
     //    "user_id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID, "first"))
      
     // );
     //   }
     //   if (keys.Count == 1)
     //   {
     //       Firebase.Analytics.FirebaseAnalytics.LogEvent(
     //customEventName,
     // new Firebase.Analytics.Parameter(
     //    "user_id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID, "first")),
     //  new Firebase.Analytics.Parameter(
     //  keys[0], values[0].ToString())
     // );
     //   }
     //   else if (keys.Count == 2)
     //   {
     //       Firebase.Analytics.FirebaseAnalytics.LogEvent(
     //customEventName,
     // new Firebase.Analytics.Parameter(
     //    "user_id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID, "first")),
     //  new Firebase.Analytics.Parameter(
     //  keys[0], values[0].ToString()),
     //  new Firebase.Analytics.Parameter(
     //      keys[1], values[1].ToString())
     // );
     //   }
     //   else if (keys.Count == 3)
     //   {
     //       Firebase.Analytics.FirebaseAnalytics.LogEvent(
     //customEventName,
     // new Firebase.Analytics.Parameter(
     //    "user_id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID, "first")),
     //  new Firebase.Analytics.Parameter(
     //  keys[0], values[0].ToString()),
     //  new Firebase.Analytics.Parameter(
     //      keys[1], values[1].ToString()),
     //  new Firebase.Analytics.Parameter(
     //     keys[2], values[2].ToString())
     //);
     //   }
     //   else if (keys.Count == 4)
     //   {
     //       Firebase.Analytics.FirebaseAnalytics.LogEvent(
     //customEventName,
     // new Firebase.Analytics.Parameter(
     //    "user_id", PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID, "first")),
     //  new Firebase.Analytics.Parameter(
     //  keys[0], values[0].ToString()),
     //  new Firebase.Analytics.Parameter(
     //      keys[1], values[1].ToString()),
     //  new Firebase.Analytics.Parameter(
     //     keys[2], values[2].ToString()),
     //  new Firebase.Analytics.Parameter(
     //      keys[3], values[3].ToString()));
     //   }
     //   // Debug.LogError("NAME  " + customEventName);
     //   // eventData.Add("deviceId", SystemInfo.deviceUniqueIdentifier);
     //   // UnityEngine.Analytics.Analytics.CustomEvent(customEventName, eventData);

    }



    
}
