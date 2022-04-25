using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;
using LitJson;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


namespace Authentication
{

    [System.Serializable]
    public class RegisterCallback
    {
        public string message;
        public ProfileData data;
    }

    [System.Serializable]
    public class ProfileData
    {
        public string id;
        public string _id;
        public string name;
        public string deviceId;
        public int coins;
        public List<RestaurantsData> restaurants;
        public List<TimersData> timers;

    }

    [System.Serializable]
    public class RestaurantsData
    {

    }
    [System.Serializable]
    public class TimersData
    {

    }


    [System.Serializable]
    public class FacebookFriends
    {
        public string id;
        public string first_name;
        public string last_name;
        public string imageUrl;
    }


    public class Authentication : WebRequest
    {

        public  ProfileData profileData;


        public static bool socketConnected;
        public static List<FacebookFriends> facebookFriends = new List<FacebookFriends>();
        public static LobbyData.UserProfile userProfile;
        private string _tempPassword;
        private string _tempEmail;
        [FormerlySerializedAs("_tempOtp")] public int tempOtp;
        public static Texture profileImage;

        public static List<LobbyData.ChatData> allChats = new List<LobbyData.ChatData>();
        public static List<LobbyData.MissionData> allMissions = new List<LobbyData.MissionData>();

        void SavePlayerPrefs(string email, string name, string token, string password,
            string id, string socialid)
        {
            PlayerPrefs.SetString(PlayerPrefsData.ID, id);
            PlayerPrefs.SetString(PlayerPrefsData.SOCIALID, socialid);
            PlayerPrefs.SetString(PlayerPrefsData.PASSWORD, password);
            PlayerPrefs.SetString(PlayerPrefsData.EMAIL, email);
            PlayerPrefs.SetString(PlayerPrefsData.FIRSTNAME, name);
            PlayerPrefs.SetString(PlayerPrefsData.TOKEN, token);
        }

        public void Register()
        {
           Dictionary<string, object> data = new Dictionary<string, object>()
           {
              
                 {"deviceId","bhjbjhbhj" }// SystemInfo.deviceUniqueIdentifier}

            };
           
            StartCoroutine(PostNetworkRequest(AuthenticationConstants.REGISTER ,data, RegisterCallBack, Error, false));
            UIManager.instance.ToggleLoadingPanel(true);
        }


        public void UpdateName(string name)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
           {

                 {"id",  PlayerPrefs.GetString("ID")},
                 {"name", name}

            };

            StartCoroutine(PostNetworkRequest(AuthenticationConstants.UPDATENAME, data, UpdateNameCallBack, Error, false));
            UIManager.instance.ToggleLoadingPanel(true);
        }
        public void RegisterCallBack(string callback)
        {
            Debug.Log("LOGIN CALLS" + callback);
            UIManager.instance.ToggleLoadingPanel(false);
            
                ProfileData data = JsonUtility.FromJson<ProfileData>(callback);
                PlayerPrefs.SetString("ID", data._id);
                profileData = data;
            RoomContoller.SocketMaster.instance.InitialiseSocket();
            if (string.IsNullOrEmpty(profileData.name))
                {
                    UIManager.instance.EnableScreen(UIManager.instance.loginScreen);
                }
                else
                {
                    SceneManager.LoadScene("Lobby");
                }
            {
              
               
            }
        }
        public void UpdateNameCallBack(string callback)
        {
            Debug.Log("LOGIN CALLS" + callback);
            UIManager.instance.ToggleLoadingPanel(false);
            ProfileData data = JsonUtility.FromJson<ProfileData>(callback);
            {
                PlayerPrefs.SetString("ID", data._id);
                profileData = data;
                SceneManager.LoadScene("Lobby");
            }
            {


            }
        }























