using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PollResultManager : MonoBehaviour {
    [SerializeField] private float effectDuration = 10.0f;

    [SerializeField] private MovementScript pMovement;
    [SerializeField] private PlayerWeapon pWeapons;

    private IEnumerator[] pollEffects;

    private void Awake() {
        pollEffects = new IEnumerator[] {
            PollEffects.changeFireRate(pWeapons, effectDuration, 0.25f),
            PollEffects.changeFireRate(pWeapons, effectDuration, 2f),
            PollEffects.changeMoveSpeed(pMovement, effectDuration, 0.25f),
            PollEffects.changeMoveSpeed(pMovement, effectDuration, 2f),
            PollEffects.changeDamageTaken(pMovement, effectDuration, 0.75f),
            PollEffects.changeDamageTaken(pMovement, effectDuration, 1.25f),
        };
    }

    public void applyPollEffect(Poll poll) {
        int maxValue = poll.votes.Max();
        int maxIndex = Array.IndexOf(poll.votes, maxValue);
        PollEffectID effect = poll.effects[maxIndex];

        StartCoroutine(pollEffects[(int)effect]);
    }
}
