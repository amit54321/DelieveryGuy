using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField]
    TextMeshPro text;
    [SerializeField]
    SpriteRenderer sprite;
    public int level,quantity,waitTime,restaurantId;

    public RestaurantCollider restaurantCollider;
    public new void OnCLickMethod()
    {
        UnityEngine.Debug.LogError("CLICKED REST " + id);
    }
    void Start()
    {
       
        
    }
    
    public IEnumerator StartConstruction(int id,int constructionTime,int level,int quantity,int waitTime,int resId)
    {
        this.id = id;
        this.level = level;
        this.quantity = quantity;
        this.waitTime = waitTime;
        this.restaurantId = resId;
        transform.GetComponent<Collider>().enabled = false;
        sprite.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        text.transform.LookAt(GameManager.Instance.cityCamera.transform);
        for(int i=0;i<constructionTime;i++)
        {
            text.text = (constructionTime - i).ToString()+ " s";
            yield return new WaitForSeconds(1);
        }
        sprite.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        transform.GetComponent<Collider>().enabled = true;

        GameManager.Instance.FindPlotById(id).enabled = false;


    }



}
