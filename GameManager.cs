using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public List<Restaurants> restaurants;

    public Restaurants currentRestaurant;

    public ArrowHandler arrowHandler;

  
    // Start is called before the first frame update
    void Start()
    {
        currentRestaurant = restaurants[0];
        arrowHandler.SetArrow(currentRestaurant.transform);
    }

    
}
