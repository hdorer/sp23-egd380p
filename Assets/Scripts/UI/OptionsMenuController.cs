using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour {
    [SerializeField] private Toggle twitchToggle;
    [SerializeField] private TMP_InputField usernameInput;

	private void Start() {
        twitchToggle.isOn = GameManager.UseTwitch;
        usernameInput.text = GameManager.TwitchChannel;
	}

	public void twitchToggleChanged(bool value) {
        GameManager.UseTwitch = value;
        usernameInput.interactable = value;
    }

    public void usernameInputChanged(string value) {
        GameManager.TwitchChannel = value;
    }
}
