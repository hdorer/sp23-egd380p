using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SniperCombat : State
{
    public State attack;
    public State pursuit;
    public float combatDeadzone = 2;
    public LineRenderer lineRend;
    public float lineUpShotTime = 0;
    float timeToLine = 1;
    public override State StateTick(Enemy enemy)
    {
        if (enemy.currentRecoveryTime <= 0 && enemy.GetAttack())
        {
            if (lineUpShotTime <= timeToLine) 
            {
                lineUpShotTime += Time.deltaTime;
                lineRend.enabled = true;
                lineRend.SetPosition(0, enemy.bulletSpawnPosition.position);
                lineRend.SetPosition(1, enemy.target.transform.position + new Vector3(0, 1f, 0));
            }
            else
            {
                lineRend.enabled = false;
                enemy.agent.enabled = false;
                lineUpShotTime = 0;
                return attack;
            }

        }

        if (enemy.currentRecoveryTime > 0 && enemy.GetAttack() && (!enemy.isActing || !enemy.isMelee))
        {
            enemy.agent.enabled = true;
            //move around the player like a gremlin 
            enemy.agent.SetDestination(enemy.target.transform.position);

            enemy.GetComponent<Animator>().SetBool("IsWalking", true);
            if (enemy.gun != null)
            {
                Vector3 shootTarget = enemy.target.transform.position;
                enemy.gun.transform.LookAt(shootTarget, enemy.transform.up);
            }
        }
        if (enemy.distanceFromTarget > combatDeadzone && !enemy.isActing)
        {
            return pursuit;
        }

        return this;
    }
}
