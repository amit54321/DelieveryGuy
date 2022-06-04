using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ShareMenu : MonoBehaviour
{
    public InputField code;
    public Button share;
    private bool isProcessing = false;
    string destination;
    public string shareText = "Download This Game";
    public string gameLink = "Download the game on play store at " + "\nhttps://play.google.com/store/apps/details?id=com.CrazyDrivers";
    public string imageName = "MyPic"; // without the extension, for iinstance, MyPic 
    public void shareImage()
    {
        if(code!=null)
        {
            gameLink = "Add this code and play with me " + code.text;
        }
        if (!isProcessing)
            StartCoroutine(ShareScreenshot());

    }
    public void TakeScreenshot()
    {
        string folderPath = Application.persistentDataPath+"/Screenshots/";

        if (!System.IO.Directory.Exists(folderPath))
            System.IO.Directory.CreateDirectory(folderPath);

        var screenshotName =
                                "Screenshot_" +
                                System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") +
                                ".png";
        ScreenCapture.CaptureScreenshot(System.IO.Path.Combine(folderPath, screenshotName));
        destination = folderPath + screenshotName;
        Debug.Log(folderPath + screenshotName);
    }

    private IEnumerator ShareScreenshot()
    {
        TakeScreenshot();
        //isProcessing = true;
        yield return new WaitForSeconds(1.5f);

        //Texture2D screenTexture = new Texture2D(1080, 1080, TextureFormat.RGB24, true);
        //screenTexture.Apply();


        //string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        //Debug.Log(destination);

        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText + gameLink);
            intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

            currentActivity.Call("startActivity", intentObject);

        }

        isProcessing = false;

    }

}