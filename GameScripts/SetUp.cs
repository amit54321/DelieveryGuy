using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using KartGame.KartSystems;
using UnityEngine.SceneManagement;


public class SetUp : NetworkBehaviour
{
    [SerializeField] ArcadeKart arcadeKart;
    [SerializeField] KartAnimation kartAnimation;
    [SerializeField] KartPlayerAnimator kartPlayerAnimator;
  //  [SerializeField] ArcadeEngineAudio arcadeEngineAudio;
    // Start is called before the first frame update
    void OnEnable()
    {
        Debug.Log("enabling player");
        if (SceneManager.GetActiveScene().name.Equals("Lobby"))
        {

            ToggleScripts(false);
            //   arcadeEngineAudio.enabled = false;

        }
        SceneManager.sceneLoaded  += SceneLoaded;
    }
   

    public void ToggleScripts(bool toggle)
    {
        arcadeKart.enabled = toggle;
        kartAnimation.enabled = toggle;
        kartPlayerAnimator.enabled = toggle;
    }
    void SceneLoaded(Scene scene,LoadSceneMode sceneMode)
    {
        Debug.Log("SCENE LOADED " + scene.name);
        if (scene.name.Equals("Lobby"))
        {

            ToggleScripts(false);
         //   arcadeEngineAudio.enabled = false;

        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
