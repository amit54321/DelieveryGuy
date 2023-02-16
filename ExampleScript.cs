using UnityEngine;

// Generate a screenshot and save it to disk with the name SomeLevel.png.

public class ExampleScript : MonoBehaviour
{//here you can set the folder you want to use, 
    //IMPORTANT - use "@" before the string, because this is a verbatim string
    //IMPORTANT - the folder must exists
    string pathToYourFile = @"C:\";
    //this is the name of the file
    string fileName = "filename";
    //this is the file type
    string fileType = ".png";

    private int CurrentScreenshot { get => PlayerPrefs.GetInt("ScreenShot"); set => PlayerPrefs.SetInt("ScreenShot", value); }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.ScreenCapture.CaptureScreenshot(Application.persistentDataPath + fileName + CurrentScreenshot + fileType);
            CurrentScreenshot++;
        }
    }
}