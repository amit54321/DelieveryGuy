using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RestaurantMenuPrefab : MonoBehaviour
{
    [SerializeField]
    Image icon;
    int id;
    // Start is called before the first frame update
    public void SetData(int id,int restId)
    {
        this.id = id;
        icon.sprite = Resources.Load<Sprite>("Prefabs/RestaurantImage/" + restId);
    }

    // Update is called once per frame
   public void OnClick()
    {
        GameManager.Instance.SetCityCameraPosition(id);
    }
}
