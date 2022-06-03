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
    public int id;
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
        if(InGame.UIManager.Instance.tutorialUI.activeSelf  && GameManager.Instance.tutorial.current!= 4 )
        {
            return;
        }
        if (InGame.UIManager.Instance.tutorialUI.activeSelf && id == 3)
        {
            GameManager.Instance.tutorial.SetTutorial();
            ButtonSOund.instance.Play();
            GameManager.Instance.SetCityCameraPosition(id);
        }
        else if (!InGame.UIManager.Instance.tutorialUI.activeSelf)
        {
            ButtonSOund.instance.Play();
            GameManager.Instance.SetCityCameraPosition(id);
        }
     
       
    }
}
