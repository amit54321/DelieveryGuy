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
    public void SetData(int id,string titleText,string descText,int costText,int levelText,int timerText )
    {
        this.id = id;
        title.text = titleText;
        desc.text = descText;
        cost.text = "Cost: "+costText.ToString();
        if(level)
        level.text = "Level: "+levelText.ToString();
        timer.text = "Build Time: "+timerText.ToString()+"s";
        cTime = timerText;
        buildButton.onClick.AddListener(Build);

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
                    LeaveRoomCallBack(
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


    private void LeaveRoomCallBack(LobbyData.RoomDataCallBack callbackdata)
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
