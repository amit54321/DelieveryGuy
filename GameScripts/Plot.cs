using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Plot : MonoBehaviour, OnClick
{

    public int id;
    public Vector3 eulerAngle;

    public void OnCLickMethod()
    {

        if (RoomContoller.SocketMaster.instance.profileData.timers.Count > 0)
        {
            //show warning one timer is running
            return;
        }
        if (transform.GetComponent<Restaurants>()!=null)
        {
            if (RoomContoller.SocketMaster.instance.profileData.restaurants.Count <10)
            {
                //show warning first construct all restaurants
                return;
            }
            GameManager.Instance.clickedPlotId = id;
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.upgradePopUp);
            return;
        }
        
       
        GameManager.Instance.clickedPlotId = id;
        InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.constructionPopUp);
        UnityEngine.Debug.LogError("CLICKED  " + id);
    }

   

}
