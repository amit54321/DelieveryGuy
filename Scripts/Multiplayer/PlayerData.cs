using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
   [SerializeField] private Image picture;
   [SerializeField] Text name;
    
    public void SetDataForWaiting()
   {
      
      this.name.text = "waiting for player"; 
   }

   public void SetData(LobbyData.UserProfile profile,Sprite sp)
   {
      if (profile != null)
      {
         name.text = profile.name;
            picture.sprite = sp;
      //   StartCoroutine(DownloadImage.LoadRawImage(profile.avatar, picture));
      }
     
   }
   
}
