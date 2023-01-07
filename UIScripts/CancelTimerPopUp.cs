using System.Collections;
using System.Collections.Generic;
using RoomContoller;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CancelTimerPopUp : BasePOpUp
{
    int cost;
    public Text costText;
    public Button yesButton;
    Restaurants r;

    public void OnEnable()
    {
         r = GameManager.Instance.FindRestaurantById(GameManager.Instance.clickedPlotId);
        if (r != null)
        {         //  Debug.Log("CURRENT VALUE " + GameManager.Instance.timer.timerValue);
            cost = r.currentTimerValue;
            costText.text = r.currentTimerValue.ToString();
            if (SocketMaster.instance.profileData.coins < cost)
            {
                yesButton.gameObject.SetActive(false);
            }
        }
    }
    private void Update()
    {
        if (r != null)
        {
            cost = r.currentTimerValue;
            costText.text = r.currentTimerValue.ToString();
            if (SocketMaster.instance.profileData.coins < cost)
            {
                yesButton.gameObject.SetActive(false);
            }
        }
    }
    public void Yes()
    {
        SocketMaster.instance.CancelTimerPack(cost);
        InGame.UIManager.Instance.DisablePopUp();

    }

    // Update is called once per frame
    public void No()
    {
        InGame.UIManager.Instance.DisablePopUp();
    }
}
