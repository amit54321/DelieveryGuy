using System.Threading.Tasks;
using UnityEngine;
//using Google;
using UnityEngine.UI;
//using Firebase.Auth;
//using Facebook.Unity;
using SimpleJSON;
using UnityEngine.Networking;
using System.Collections;



public delegate void SocialNetwork();
public class googleSignIn : MonoBehaviour
{
    /*public static event SocialNetwork socialNetwork;
    private FirebaseAuth auth;
    private FirebaseUser FBuser;
   // public Text Info;
  //  public Text EmailId;
    string user_public_profile, user_email, user_friends;


    
   
    // Start is called before the first frame update
    void Start()
    {
        InitializeFirebase();
    }

    // Update is called once per frame
    

    void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

    //    auth.StateChanged += AuthStateChanged;
      //  AuthStateChanged(this, null);
    }

   
    
    public void GoogleSIgnIn()
    {

      
        GoogleSignIn.Configuration = new GoogleSignInConfiguration
        {
            RequestIdToken = true,
            RequestEmail = true,
            // Copy this value from the google-service.json file.
            // oauth_client with type == 3
            WebClientId = "102631975781-mmjthmjhcs00eg3kcn8e66vh05oaevho.apps.googleusercontent.com"
        };

        Task<GoogleSignInUser> signIn = GoogleSignIn.DefaultInstance.SignIn();
Debug.Log("GOOGLECALLING1");
        TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser>();
        signIn.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                signInCompleted.SetCanceled();
             //   Info.text = "canceled  1 " + FBuser.UserId.ToString();
            }
            else if (task.IsFaulted)
            {
                signInCompleted.SetException(task.Exception);
               // Info.text = "is faulted 1 " + FBuser.UserId.ToString();
            }
            else
            {

                Credential credential = GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
                auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
                {
                    if (authTask.IsCanceled)
                    {
                        signInCompleted.SetCanceled();
                      //  Info.text = "canceled  " + FBuser.UserId.ToString();
                    }
                    else if (authTask.IsFaulted)
                    {
                        signInCompleted.SetException(authTask.Exception);
                       // Info.text = "is faulted  " + FBuser.UserId.ToString();
                    }
                    else
                    {
                        Debug.Log("GOOGLECALLING2");
                     string photourl =    FBuser.PhotoUrl.ToString();
//Application.OpenURL(photourl);
              //   PlayerPrefs.SetString(Constants.PlayerPrefs.Email,FBuser.Email.ToString());
               //  PlayerPrefs.SetString(Constants.PlayerPrefs.username,FBuser.DisplayName.ToString());
                    
               
               //StartCoroutine(GetText(photourl));
                      
                    //    if(socialNetwork!=null)
              //  socialNetwork();
                        signInCompleted.SetResult(authTask.Result);
                      //  Info.text = "sign in  " + FBuser.UserId.ToString() + " " + FBuser.DisplayName.ToString();
                      //  EmailId.text = FBuser.Email.ToString();
                    }
                });
            }
        });
    }


    /*
    #region FACEBOOK

    private void FbInitialized()
    {
        if (!FB.IsInitialized)
        {
            FB.Init();
        }
        else
        {
            FB.ActivateApp();
        }
    }

    // Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            bool signedIn = FBuser != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && FBuser != null)
            {
              //  Debug.Log("Signed out " + FBuser.UserId);
             //   Info.text = "sign out " + FBuser.UserId.ToString();
            }
            FBuser = auth.CurrentUser;
            if (signedIn)
            {
              //  Debug.Log("Signed in " + FBuser.UserId);
             //   Info.text = "sign in " + FBuser.UserId.ToString();
            }
        }
        FbInitialized();
    }

    public void FaceBookLogIn()
    {
        FB.LogInWithReadPermissions(callback: OnLogIn);
    }
    private void OnLogIn(ILoginResult result)
    {
        if (FB.IsLoggedIn)
        {
            AccessToken tocken = AccessToken.CurrentAccessToken;
         //  EmailId.text = tocken.UserId;
            FB.API("me?fields=id,email,first_name,last_name,friends.limit(100).fields(first_name,last_name,id)", HttpMethod.GET, (authresult) =>
            {
               
                JSONObject json = (JSONObject)JSON.Parse(authresult.RawResult);
                Debug.Log("JSON  "+authresult.RawResult);
                string name =  json["name"];
                string id = json["id"];


               // PlayerPrefs.SetString(Constants.PlayerPrefs.Email,id);
              //  Info.text = name;
             FB.API ("/me/picture?type=square&height=200&width=200", HttpMethod.GET, UpdateProfileImage);
                
                Debug.Log(name + "  name  " +  id + " id ");
             //    PlayerPrefs.SetString(Constants.PlayerPrefs.username,name);
            }
            );
            Credential credential = FacebookAuthProvider.GetCredential(tocken.TokenString);

        }
        else
        {
            Debug.Log("Login Failed");
        }
    }

    IEnumerator GetText(string url)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                 profileImage = texture;
                if(socialNetwork!=null)
                socialNetwork();
            }
        }
    }

public static Texture2D profileImage;
private void UpdateProfileImage(IGraphResult result) {
    
    if(result.Texture != null) {
        profileImage = result.Texture;
    }
   if(socialNetwork!=null)
   {
       Debug.Log("RESULTR  "+result.Texture);
                socialNetwork();
   }
}
public void SignOut()
{
    FB.LogOut();
   
}

    public void accessToken(Credential firebaseResult)
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        if (!FB.IsLoggedIn)
        {
            return;
        }

        auth.SignInWithCredentialAsync(firebaseResult).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            FirebaseUser newUser = task.Result;
           // Info.text = newUser.DisplayName;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }
    
    #endregion#1#*/
}

//
// public class Credentials
// {
//     public string name;
//     public string id;
//
//
// }
