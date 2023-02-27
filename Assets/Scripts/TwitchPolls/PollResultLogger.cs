using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PollResultLogger : MonoBehaviour {
    public void logPollResult(Poll poll) {
        int maxValue = poll.votes.Max();
        int maxIndex = Array.IndexOf(poll.votes, maxValue);

        Debug.Log("The winning result is " + poll.optionNames[maxIndex] + " with " + maxValue + " votes");
    }
}
