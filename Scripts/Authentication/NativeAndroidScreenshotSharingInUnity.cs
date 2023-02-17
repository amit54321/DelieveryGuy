using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class NativeAndroidScreenshotSharingInUnity : MonoBehaviour
{
	public InputField code;

	public Button shareButton;

	private bool isFocus = false;

	public string shareSubject, shareMessage;
	private bool isProcessing = false;
	private string screenshotName;

	void Start()
	{
		

		shareButton.onClick.AddListener(OnShareButtonClick);
	}


	void OnApplicationFocus(bool focus)
	{
		isFocus = focus;
	}

	public void OnShareButtonClick()
	{

		screenshotName = "fireblock_highscore.png";
		shareSubject = "Play with me, download the game Delievery Boy";
		shareMessage =
        "Get the Delievery Boy from the link below. \nCheers\n" +
        "\nhttps://play.google.com/store/apps/details?id=com.kaiser.delieveryboy";

		if (code != null)
		{
			shareSubject = code.text;
			shareMessage = "Play with me by joining the room with code " + code.text;
		}
		ShareScreenshot();
	}


	private void ShareScreenshot()
	{

#if UNITY_ANDROID
		if (!isProcessing)
		{
			StartCoroutine(TakeScreenshotAndShare());
		}

#else
		Debug.Log("No sharing set up for this platform.");
#endif
	}

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject(shareSubject).SetText(shareMessage).SetUrl("https://github.com/yasirkula/UnityNativeShare")
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();

		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}

#if UNITY_ANDROID
	public IEnumerator ShareScreenshotInAnroid()
	{

		isProcessing = true;
		// wait for graphics to render
		yield return new WaitForEndOfFrame();

		string screenShotPath = Application.persistentDataPath + "/" + screenshotName;
		ScreenCapture.CaptureScreenshot(screenshotName, 1);
		yield return new WaitForSeconds(0.5f);

		if (!Application.isEditor)
		{
			
			//Create intent for action send
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

			//create image URI to add it to the intent
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + screenShotPath);

			//put image and string extra
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
			intentObject.Call<AndroidJavaObject>("setType", "image/png");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
			currentActivity.Call("startActivity", chooser);
		}

		yield return new WaitUntil(() => isFocus);
		isProcessing = false;
	}
#endif
}