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
    public int plot_id;

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
    int cTime,quantity, cookTime;
    int currentLevel;
    public void SetData(int id,string titleText,string descText,int costText,int levelText,int timerText ,int quantity,int cookTime)
    {
        this.id = id;
        title.text = titleText;
        desc.text = descText;
        this.quantity = quantity;
        this.cookTime = cookTime;
        cost.text = "Cost: "+costText.ToString();
        currentLevel = levelText;
        Debug.LogError("UPGRADE START  "+level);
        if (level)
        {
            Debug.LogError("UPGRADE START");
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
        // GameManager.Instance.UpgradeBuilding(GameManager.Instance.clickedPlotId, id, cTime, quantity, cookTime, currentLevel) ;

        //    return;
        Debug.LogError("UPGRADE CLICKED");
        InGame.UIManager.Instance.DisablePopUp();
        UpgradeRestaurant constructRestaurant;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.UPGRADE,
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
                plot_id = GameManager.Instance.clickedPlotId,
                restaurant_id = id
            });
    }

    private void UpgradeCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            GameManager.Instance.UpgradeBuilding(GameManager.Instance.clickedPlotId, id, cTime, quantity, cookTime, currentLevel);

        }
        else
        {
            UIManager.instance.ShowError(callbackdata.message);
        }
    }


    private void Build()
    {
        // GameManager.Instance.ConstructBuilding(GameManager.Instance.clickedPlotId, id,cTime,quantity,cookTime,currentLevel);
        //
        //  return;
        InGame.UIManager.Instance.DisablePopUp();
        ConstructRestaurant constructRestaurant;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.CONSTRUCT,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.Log(JsonMapper.ToJson(args[0]) + "  DATA  CONSTRUCTION STARTED ");

                    UIManager.instance.ToggleLoader(false);
                    BuildCallBack(
                        JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            constructRestaurant = new ConstructRestaurant()
            {
                id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                plot_id = GameManager.Instance.clickedPlotId,
                restaurant_id = id
            });
    }


    private void BuildCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            GameManager.Instance.ConstructBuilding(GameManager.Instance.clickedPlotId, id, cTime, quantity, cookTime, currentLevel);

        }
        else
        {
            UIManager.instance.ShowError(callbackdata.message);
        }
    }

}
