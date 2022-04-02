using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantCollider : MonoBehaviour
{
    private Collider collider;
    float waitTime;
  

    void OnEnable()
    {
        collider = GetComponent<Collider>();
        waitTime = transform.parent.GetComponent<Restaurants>().restaurantData.waitTime;
        collider.enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InGame.UIManager.Instance.HideInputs();
            transform.GetChild(0).gameObject.SetActive(false);
            collider.enabled = false;
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
