using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] Text timerText, tasksDone,opponentTasksDone;
    int timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        timerText.text = timer.ToString();
        SetTasks(0);
        StartCoroutine(Timer());
    }

    public void SetTasks(int tasks)
    {
        tasksDone.text = "Tasks Done : "+tasks.ToString();
    }

    public void OpponentSetTasks(int tasks)
    {
        Debug.LogError("OPPONENT TASK UI" + tasks);
        opponentTasksDone.text = "Tasks Done : " + tasks.ToString();
    }

    string SecondsToHour(int totalSeconds)
    {
     int   timeSpanConversionHours = TimeSpan.FromSeconds(totalSeconds).Hours;
        int timeSpanConversiondMinutes = TimeSpan.FromSeconds(totalSeconds).Minutes;
        int timeSpanConversionSeconds = TimeSpan.FromSeconds(totalSeconds).Seconds;

        //Convert TimeSpan variables into strings for textfield display
       string textfieldHours = timeSpanConversionHours.ToString();
        string textfieldMinutes = timeSpanConversiondMinutes.ToString();
        string textfieldSeconds = timeSpanConversionSeconds.ToString();
        string s;
        //Display the time given the number of digits.
        if (textfieldMinutes.Length == 2 && textfieldSeconds.Length == 2)
        s= textfieldHours + ":" + textfieldMinutes + ":" + textfieldSeconds; 
        else if (textfieldMinutes.Length == 2 && textfieldSeconds.Length == 1)
        { s = textfieldHours + ":" + textfieldMinutes + ":0" + textfieldSeconds; }
        else if (textfieldMinutes.Length == 1 && textfieldSeconds.Length == 1)
        {s= textfieldHours + ":0" + textfieldMinutes + ":0" + textfieldSeconds; }
        else if (textfieldMinutes.Length == 1 && textfieldSeconds.Length == 2)
        { s= textfieldHours + ":0" + textfieldMinutes + ":" + textfieldSeconds; }
        else
        {s= textfieldHours + ":" + textfieldMinutes + ":" + textfieldSeconds; }
        return s;

    }
    // Update is called once per frame
    public IEnumerator  Timer()
    {
        while(!GameManager.Instance.gameOver)
        {
            yield return new WaitForSeconds(1);
            timerText.text = SecondsToHour(timer);//.ToString();
            timer++;
        }
        
    }
}
