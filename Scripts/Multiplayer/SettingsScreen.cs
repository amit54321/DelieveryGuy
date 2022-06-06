using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SettingsScreen : MonoBehaviour
{
    [SerializeField]
    Toggle sound, music;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("MUSICVFX", 1)==1)
        {
            music.isOn = true;
        }
        else
        {
            music.isOn = false;
        }
        if (PlayerPrefs.GetInt("SOUNDVFX", 1) == 1)
        {
            sound.isOn = true;
        }
        else
        {
            sound.isOn = false;
        }
        sound.onValueChanged.AddListener(delegate {
            ToggleValueChanged(sound);
        });
        music.onValueChanged.AddListener(delegate {
            ToggleValueChangedMusic(music);
        });

        //Initialise the Text to say the first state of the Toggle

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
        Application.OpenURL("https://kissasian.land/kindaichi-shonen-no-jikenbo-5-2022-episode-5/");
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
