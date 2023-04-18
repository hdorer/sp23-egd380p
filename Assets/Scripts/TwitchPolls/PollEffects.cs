using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PollEffectID {
    FIRE_RATE_DOWN,
    FIRE_RATE_UP,
    MOVE_SPEED_DOWN,
    MOVE_SPEED_UP,
    DAMAGE_TAKEN_DOWN,
    DAMAGE_TAKEN_UP,
}

public static class PollEffects {
    public static IEnumerator changeFireRate(PlayerWeapon player, float duration, float modifier) {
        player.setFireRateModifier(modifier);
        yield return new WaitForSeconds(duration);
        player.resetFireRateModifier();
    }

    public static IEnumerator changeMoveSpeed(MovementScript player, float duration, float modifier) {
        player.setMoveSpeedModifier(modifier);
        yield return new WaitForSeconds(duration);
        player.resetMoveSpeedModifier();
    }

    public static IEnumerator changeDamageTaken(MovementScript player, float duration, float modifier) {
        player.setDamageTakenModifier(modifier);
        yield return new WaitForSeconds(duration);
        player.resetDamageTakenModifier();
}

}