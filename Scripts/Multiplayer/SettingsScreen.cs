using System.Collections;
using System.Collections.Generic;
using Authentication;
using UnityEngine;
using UnityEngine.UI;
public class SettingsScreen : MonoBehaviour
{
    [SerializeField]
    Button sound, music;
    [SerializeField]
    Sprite on,off;
   
  
    // Start is called before the first frame update
    void Start()
    {
        ChangeSprites();
      ;

      //  sound.onValueChanged.AddListener(delegate {
      //     ToggleValueChanged(sound);
      // });
      // music.onValueChanged.AddListener(delegate {
      // //    ToggleValueChangedMusic(music);
      //  });

        //Initialise the Text to say the first state of the Toggle

    }

    void ChangeSprites()
    {
        if (PlayerPrefs.GetInt("MUSICVFX", 1) == 1)
        {
            music.GetComponent<Image>().sprite = on;
        }
        else
        {
            music.GetComponent<Image>().sprite = off;
        }
        if (PlayerPrefs.GetInt("SOUNDVFX", 1) == 1)
        {
            sound.GetComponent<Image>().sprite = on;
        }
        else
        {
            sound.GetComponent<Image>().sprite = off;
        }
    }
    public void ToggleSound()
    {
        if (sound.GetComponent<Image>().sprite==off)
        {
            sound.GetComponent<Image>().sprite = on;
            PlayerPrefs.SetInt("SOUNDVFX", 1);
        }
        else{
            sound.GetComponent<Image>().sprite = off;
            PlayerPrefs.SetInt("SOUNDVFX", 0);
        }
     //   ChangeSprites();
    }
    public void ToggleMusic()
    {

        if (music.GetComponent<Image>().sprite == off)
        {
            PlayerPrefs.SetInt("MUSICVFX", 1);
            music.GetComponent<Image>().sprite = on;
        }
        else
        {
            music.GetComponent<Image>().sprite = off;
            PlayerPrefs.SetInt("MUSICVFX", 0);
        }
       // ChangeSprites();
    }
    void ToggleValueChangedMusic(Toggle change)
    {
        if (change.isOn)
        {
            PlayerPrefs.SetInt("MUSICVFX", 1);
        }
        else
        {
            PlayerPrefs.SetInt("MUSICVFX", 0);
        }

    }
    //Output the new state of the Toggle into Text
    void ToggleValueChanged(Toggle change)
    {
        if(change.isOn)
        {
            PlayerPrefs.SetInt("SOUNDVFX", 1);
        }
        else
        {
            PlayerPrefs.SetInt("SOUNDVFX",0);
        }
       
    }
   
    public void PrivacyPolicy()
    {
        RoomContoller.UIManager.instance.EnablePanel(RoomContoller.UIManager.instance.privacyPolicyPOpUP);
     //   Application.OpenURL("https://kissasian.land/kindaichi-shonen-no-jikenbo-5-2022-episode-5/");
    }
    public void TermsAndCOnditions()
    {
        Application.OpenURL("https://kissasian.land/kindaichi-shonen-no-jikenbo-5-2022-episode-5/");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
