using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Testing
{
    public string name;
}

[System.Serializable]
public class TestingMessage
{
    public string message;
}
public class SocketTesting : MonoBehaviour
{
    public void SetName()
    {
        Debug.LogError("creating ");


        Testing roomData;
        RoomContoller.SocketMaster.instance.socketMaster.Socket.Emit(
          "setName",
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.LogError("createroom" + JsonMapper.ToJson(args[0]));
                    CreateRoomCallBack(
                        JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            roomData = new Testing()
            {
                name = "amit"
               
            });


    }

    private void CreateRoomCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            PlayerPrefs.SetString("RoomId", callbackdata.room._id);
          
        }
        else
        {
          
        }
    }

    public void SetMessage()
    {


        TestingMessage roomData;
        RoomContoller.SocketMaster.instance.socketMaster.Socket.Emit(
          "sendPublic",
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    Debug.LogError("createroom" + JsonMapper.ToJson(args[0]));
                   MessageCallBack(
                        JsonUtility.FromJson<LobbyData.RoomDataCallBack>(JsonMapper.ToJson(args[0])));
                }
            },
            roomData = new TestingMessage()
            {
                message = "hello"

            });


    }

    private void MessageCallBack(LobbyData.RoomDataCallBack callbackdata)
    {
        if (callbackdata.status == 200)
        {
            PlayerPrefs.SetString("RoomId", callbackdata.room._id);

        }
        else
        {

        }
    }
}
