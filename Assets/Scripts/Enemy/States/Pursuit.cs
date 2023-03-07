using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : State
{
    public State combat;
    public override State StateTick(Enemy enemy)
    {
        if (enemy.GetAttack())
        {
            enemy.agent.enabled = false;
            enemy.GetComponent<Animator>().SetBool("IsWalking", false);
            return combat;
        }
        else 
        {
            enemy.agent.enabled = true;
            enemy.GetComponent<Animator>().SetBool("IsWalking", true);
            enemy.agent.SetDestination(enemy.target.transform.position);
        }
        if (enemy.gun != null)
        {
            Vector3 shootTarget = enemy.target.transform.position;
            enemy.gun.transform.LookAt(shootTarget, enemy.transform.up);
        }
        return this;
    }
}
