using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    private static GameManager instance;

    private bool useTwitch = false;
    private string twitchChannel = "";

    public static bool UseTwitch { get => instance.useTwitch; set => instance.useTwitch = value; }
    public static string TwitchChannel { get => instance.twitchChannel; set => instance.twitchChannel = value; }

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
