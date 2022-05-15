using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Analytics :MonoBehaviour
{
   
    public static string RoomSelected = "ROOMSELECTED";
    public static string Construction = "BUILD";
    public static string Upgrade = "UPGRADE";
    public static string GameEndStatus = "GAMEENDSTATUS";
    public static string GameType = "GAMETYPE";

    public static string PlayerQuit = "PLAYERQUIT";
    public static string OPENPOPUP = "OPEN";
    
    public static void SendAnalytics(string customEventName, Dictionary<string, object> eventData)
    {
        // Debug.LogError("NAME  " + customEventName);
        eventData.Add("deviceId", SystemInfo.deviceUniqueIdentifier);
        UnityEngine.Analytics.Analytics.CustomEvent(customEventName, eventData);

    }



    
}
