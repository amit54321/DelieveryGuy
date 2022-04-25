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
       
    }

   public void EnableCollider()
    {
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
            GameManager.Instance.StopPlayer(transform);
            StartCoroutine(StartTimer());

        }
    }

    IEnumerator StartTimer()
    {
        GameManager.Instance.timer.ToggleTimer(true);
        for (int i = 0; i < waitTime; i++)
        {
            GameManager.Instance.timer.Set("Cooking ... ", (int)waitTime, i);

            yield return new WaitForSeconds(1);
        }
        GameManager.Instance.timer.ToggleTimer(false);
        InGame.UIManager.Instance.ShowInputs();
        collider.enabled = false;
        GameManager.Instance.player.GetComponent<Rigidbody>().isKinematic = false;
        GameManager.Instance.EnableCurrentTaskHouse();
        

    }
}
