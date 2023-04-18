using Lexone.UnityTwitchChat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchListener : MonoBehaviour {
    [SerializeField] IRC twitchIrc;

    private string[] validMessages;
    public string[] ValidMessages { get => validMessages; set => validMessages = value; }

    public event Action<string, string> onValidMessageRecieved;
    
    private void OnEnable() {
        twitchIrc.OnChatMessage += parseChatMessage;
    }

    private void Awake() {
        if(GameManager.UseTwitch) {
            twitchIrc.channel = GameManager.TwitchChannel;
        }
    }

    private void OnDisable() {
        twitchIrc.OnChatMessage -= parseChatMessage;
    }

    private void parseChatMessage(Chatter chatter) {
        if(Array.Exists(validMessages, element => element == chatter.message)) {
            Debug.Log(chatter.message);
            onValidMessageRecieved?.Invoke(chatter.message, chatter.login);
        }
    }
}
