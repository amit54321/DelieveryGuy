using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class RestaurantDataUI
{
    public int id;
    public string title, desc;
    public List<RestaurantLevel> levels;
}
[System.Serializable]
public class RestaurantDataUIArray
{
    public List<RestaurantDataUI> data;
}

[System.Serializable]
public class RestaurantLevel
{
    public int cost, timer,cookTime,quantity;
}
    public class ConstructionPopUp : BasePOpUp
{
    [SerializeField]
    Button crossButton;
    public ConstructionPrefab constructionPrefab;
    public Transform parent;
    [SerializeField]
    bool constructionPopUp;

    [SerializeField]
    ConstructionPrefab statiConstructionPrefab;
    public ScrollRect scrollRect;


    // Start is called before the first frame update
    void OnEnable()
    {
        if (constructionPopUp)
        {
            foreach (Transform t in parent)
            {
                Destroy(t.gameObject);
            }

          
            foreach (RestaurantDataUI r in GameManager.Instance.data.data)
            {
                bool show = true;
                foreach (Authentication.RestaurantsData res in RoomContoller.SocketMaster.instance.profileData.restaurants)
                    {
                    if(res.restaurant_id == r.id)
                    {
                        show = false;
                    }

                    }
                foreach (Authentication.TimersData res in RoomContoller.SocketMaster.instance.profileData.timers)
                {
                    if (res.restaurant_id == r.id)
                    {
                        show = false;
                    }

                }


                if (show)
                {
                    ConstructionPrefab c = Instantiate(constructionPrefab, parent);
                    c.SetData(r.id, r.title, r.desc, r.levels[0].cost, 0, r.levels[0].timer, r.levels[0].quantity, r.levels[0].cookTime);
                }
            }
        }
        else
        {
          
         //   TextAsset textAsset = Resources.Load<TextAsset>("restaurantData");
          //  RestaurantDataUIArray data = JsonUtility.FromJson<RestaurantDataUIArray>(textAsset.text);
            Restaurants res = GameManager.Instance.FindRestaurantById(GameManager.Instance.clickedPlotId);
            if (res != null)
            {
                foreach (RestaurantDataUI r in GameManager.Instance.data.data)
                {
                    UnityEngine.Debug.LogError("INITIAL  " + r.id + "     " + res.restaurantData.id);
                    if (r.id == res.restaurantData.id)
                    {


                        statiConstructionPrefab.SetData(r.id, r.title, r.desc, r.levels[res.restaurantData.level].cost, res.restaurantData.level, r.levels[res.restaurantData.level].timer,
                             r.levels[res.restaurantData.level].quantity, r.levels[res.restaurantData.level].cookTime);
                    }
                }
            }
        }
        if (InGame.UIManager.Instance.tutorialUI.activeSelf)
        {
            scrollRect.enabled = false;
        }
        crossButton.onClick.AddListener(Cross);

        
    }
   
    void Cross()
    {
        if(InGame.UIManager.Instance.tutorialUI.activeSelf)
        {
            return;
        }
        InGame.UIManager.Instance.DisablePopUp();
    }
    private void OnDisable()
    {
       
       // crossButton.onClick.RemoveListener(Cross);
    }
}
