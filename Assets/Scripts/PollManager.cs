using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct Poll {
    public string[] optionNames;
    public string[] voteStrings;
    public int[] votes;

    public Poll(string[] optionNames) {
        this.optionNames = optionNames;
        voteStrings = new string[optionNames.Length];
        votes = new int[optionNames.Length];

        for(int i = 0; i < optionNames.Length; i++) {
            voteStrings[i] = (i + 1).ToString();
        }
    }
}

public class PollManager : MonoBehaviour {
    [SerializeField] private float pollTime = 20f;
    private float pollTimer;
    [SerializeField] private float pollDowntime = 60f;
    private float pollDownTimer;
    private float nextUiUpdate;
    [SerializeField] private bool startActive = false;
    private bool pollActive;

    [SerializeField] private string[] optionLabels;
    
    private Poll activePoll;

    [SerializeField] private TwitchListener listener;

    [System.Serializable] public class UiUpdateEvent : UnityEvent<Poll, float> { }
    public UiUpdateEvent onUiUpdate;

    [System.Serializable] public class PollStartEvent : UnityEvent<Poll> { }
    public PollStartEvent onPollStart;

    [System.Serializable] public class PollEndEvent : UnityEvent<Poll> { }
    public PollEndEvent onPollEnd;

    private void OnEnable() {
        listener.onValidMessageRecieved += parseMessage;
    }

    private void Start() {
        pollTimer = pollTime;
        nextUiUpdate = pollTimer - 1f;

        pollDownTimer = pollDowntime;

        listener.gameObject.SetActive(false);

        if(startActive) {
            startPoll();
        }
    }

    private void Update() {
        if(pollActive) {
            pollTimer -= Time.deltaTime;

            if(pollTimer <= nextUiUpdate) {
                onUiUpdate?.Invoke(activePoll, pollTimer);
                nextUiUpdate -= 1f;
            }

            if(pollTimer <= 0f) {
                endPoll();
            }
        } else {
            pollDownTimer -= Time.deltaTime;

            if(pollDownTimer <= 0f) {
                startPoll();
            }
        }
    }

    private void OnDisable() {
        listener.onValidMessageRecieved -= parseMessage;
    }

    private void startPoll() {
        pollActive = true;
        pollTimer = pollTime;

        activePoll = new Poll(optionLabels);
        
        listener.gameObject.SetActive(true);
        listener.ValidMessages = activePoll.voteStrings;

        onPollStart?.Invoke(activePoll);
        onUiUpdate?.Invoke(activePoll, pollTimer);
    }

    private void endPoll() {
        pollActive = false;
        pollDownTimer = pollDowntime;

        listener.gameObject.SetActive(false);

        onPollEnd?.Invoke(activePoll);
    }

    private void parseMessage(string message) { 
        for(int i = 0; i < activePoll.voteStrings.Length; i++) {
            if(message == activePoll.voteStrings[i]) {
                activePoll.votes[i]++;
            }
        }
    }

    private void logPollResults() {
        string output = "";
        for(int i = 0; i < activePoll.voteStrings.Length; i++) {
            output += activePoll.voteStrings[i] + ": " + activePoll.votes[i] + ", ";
        }

        Debug.Log(output);
    }
}
