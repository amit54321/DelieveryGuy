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
        for (int i = 0; i < waitTime; i++)
        {
            yield return new WaitForSeconds(1);
        }
        InGame.UIManager.Instance.ShowInputs();
        collider.enabled = false;

    }
}
