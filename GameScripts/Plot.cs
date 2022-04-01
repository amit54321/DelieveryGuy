using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Plot : MonoBehaviour, OnClick
{

    public int id;

    public void OnCLickMethod()
    {
        if(transform.GetComponent<Restaurants>()!=null)
        {
            GameManager.Instance.clickedPlotId = id;
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.upgradePopUp);
            return;
        }
        
        GameManager.Instance.clickedPlotId = id;
        InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.constructionPopUp);
        UnityEngine.Debug.LogError("CLICKED  " + id);
    }

   

}
