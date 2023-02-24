using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Combat : State
{
    public State attack;
    public State pursuit;
    public float combatDeadzone = 2;
    public override State StateTick(Enemy enemy)
    {
        if (enemy.currentRecoveryTime <= 0 && enemy.GetAttack())
        {
            enemy.agent.enabled = false;
            return attack;
        }
        if (enemy.currentRecoveryTime > 0 && enemy.GetAttack())
        {
            //move towards the player like it was pursuit. 
            enemy.agent.enabled = true;
            enemy.agent.SetDestination(enemy.target.transform.position);
            if(enemy.gun != null)
            {
                Vector3 shootTarget = enemy.target.transform.position;
                shootTarget.y += 0.5f;
                Vector3 shootVelocity = shootTarget - transform.position;
                Quaternion newRot = Quaternion.LookRotation(shootVelocity);
                newRot.eulerAngles = new Vector3(0, newRot.eulerAngles.y, 0);
                enemy.gun.transform.rotation = Quaternion.Slerp(enemy.gun.transform.rotation, newRot, 10 * Time.deltaTime);
            }
        }
        if (enemy.distanceFromTarget > combatDeadzone && !enemy.isActing)
        {
            return pursuit;
        }
        
        return this;
    }
}
