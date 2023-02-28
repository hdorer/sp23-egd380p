using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PollEffectID {
    FIRE_RATE_DOWN,
    FIRE_RATE_UP,
    MOVE_SPEED_DOWN,
    MOVE_SPEED_UP,
}

public static class PollEffects {
    public static IEnumerator ChangeFireRate(PlayerWeapon player, float duration, float modifier) {
        player.setFireRateModifier(modifier);
        yield return new WaitForSeconds(duration);
        player.resetFireRateModifier();
    }

    public static IEnumerator ChangeMoveSpeed(MovementScript player, float duration, float modifier) {
        player.setMoveSpeedModifier(modifier);
        yield return new WaitForSeconds(duration);
        player.resetMoveSpeedModifier();
    }
}
