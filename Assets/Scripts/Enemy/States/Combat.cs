using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Combat : State
{
    public State attack;
    public State pursuit;
    public float combatDeadzone = 2;
    float angle = 0;
    public override State StateTick(Enemy enemy)
    {
        if (enemy.currentRecoveryTime <= 0 && enemy.GetAttack())
        {
            enemy.agent.enabled = false;
            return attack;
        }
        if (enemy.currentRecoveryTime > 0 && enemy.GetAttack() && !enemy.isActing)
        {
            //move around the player like a gremlin 
            enemy.agent.enabled = true;
            if (!enemy.isMelee)
            {
                Vector3 offset = new Vector3(Mathf.Cos(angle) * enemy.distanceFromTarget, enemy.transform.position.y, Mathf.Sin(angle) * enemy.distanceFromTarget);
                enemy.agent.SetDestination(enemy.transform.position + offset);
                angle += Time.deltaTime;
            }
            else
            {
                enemy.agent.SetDestination(enemy.target.transform.position);              
            }
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
