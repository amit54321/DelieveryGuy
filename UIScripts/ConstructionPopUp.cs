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
    public int cost, timer;
}
[System.Serializable]
public class RestaurantDataUIArray
{
    public List<RestaurantDataUI> data;
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
                c.SetData(r.id, r.title, r.desc, r.cost, 1, r.timer);

            }
        }
        else
        {
            TextAsset textAsset = Resources.Load<TextAsset>("restaurantData");
            RestaurantDataUIArray data = JsonUtility.FromJson<RestaurantDataUIArray>(textAsset.text);
            foreach (RestaurantDataUI r in data.data)
            {
                if(r.id== GameManager.Instance.clickedPlotId)
                statiConstructionPrefab.SetData(r.id, r.title, r.desc, r.cost, 1, r.timer);
            }
        }

        crossButton.onClick.AddListener(Cross);
    }

    void Cross()
    {
        InGame.UIManager.Instance.DisablePopUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
