using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Authentication;

[Serializable]
public class TutorialStep
{
    public bool showHand;
    public Vector2 handPos;
    public string desc;

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
        Debug.LogError("SENDING STEP" + step);
        StartCoroutine(PostNetworkRequest(AuthenticationConstants.TUTORIALSTEP, data, RegisterCallBack, Error, false));
    }
    public void RegisterCallBack(string callback)
    {
    }
        // Update is called once per frame
        public void SetTutorial()
    {
        
        current++;
        Debug.LogError("STEPS  " + current);
        TutorialStep step = steps[current-1];
        if (step.showHand)
        {
            hand.anchoredPosition = step.handPos;
            hand.transform.GetComponent<Image>().enabled = true;
        }
        else
        {
            hand.transform.GetComponent<Image>().enabled = false;
        }
        descr.text = step.desc;
    }
}
