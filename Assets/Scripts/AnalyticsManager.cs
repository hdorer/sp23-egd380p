using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using UnityEngine;

public class AnalyticsManager : MonoBehaviour {
    private async void Start() {
        try {
            await UnityServices.InitializeAsync();
        } catch(ConsentCheckException e) {
            Debug.LogError(e.ToString());
        }
    }

    public void sendPollStartEvent(Poll poll) {
        Dictionary<string, object> parameters = new Dictionary<string, object>() {
            { "pollEffectName", poll.optionNames[0] },
            { "pollEffectName1", poll.optionNames[1] },
            { "pollEffectName2", poll.optionNames[2] },
            { "pollEffectName3", poll.optionNames[3] }
        };

        AnalyticsService.Instance.CustomData("pollStart", parameters);
        AnalyticsService.Instance.Flush();

        Debug.Log("event sent");
    }
}
