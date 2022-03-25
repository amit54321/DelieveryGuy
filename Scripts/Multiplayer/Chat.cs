using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [SerializeField] Text chatText;
    [SerializeField] private Button button;
    [SerializeField] private LobbyData.ChatData chatData;
    public Action<LobbyData.ChatData> chatClicked;

    public void SetChat(LobbyData.ChatData chatData)
    {
        this.chatData = chatData;
        chatText.text = chatData.chatText;
        button.onClick.AddListener(ChatClicked);
    }

    // Update is called once per frame
    void ChatClicked()
    {
        chatClicked.Invoke(chatData);
    }
}