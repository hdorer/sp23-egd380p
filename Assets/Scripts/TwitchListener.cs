using Lexone.UnityTwitchChat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchListener : MonoBehaviour {
    [SerializeField] IRC twitchIrc;

    private string[] validMessages;
    public string[] ValidMessages { get => validMessages; set => validMessages = value; }

    public event Action<string> onValidMessageRecieved;
    
    private void OnEnable() {
        twitchIrc.OnChatMessage += parseChatMessage;
    }

    private void OnDisable() {
        twitchIrc.OnChatMessage -= parseChatMessage;
    }

    private void parseChatMessage(Chatter chatter) {
        if(Array.Exists(validMessages, element => element == chatter.message)) {
            onValidMessageRecieved?.Invoke(chatter.message);
        }
    }
}
