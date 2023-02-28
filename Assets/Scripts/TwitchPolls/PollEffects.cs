using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PollEffects {
    public static IEnumerator ChangeFireRate(PlayerWeapon player, float duration, float modifier) {
        player.setFireRateModifier(modifier);
        yield return new WaitForSeconds(duration);
        player.resetFireRateModifier();
    }
}
