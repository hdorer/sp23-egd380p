using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct Poll {
    public PollEffectID[] effects;
    public string[] optionNames;
    public string[] voteStrings;
    public int[] votes;

    public Poll(string[] optionNames) {
        effects = new PollEffectID[] {
            PollEffectID.FIRE_RATE_DOWN,
            PollEffectID.FIRE_RATE_UP,
            PollEffectID.MOVE_SPEED_DOWN,
            PollEffectID.MOVE_SPEED_UP
        }; // temp
        this.optionNames = optionNames;
        voteStrings = new string[optionNames.Length];
        votes = new int[optionNames.Length];

        for(int i = 0; i < optionNames.Length; i++) {
            voteStrings[i] = (i + 1).ToString();
        }
    }
}

public class PollManager : MonoBehaviour {
    [Header("General")]
    [SerializeField] private bool startActive = false;
    private bool pollActive;

    [SerializeField] bool oneVotePerChatter;
    private List<string> voterUsernames;

    [SerializeField] private string[] optionLabels;

    [Header("Timers")]
    [SerializeField] private float pollTime = 20f;
    private float pollTimer;
    
    [SerializeField] private float pollDowntime = 60f;
    private float pollDownTimer;
    private float nextUiUpdate;
    
    private Poll activePoll;

    [SerializeField] private TwitchListener listener;

    [System.Serializable] public class UiUpdateEvent : UnityEvent<Poll, float> { }
    public UiUpdateEvent onUiUpdate;

    [System.Serializable] public class PollStartEvent : UnityEvent<Poll> { }
    public PollStartEvent onPollStart;

    [System.Serializable] public class PollEndEvent : UnityEvent<Poll> { }
    public PollEndEvent onPollEnd;

    [System.Serializable] public class SetActiveEvent : UnityEvent<bool> { }
    public SetActiveEvent onSetActive;

    private void OnEnable() {
        listener.onValidMessageRecieved += parseMessage;
    }

    private void Start() {
        if(GameManager.UseTwitch) {
            voterUsernames = new List<string>();

            pollTimer = pollTime;
            pollDownTimer = pollDowntime;

            if(startActive) {
                startPoll();
            } else {
                listener.gameObject.SetActive(false);
            }
        }

        onSetActive?.Invoke(GameManager.UseTwitch);
        gameObject.SetActive(GameManager.UseTwitch);
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

        nextUiUpdate = pollTimer - 1;

        onPollStart?.Invoke(activePoll);
        onUiUpdate?.Invoke(activePoll, pollTimer);
    }

    private void endPoll() {
        pollActive = false;
        pollDownTimer = pollDowntime;

        voterUsernames.Clear();

        listener.gameObject.SetActive(false);

        onPollEnd?.Invoke(activePoll);
    }

    private void parseMessage(string message, string username) {
        if(oneVotePerChatter && voterUsernames.Contains(username)) {
            return;
        }
        
        for(int i = 0; i < activePoll.voteStrings.Length; i++) {
            if(message == activePoll.voteStrings[i]) {
                activePoll.votes[i]++;
            }
        }

        voterUsernames.Add(username);
    }

    private void logPollResults() {
        string output = "";
        for(int i = 0; i < activePoll.voteStrings.Length; i++) {
            output += activePoll.voteStrings[i] + ": " + activePoll.votes[i] + ", ";
        }

        Debug.Log(output);
    }

    // TODO: after more effects are added, create a function to generate a poll populated with randomly selected effects/effect texts
}
