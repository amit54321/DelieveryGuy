// simple component that holds match information
using System;
using System.Globalization;
using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/ Interest Management/ Match/Network Match")]
    [HelpURL("https://mirror-networking.gitbook.io/docs/guides/interest-management")]
    public class NetworkMatch : NetworkBehaviour
    {
        ///<summary>Set this to the same value on all networked objects that belong to a given match</summary>
        public Guid matchId;

        [Server]
        private void OnEnable()
        {
            Invoke("DelayedMatchid", 2f);
            
        }

        public void DelayedMatchid()
        {
            Debug.Log("Game object name = " + gameObject.name + "Match id  = " + matchId);
        }


    }

    
}
