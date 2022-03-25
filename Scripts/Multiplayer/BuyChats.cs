using System.Collections;
using System.Collections.Generic;
using Authentication;
using LitJson;
using RoomContoller;
using UnityEngine;

public class BuyChats : WebRequest
{
    [SerializeField] Chat chatPrefab;
    [SerializeField] private Transform parent;

    void OnEnable()
    {
        SetCurrentScreen();
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        GetChats();
    }

    public void GetChats()
    {
        LobbyData.DiceRolled data;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.GETALLCHATS,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    GetAllChats(
                        JsonUtility.FromJson<LobbyData.GetAllChats>(JsonMapper.ToJson(args[0])));
                }
            },
            data = new LobbyData.DiceRolled()
            {
                _id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
            });
    }

    void GetAllChats(LobbyData.GetAllChats chats)
    {
        foreach (Transform t in parent)
        {
            Destroy(t.gameObject);
        }

        List<LobbyData.ChatData> chatsUserHas = chats.chatPacks;
        foreach (LobbyData.ChatData chat in Authentication.Authentication.allChats)
        {
            bool found = false;
            foreach (LobbyData.ChatData chatP in chatsUserHas)
            {
                if (chat.chatId == chatP.chatId)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                InstantiateChat(chat);
            }
        }
    }

    void InstantiateChat(LobbyData.ChatData chatData)
    {
        Chat chat = Instantiate(chatPrefab, parent);
        chat.SetChat(chatData);
        chat.chatClicked = ChatClicked;
    }

    void ChatClicked(LobbyData.ChatData chat)
    {
        //    GameLogic.instance.analytics.SetEvents("chat", chat.chatText);
        //    GameLogic.instance.analytics.SendAnalytics();

        BuyChat(chat);
    }

    void BuyChat(LobbyData.ChatData chats)
    {
        Debug.LogError("CHAT dsfgfh  " + chats.chatText);
        LobbyData.AddChats data;
        SocketMaster.instance.socketMaster.Socket.Emit(
            LobbyConstants.ADDCHATPACK,
            (socket, packet, args) =>
            {
                if (args != null && args.Length > 0)
                {
                    GetAllChats(
                        JsonUtility.FromJson<LobbyData.GetAllChats>(JsonMapper.ToJson(args[0])));
                }
            },
            data = new LobbyData.AddChats()
            {
                id = PlayerPrefs.GetString(Authentication.PlayerPrefsData.ID),
                chats = chats
            });
    }
}