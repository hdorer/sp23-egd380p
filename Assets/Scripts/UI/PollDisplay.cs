using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PollDisplay : MonoBehaviour {
    [SerializeField] private Text timerText;
    [SerializeField] private Slider[] voteSliders;

    public void updateUi(Poll poll, float pollTimer) {
        timerText.text = "Time Left: " + Mathf.Ceil(pollTimer);

        float highestVote = poll.votes.Max();
        for(int i = 0; i < voteSliders.Length; i++) {
            voteSliders[i].maxValue = highestVote > 0 ? highestVote : 1;
            
            try {
                voteSliders[i].value = poll.votes[i];
            } catch(Exception e) {
                voteSliders[i].value = 0;
            }
        }
    }
}
