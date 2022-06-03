using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSOund : MonoBehaviour
{
    [SerializeField]
    AudioSource source;

    public static ButtonSOund instance;
    // Start is called before the first frame update
    public void Play()
    {
        source.Play();
    }
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        source = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
