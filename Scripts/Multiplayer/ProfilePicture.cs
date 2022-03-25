using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;

namespace RoomContoller
{
    public class ProfilePicture : Authentication.WebRequest
    {
        private string selectedImage;
        [SerializeField] private UserProfile userProfile;

        public void ChangeProfilePicture(GameObject g)
        {
            UIManager.instance.ToggleLoader(true);
            WWWForm form = new WWWForm();
            selectedImage = "dots" + g.name;
            Debug.Log("SELECTED IMAGE  " + selectedImage);
            form.AddField("image", "dots" + g.name);
            //      StartCoroutine(PostRequest(Authentication.AuthenticationConstants.PROFILE_PIC, form,
            //        ChangeProfilePictureCallback, Error, true));
        }

        public void Cross()
        {
            gameObject.SetActive(false);
        }

        public void Error(string error)
        {
            UIManager.instance.ToggleLoader(false);
        }

        public void ChangeProfilePictureCallback(string callback)
        {
            UIManager.instance.ToggleLoader(false);
            //   AuthenticationData.ProfilePicCallback data =
            //       JsonUtility.FromJson<AuthenticationData.ProfilePicCallback>(callback);
            // if (data.success)
            // {
            //     Debug.Log("CALL  " + callback);
            //     Authentication.Authentication.userProfileData.profilePic = data.data.image;
            //     userProfile.SetProfileImage(); //(Authentication.Authentication.userProfileData.profilePic);
            //     //   DataRoomController.FriendList friendData =  JsonUtility.FromJson<DataRoomController.FriendList>(callback);
            //
            //     gameObject.SetActive(false);
            // }
        }
    }
}