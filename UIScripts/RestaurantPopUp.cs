using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantPopUp : MonoBehaviour
{
    [SerializeField]
    RestaurantMenuPrefab rPrefab;
    [SerializeField]
    Transform parent;

    // Start is called before the first frame update
   public void SetData()
    {
        foreach(Transform t in parent)
        {
            Destroy(t.gameObject);
        }
        List<int> plots = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        foreach (Restaurants r in GameManager.Instance.allRestaurants)
        {
            RestaurantMenuPrefab task = Instantiate(rPrefab, parent);
            task.SetData(r.id,r.restaurantData.id, r.restaurantData.level);
            plots.Remove(r.id);
        }

        foreach (int r in plots)
        {
            RestaurantMenuPrefab task = Instantiate(rPrefab, parent);
            task.SetData(r, 11,-1);
          
        }
        }

    // Update is called once per frame
    void Update()
    {
        
    }
}
