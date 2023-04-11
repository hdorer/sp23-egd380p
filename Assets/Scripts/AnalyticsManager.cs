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
}
