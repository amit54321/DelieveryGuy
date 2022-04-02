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
    // Start is called before the first frame update
    void OnEnable()
    {
        if (constructionPopUp)
        {
            foreach (Transform t in parent)
            {
                Destroy(t.gameObject);
            }

            TextAsset textAsset = Resources.Load<TextAsset>("restaurantData");
            RestaurantDataUIArray data = JsonUtility.FromJson<RestaurantDataUIArray>(textAsset.text);
            foreach (RestaurantDataUI r in data.data)
            {
                ConstructionPrefab c = Instantiate(constructionPrefab, parent);
                c.SetData(r.id, r.title, r.desc, r.levels[0].cost, 0, r.levels[0].timer,r.levels[0].quantity,r.levels[0].cookTime);

            }
        }
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>("restaurantData");
            RestaurantDataUIArray data = JsonUtility.FromJson<RestaurantDataUIArray>(textAsset.text);
            Restaurants res = GameManager.Instance.FindRestaurantById(GameManager.Instance.clickedPlotId);
            foreach (RestaurantDataUI r in data.data)
            {

                if (r.id == res.restaurantId)
                {
                  
                    Debug.LogError("INITIAL  " + res.level);
                    statiConstructionPrefab.SetData(r.id, r.title, r.desc, r.levels[res.level].cost, res.level+1, r.levels[res.level].timer,
                         r.levels[res.level].quantity, r.levels[res.level].cookTime);
                }
            }
        }

        crossButton.onClick.AddListener(Cross);
    }

    void Cross()
    {
        InGame.UIManager.Instance.DisablePopUp();
    }

}
