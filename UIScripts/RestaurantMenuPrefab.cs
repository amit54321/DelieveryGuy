using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RestaurantMenuPrefab : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Text levelText;
    int id;
    // Start is called before the first frame update
    public void SetData(int id,int restId,int level)
    {
        this.id = id;
        icon.sprite = Resources.Load<Sprite>("Prefabs/RestaurantImage/" + restId);
        if(level<=0)
        {
            levelText.transform.parent.gameObject.SetActive(false);
        }
        levelText.text = level.ToString();
    }

    // Update is called once per frame
   public void OnClick()
    {
        GameManager.Instance.SetCityCameraPosition(id);
    }
}