        public void UpdateUser(string name,string avatar)
        {
          avatar=  "kaiser/" + Random.Range(0,4).ToString();
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"name", name},
                 {"avatar", avatar },
               //   {"deviceId", "djjddd"},
                {"deviceId", SystemInfo.deviceUniqueIdentifier}
            };

            StartCoroutine(PostNetworkRequest(AuthenticationConstants.UPDATEUSER, data, UpdateUserCallBack, Error, false));
            UIManager.instance.ToggleLoadingPanel(true);
        }

        public void UpdateUserCallBack(string callback)
        {
            Debug.Log("LOGIN CALLS" + callback);
            UIManager.instance.ToggleLoadingPanel(false);
            LobbyData.DefaultAUth data = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
            if (data.status == 200)
            {
                SavePlayerPrefs(data.message.email, data.message.name, "Bearer " + data.message.token,
                    _tempPassword, data.message._id, "");
                userProfile = data.message;
                allChats = data.chatPacks;
                allMissions = data.missions;
                SceneManager.LoadScene("Lobby");

            }
            else
            {
                UIManager.instance.ShowError(data.error);
                Debug.Log("MESSAGE ERROR " + data.error);
            }
        }









        public void Login(string email, string password)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"email", email},
                {"password", password}
            };
            _tempPassword = password;
           // StartCoroutine(PostNetworkRequest(AuthenticationConstants.LOGIN, data, LoginCallBack, Error, false));
            UIManager.instance.ToggleLoadingPanel(true);
        }

        public void LoginCallBack(string callback)
        {
            Debug.Log("LOGIN CALLS" + callback);
            UIManager.instance.ToggleLoadingPanel(false);
            LobbyData.DefaultAUth data = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
            if (data.status == 200)
            {
                SavePlayerPrefs(data.message.email, data.message.name, "Bearer " + data.message.token,
                    _tempPassword, data.message._id, "");
                userProfile = data.message;
                allChats = data.chatPacks;
                allMissions = data.missions;
                PlayerPrefs.SetInt("login", 1);
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                UIManager.instance.ShowError(data.error);
                Debug.Log("MESSAGE ERROR " + data.error);
            }
        }

        public void LoginBySocial(string email, string password, string firstName, string lastName,
            string socialId, string url)
        {
            Debug.Log("LOGIN CALLS" + firstName + "  " + lastName);
            WWWForm form = new WWWForm();
            _tempPassword = password;
            form.AddField("email", email);
            form.AddField("password", password);
            form.AddField("first_name", firstName);
            form.AddField("last_name", lastName);
            form.AddField("social_id", socialId);
            form.AddField("profile_link", url);
            UIManager.instance.ToggleLoadingPanel(true);
            StartCoroutine(PostRequest(AuthenticationConstants.SIGNUP, form, LoginBySocialCallBack, Error, false));
        }

        public void LoginBySocialCallBack(string callback)
        {
            UIManager.instance.ToggleLoadingPanel(false);

            LobbyData.DefaultAUth data = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
            if (data.status == 200)
            {
                SavePlayerPrefs(data.message.email, data.message.name, "Bearer " + data.message.token,
                    _tempPassword, data.message._id, data.message.social_id);
                userProfile = data.message;
                PlayerPrefs.SetInt("login", 1);
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                UIManager.instance.ShowError(data.error);
                Debug.Log("MESSAGE ERROR " + data.message);
            }
        }


        public void SignUp(string email, string password, string firstName, string lastName)
        {
            Dictionary<string, object> data = new Dictionary<string, object>()
            {
                {"email", email},
                {"password", password},
                {"name", firstName}
            };
            _tempPassword = password;
            UIManager.instance.ToggleLoadingPanel(true);
            StartCoroutine(PostNetworkRequest(AuthenticationConstants.SIGNUP, data, SignUpCallBack, Error, false));
        }

        public void SignUpCallBack(string callback)
        {
            UIManager.instance.ToggleLoadingPanel(false);
            LobbyData.DefaultAUth data = JsonUtility.FromJson<LobbyData.DefaultAUth>(callback);
            if (data.status == 200)
            {
                SavePlayerPrefs(data.message.email, data.message.name, "Bearer " + data.message.token,
                    _tempPassword, data.message._id, "");
                userProfile = data.message;
                socketConnected = false;
                allChats = data.chatPacks;
                allMissions = data.missions;
                Debug.Log("CHATS " + allChats.Count);
                PlayerPrefs.SetInt("login", 1);
                SceneManager.LoadScene("Lobby");
            }
            else
            {
                UIManager.instance.ShowError(data.error);
            }
        }


        public void ForgetPassword(string email)
        {
            WWWForm form = new WWWForm();
            form.AddField("emailId", email);
            _tempEmail = email;
            UIManager.instance.ToggleLoadingPanel(true);
            StartCoroutine(PostRequest(AuthenticationConstants.FORGETPASSWORD, form, ForgetPasswordCallBack, Error,
                false));
        }

        public void ForgetPasswordCallBack(string callback)
        {
            UIManager.instance.ToggleLoadingPanel(false);
            /*AuthenticationData.ForgetPasswordCallBack data =
                JsonUtility.FromJson<AuthenticationData.ForgetPasswordCallBack>(callback);
            if (data.success)
            {
                UIManager.instance.EnableScreen(UIManager.instance.verifyOTPscreen);
                Debug.Log("MESSAGE SUCCESS " + data.otp);
            }
            else
            {
                UIManager.instance.ShowError(data.message);
                Debug.Log("MESSAGE ERROR " + data.message);
            }*/
        }


        public void VerifyOTP(int otp)
        {
            WWWForm form = new WWWForm();
            form.AddField("emailId", _tempEmail);
            form.AddField("otp", otp);
            tempOtp = otp;
            UIManager.instance.ToggleLoadingPanel(true);

            StartCoroutine(PostRequest(AuthenticationConstants.VERIFYOTP, form, VerifyOTPCallBack, Error, false));
        }

        public void VerifyOTPCallBack(string callback)
        {
            UIManager.instance.ToggleLoadingPanel(false);
            /*AuthenticationData.VerifyOTPCallBack data =
                JsonUtility.FromJson<AuthenticationData.VerifyOTPCallBack>(callback);
            if (data.success)
            {
                UIManager.instance.EnableScreen(UIManager.instance.resetPasswordScreen);
                Debug.Log("MESSAGE SUCCESS " + data.message);
            }
            else
            {
                UIManager.instance.ShowError(data.message);
                Debug.Log("MESSAGE ERROR " + data.message);
            }*/
        }


        public void ResetPassword(string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("emailId", _tempEmail);
            form.AddField("otp", tempOtp);
            form.AddField("password", password);
            UIManager.instance.ToggleLoadingPanel(true);
            StartCoroutine(
                PostRequest(AuthenticationConstants.RESETPASSWORD, form, ResetPasswordCallBack, Error, false));
        }

        public void ResetPasswordCallBack(string callback)
        {
            /*AuthenticationData.ResetPasswordCallBack data =
                JsonUtility.FromJson<AuthenticationData.ResetPasswordCallBack>(callback);
            UIManager.instance.ToggleLoadingPanel(false);

            if (data.success)
            {
                UIManager.instance.EnableScreen(UIManager.instance.loginScreen);
                Debug.Log("MESSAGE SUCCESS " + data.message);
            }
            else
            {
                UIManager.instance.ShowError(data.message);

                Debug.Log("MESSAGE ERROR " + data.message);
            }*/
        }


        public void ChangePassword(string newPassword, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("newPassword", newPassword);
            form.AddField("password", password);
            StartCoroutine(PostRequest(AuthenticationConstants.CHANGEPASSWORD, form, ChangePasswordCallBack, Error,
                false));
        }

        public void ChangePasswordCallBack(string callback)
        {
            /*AuthenticationData.ChangePasswordCallBack data =
                JsonUtility.FromJson<AuthenticationData.ChangePasswordCallBack>(callback);*/
        }


        public void Error(string error)
        {
            UIManager.instance.ShowError(error);
        }
    }
}