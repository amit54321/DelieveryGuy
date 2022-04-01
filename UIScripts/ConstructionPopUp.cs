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
    // Start is called before the first frame update
    void OnEnable()
    {
        foreach(Transform t in parent)
        {
            Destroy(t.gameObject);
        }
        crossButton.onClick.AddListener(Cross);
        TextAsset textAsset = Resources.Load<TextAsset>("restaurantData");
        RestaurantDataUIArray data =  JsonUtility.FromJson<RestaurantDataUIArray>(textAsset.text);
        foreach(RestaurantDataUI r in data.data)
        {
            ConstructionPrefab c = Instantiate(constructionPrefab,parent);
            c.SetData(r.id, r.title, r.desc, r.cost, 1, r.timer);

        }
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
