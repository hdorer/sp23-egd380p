using Lexone.UnityTwitchChat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwitchListener : MonoBehaviour {
    [SerializeField] IRC twitchIrc;

    private int[] votes = new int[4];
    private string[] voteStrings = new string[4] { "1", "2", "3", "4" };
    
    private void OnEnable() {
        twitchIrc.OnChatMessage += parseChatMessage;
    }

    private void OnDisable() {
        twitchIrc.OnChatMessage -= parseChatMessage;
    }

    private void parseChatMessage(Chatter chatter) {
        for(int i = 0; i < voteStrings.Length; i++) {
            if(chatter.message == voteStrings[i]) {
                votes[i]++;
            }
        }

        string logOutput = "";
        for(int i = 0; i < voteStrings.Length; i++) {
            logOutput += voteStrings[i] + ": " + votes[i] + ", ";
        }

        Debug.Log(logOutput);
    }
}
