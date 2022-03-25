
using System.Collections.Generic;

using UnityEngine;

public class Analytics 
{
   
   
    public static void SendAnalytics(string eventName , Dictionary<string, object> data)
    {
       
        Debug.LogError(eventName);
        UnityEngine.Analytics.Analytics.CustomEvent(eventName, data);
       
    }

    
}
