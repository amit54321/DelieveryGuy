using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum STATUS
{
    PLAY,
    SET
}
public class HomeScreen : MonoBehaviour
{
    [SerializeField]
    Button play, set;
    public static STATUS status;
    private void Start()
    {
        play.onClick.AddListener(SetPlayStatus);
        set.onClick.AddListener(SetSetStatus);
    }
    // Start is called before the first frame update
    public void SetPlayStatus()
    {
        status = STATUS.PLAY;
        SceneManager.LoadScene("GameScene");
    }

    public void SetSetStatus()
    {
        status = STATUS.SET;
        SceneManager.LoadScene("GameScene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
