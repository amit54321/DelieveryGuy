using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace RoomContoller
{
    public class ProfileScreen : WebRequest
    {
        [SerializeField] private Image picture;
        [SerializeField] Text name,coins,matches,wins;
        [SerializeField] List<Sprite> profilePictures;
        void OnEnable()
        {
            SetCurrentScreen();
           
        }
        void SetCurrentScreen()
        {
            picture.sprite = profilePictures[int.Parse(SocketMaster.instance.profileData.avatar)];
                    name.text = SocketMaster.instance.profileData.name;
                 matches.text= "Macthes : "+ SocketMaster.instance.profileData.matches.ToString();
            wins.text = "Wins : "+SocketMaster.instance.profileData.wins.ToString();
            coins.text = "Coins : "+SocketMaster.instance.profileData.coins.ToString();
        }


    }
}