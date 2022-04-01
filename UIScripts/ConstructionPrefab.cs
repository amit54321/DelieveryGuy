using LitJson;
using RoomContoller;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ConstructRestaurant
{
    public string id;
    public int plot_id;
    public int restaurant_id;

}

public class UpgradeRestaurant
{
    public string id;
    public int level;
    public int restaurant_id;

}
public class ConstructionPrefab : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Text title, desc, cost, level, timer;
    [SerializeField]
    Button buildButton;

    int id;
    int cTime;
    int currentLevel;
    public void SetData(int id,string titleText,string descText,int costText,int levelText,int timerText )
    {
        this.id = id;
        title.text = titleText;
        desc.text = descText;
        cost.text = "Cost: "+costText.ToString();
        if (level)
        {
            currentLevel = levelText;
            buildButton.onClick.AddListener(Upgrade);
            level.text = "Level: " + levelText.ToString();
        }
        else
        {
            buildButton.onClick.AddListener(Build);
        }
        timer.text = "Build Time: "+timerText.ToString()+"s";
        cTime = timerText;
       

    }

    private void Upgrade()
    {
        GameManager.Instance.UpgradeBuilding( id, cTime);
        InGame.UIManager.Instance.DisablePopUp();
        return;

        UpgradeRestaurant constructRestaurant;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.LEAVEROOM,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");

                    UIManager.instance.ToggleLoader(false);
                    UpgradeCallBack(
                        JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            constructRestaurant = new UpgradeRestaurant()
            {
                id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                level= this.currentLevel,
                restaurant_id = GameManager.Instance.clickedPlotId
            });
    }

    private void UpgradeCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            UIManager.instance.EnablePanel(UIManager.instance.createJoinScreen);
        }
        else
        {
            UIManager.instance.ShowError(callbackdata.message);
        }
    }


    private void Build()
    {
        GameManager.Instance.ConstructBuilding(GameManager.Instance.clickedPlotId, id,cTime);
        InGame.UIManager.Instance.DisablePopUp();
        return;

        ConstructRestaurant constructRestaurant;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.LEAVEROOM,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  ");

                    UIManager.instance.ToggleLoader(false);
                    BuildCallBack(
                        JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            constructRestaurant = new ConstructRestaurant()
            {
                id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                plot_id = id,
                restaurant_id= GameManager.Instance.clickedPlotId
            });
    }


    private void BuildCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            UIManager.instance.EnablePanel(UIManager.instance.createJoinScreen);
        }
        else
        {
            UIManager.instance.ShowError(callbackdata.message);
        }
    }

}
