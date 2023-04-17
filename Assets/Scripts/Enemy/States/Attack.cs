using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    public State combat;
    public override State StateTick(Enemy enemy)
    {
        if (enemy.isActing)
        {
            return combat;
        }
        if (enemy.currentAttack != null)
        {
            if (enemy.distanceFromTarget < enemy.currentAttack.maxDistance)
            {
                if (enemy.currentRecoveryTime <= 0 && !enemy.isActing)
                {
                    enemy.isActing = true;
                    enemy.currentRecoveryTime = enemy.currentAttack.recoveryTime;
                    enemy.PlayAnimation(enemy.currentAttack.attackAnimation, enemy.currentAttack.animationLayer);
                    enemy.currentAttack = null;
                    return combat;
                }
            }
            else
            {
                return combat;
            }
        }else
        {
            return combat;
        }
        return this;
    }
}
