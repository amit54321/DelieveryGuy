using UnityEngine;
using UnityEngine.UI;
using Authentication;
using UnityEngine.SceneManagement;
//using Google;

public class UserProfile : WebRequest
{
    [SerializeField] private Text name, matchesPlayed, matcheswon, totalcoins;
    [SerializeField] public RawImage profileImage;
    [SerializeField] private Sprite defaultiImage;
    [SerializeField] public Sprite[] profileSprites;

    [SerializeField] private GameObject profileChangePopUo;

    void OnEnable()
    {
        RoomContoller.UIManager.instance.ToggleLoader(true);
        WWWForm form = new WWWForm();
        StartCoroutine(PostRequest(AuthenticationConstants.USERPROFILE, form, GetProfileCallBack, Error,
            true));
    }


    public void GetProfileCallBack(string callback)
    {
        Debug.Log("CALLBACK " + callback);
        RoomContoller.UIManager.instance.ToggleLoader(false);
     //   DataRoomController.USERPROFILEDATA data = JsonUtility.FromJson<DataRoomController.USERPROFILEDATA>(callback);
        /*if (data.status == 200)
        {
            
            StartCoroutine(DownloadImage.LoadRawImage(data.result.user.image_file,
                profileImage));
            name.text = data.result.user.first_name + " " + data.result.user.last_name;
            matchesPlayed.text = data.result.bluff.total.ToString();
            matcheswon.text = data.result.bluff.wins.ToString();
        }*/
   
    }

    public void LogOut()
    {
        PlayerPrefs.SetString(PlayerPrefsData.PASSWORD, "");
        PlayerPrefs.SetString(PlayerPrefsData.EMAIL, "");
        //if (PlayerPrefs.GetInt("LoginType") == 2)
        //{
        //    FB.LogOut();
        //}
        //else if (PlayerPrefs.GetInt("LoginType") == 3)
        //{
        //    GoogleSignIn.DefaultInstance.SignOut();
        //}

        PlayerPrefs.SetInt("LoginType", 100);
        SceneManager.LoadScene("Authentication");
    }

    // public void SetProfilePic(string url)
    // {
    //     Debug.Log(url+"  yugyugyu");
    //     WWWForm form = new WWWForm();
    //     form.AddField("image", url);
    //     StartCoroutine(PostRequest(AuthenticationConstants.PROFILE_PIC, form, SetProfilePicCallback, Error,true));
    //                 
    // }
    //
    public void SetProfilePicCallback(string url)
    {
        Authentication.Authentication.userProfile.avatar = url;
        SetProfileImage();
    }

    void Error(string error)
    {
        Debug.Log("CALLBACK " + error);
    }

    public void OpenProfileCahangePopUp()
    {
        profileChangePopUo.SetActive(true);
    }

    public void SetProfileImage()
    {
        StartCoroutine(DownloadImage.LoadRawImage(Authentication.Authentication.userProfile.avatar,
            profileImage));
        profileImage.texture = Authentication.Authentication.profileImage;
    }
}

[System.Serializable]
public class UserProfileData
{
    public UserProfileModel data;
}

//{"data":{"firstName":"t","lastName":"t","image":"dots1","won_matches":0,"played_matches":0}}
[System.Serializable]
public class UserProfileModel
{
    public int won_matches;
    public int played_matches;
}