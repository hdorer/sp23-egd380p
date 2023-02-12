using Lexone.UnityTwitchChat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchListener : MonoBehaviour {
    [SerializeField] IRC twitchIrc;
    
    private void OnEnable() {
        twitchIrc.OnChatMessage += showChatMessage;
    }

    private void showChatMessage(Chatter chatter) {
        Debug.Log(chatter.message);
    }
}
