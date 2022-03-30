using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RestaurantData 
{
    public int level;
    public int id;
    public string name;
    public int dish;
    public float waitTime;
    public int quantity;
}
public class Restaurants : Plot
{
    public RestaurantData restaurantData;
    private Collider collider;

    public new void OnCLickMethod()
    {
        UnityEngine.Debug.LogError("CLICKED REST " + id);
    }
    void Start()
    {
        collider = GetComponent<Collider>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InGame.UIManager.Instance.HideInputs();
            transform.GetChild(0).gameObject.SetActive(false);
            StartCoroutine(StartTimer());
          
        }
    }

    IEnumerator StartTimer()
    {
        for (int i = 0; i < restaurantData.waitTime; i++)
        {
            yield return new WaitForSeconds(1);
        }
        InGame.UIManager.Instance.ShowInputs();
        collider.enabled = false;

    }
   
}
