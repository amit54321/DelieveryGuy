using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollider : MonoBehaviour
{

    private Collider collider;
    float waitTime;
    public int id;

    void OnEnable()
    {
        collider = GetComponent<Collider>();
        waitTime = 3;
        
    }

    public void EnableHouse()
    {
        GetComponent<MeshRenderer>().enabled = true;
        collider.enabled = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InGame.UIManager.Instance.HideInputs();
            collider.enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            StartCoroutine(StartTimer());

        }
    }

    IEnumerator StartTimer()
    {
        GameManager.Instance.timer.ToggleTimer(true);
        for (int i = 0; i < waitTime; i++)
        {
            GameManager.Instance.timer.Set("Delievering ...",(int)waitTime,i);
            yield return new WaitForSeconds(1);
        }
        if (GameManager.Instance.DisableCurrentTask())
            yield break
              ;
        GameManager.Instance.timer.ToggleTimer(false);
        collider.enabled = false;
        InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.tasksPopUp);

    }
}
