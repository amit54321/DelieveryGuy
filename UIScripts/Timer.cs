using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] Image timerImage;
    [SerializeField] Text timerText, statusText;
    
    // Start is called before the first frame update
   public void Set(string dish,int timer,int current)
    {
        
        timerText.text = (timer - current).ToString() + " s";
       
        timerImage.fillAmount =(float)( (float)current / (float)timer);
        statusText.text = dish;
    }

   
    public void ToggleTimer(bool active)
    {
        transform.gameObject.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
