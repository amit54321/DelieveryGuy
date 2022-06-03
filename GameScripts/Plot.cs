using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Plot : MonoBehaviour, OnClick
{

    public int id;
    public Vector3 eulerAngle;
    public bool forward;
    public void OnCLickMethod()
    {
       
        if (RoomContoller.SocketMaster.instance.profileData.timers.Count > 0)
        {
            //show warning one timer is running
            return;
        }
        if (transform.GetComponent<Restaurants>()!=null)
        {
         //   Debug.LogError("CLICKED " + transform.name+"     "+ SocketMaster.instance.profileData.restaurants.Count);
            if (RoomContoller.SocketMaster.instance.profileData.restaurants.Count <10)
            {
                InGame.UIManager.Instance.ShowError("Construct all 10 restaurants.");
                //show warning first construct all restaurants
                return;
            }
         
            if (GameManager.Instance.swapFirst >= 0)
            {
               // Restaurants r = GameManager.Instance.FindRestaurantById(id);
                GameManager.Instance.swapSecond = id;
             
                Swap();
                return;

            }
            if (transform.GetComponent<Restaurants>().restaurantData.level>=10)
            {
                InGame.UIManager.Instance.ShowError("Restaurant is already on top level.");
                //show warning first construct all restaurants
                return;
            }

            GameManager.Instance.clickedPlotId = id;
           
            InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.upgradePopUp);
            return;
        }
        UnityEngine.Debug.LogError("CLICKED STEPS " + id);
        if (InGame.UIManager.Instance.tutorialUI.activeSelf)
        {
            GameManager.Instance.tutorial.SetTutorial();
        }

        GameManager.Instance.clickedPlotId = id;
        InGame.UIManager.Instance.EnablePopUp(InGame.UIManager.Instance.constructionPopUp);
        
    }

    private void Swap()
    {
        // GameManager.Instance.UpgradeBuilding(GameManager.Instance.clickedPlotId, id, cTime, quantity, cookTime, currentLevel) ;

        //    return;
        Debug.LogError("SWAP CLICKED"+ GameManager.Instance.swapFirst +"   "+ GameManager.Instance.swapSecond);
      //  InGame.UIManager.Instance.DisablePopUp();
        SwapRestaurant constructRestaurant;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.SWAP,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");

                    UIManager.instance.ToggleLoader(false);
                 ////   UpgradeCallBack(
                   //     JsonUtility.FromJson<ConstructRestaurantCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            constructRestaurant = new SwapRestaurant()
            {
                id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
               
                plot_id1 = GameManager.Instance.swapFirst,
                 plot_id2 = GameManager.Instance.swapSecond
              
            });
    }


}
