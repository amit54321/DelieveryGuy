using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
   [SerializeField] private RawImage picture;
   [SerializeField] Text name;

   public void SetDataForWaiting()
   {
      
      this.name.text = "waiting for player"; 
   }

   public void SetData(LobbyData.UserProfile profile)
   {
      if (profile != null)
      {
         name.text = profile.name;
         StartCoroutine(DownloadImage.LoadRawImage(profile.avatar, picture));
      }
     
   }
   
}
