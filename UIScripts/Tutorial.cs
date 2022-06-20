using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Authentication;
using UnityEngine.SceneManagement;

[Serializable]
public class TutorialStep
{
    public bool showHand;
    public Vector2 handPos;
    public string desc;
    public Vector2 descPos;

}
public class Tutorial : WebRequest
{
    public List<TutorialStep> steps;
    public RectTransform hand;
    public Text descr;
    public int current=-1 ;
    // Start is called before the first frame update
    void OnEnable()
    {
        SetTutorial();
    }
    public void SenStep(int step)
    {
        Dictionary<string, object> data = new Dictionary<string, object>()
           {

               {"id",PlayerPrefs.GetString(PlayerPrefsData.ID) },// SystemInfo.deviceUniqueIdentifier},
                 {"step", step},
            };
      
        StartCoroutine(PostNetworkRequest(AuthenticationConstants.TUTORIALSTEP, data, RegisterCallBack, Error, false));
    }
    public void RegisterCallBack(string callback)
    {
        TutorialFinishedCallBack data = JsonUtility.FromJson<TutorialFinishedCallBack>(callback);
        Debug.LogError("GETTING  DATA" + data.message);
        if (data.status == 200)
        {

            RoomContoller.SocketMaster.instance.profileData = data.message;
            SceneManager.LoadScene("Lobby");
        }
        }
        // Update is called once per frame
        public void SetTutorial()
    {
        
        current++;
        Dictionary<string, object> d = new Dictionary<string, object>();
        d.Add("step", current);
        Analytics.SendAnalytics(Analytics.Tutorial, d);
        Debug.LogError("STEPS  " + current);
        if(current>5)
        {
            return;
        }
        TutorialStep step = steps[current-1];
        if (step.showHand)
        {
            hand.anchoredPosition = step.handPos;
            hand.transform.GetComponent<Image>().enabled = true;
            descr.transform.GetComponent<RectTransform>().anchoredPosition = step.descPos;
        }
        else
        {
            hand.transform.GetComponent<Image>().enabled = false;
        }
        descr.text = step.desc;
    }
}
