using RoomContoller;
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
    [SerializeField]
    GameObject dish;
  //  public int level,quantity,waitTime,restaurantId;

    public RestaurantCollider restaurantCollider;

    Vector3 forwardP = new Vector3(2.1f, 24.3f, 0.66f);
    Vector3 forwardR = new Vector3(90, 0, 180);
    Vector3 forwardTP = new Vector3(0, -3.14f,0);
    Vector3 forwardTR = new Vector3(0, 0, 0);

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
        this.forward = transform.parent.GetComponent<Plot>().forward;
        this.restaurantData.level = level;
        this.restaurantData.quantity = quantity;
        this.restaurantData.waitTime = waitTime;
        this.restaurantData.id = resId;
        transform.GetComponent<Collider>().enabled = false;
        sprite.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        SetPositonOfRestaurant();
        if (this.forward)
        {
            text.transform.localPosition = forwardTP;
            text.transform.localEulerAngles = forwardTR;
            sprite.transform.localPosition = forwardP;
            sprite.transform.localEulerAngles = forwardR;
        }
        //  text.transform.LookAt(GameManager.Instance.cityCamera.transform);
        for (int i=0;i<constructionTime;i++)
        {
           // sprite.transform.localScale = Vector3.one * (1 / ((float)constructionTime+1));
            text.text = (constructionTime - i).ToString()+ " s";
            yield return new WaitForSeconds(1);
        }
      

       
    }

    void SetPositonOfRestaurant()
    {
        if(dish==null)
        {
            return;
        }
        if(this.forward)
        {
            if(restaurantData.id==1)
            {
               dish.transform.localEulerAngles = new Vector3(15, 0, 0);
            }
           else if (restaurantData.id == 4)
            {
                dish.transform.localEulerAngles = new Vector3(-65, 0, 0);
            }
            else 
            {
                dish.transform.localEulerAngles = new Vector3(15, 0, 0);
            }
        }
    }

    public void ConstructionFinished(bool newData)
    {
        Debug.LogWarning("CONSTRUCTION FINISHED NOW");
        foreach(Restaurants r in GameManager.Instance.allRestaurants)
        {
            if(r.restaurantData.id == restaurantData.id && r.restaurantData.level == restaurantData.level)

            {
                return;
            }
        }
        if(newData)
        SocketMaster.instance.StartCoroutine(SocketMaster.instance.SendMissions(new List<int>() { 3, 4, 5, 6 }));
        GameManager.Instance.allRestaurants.Add(this);
        InGame.UIManager.Instance.restaurantPopUp.SetData();
        sprite.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
        transform.GetComponent<Collider>().enabled = true;

        GameManager.Instance.FindPlotById(id).enabled = false;
    }



}
