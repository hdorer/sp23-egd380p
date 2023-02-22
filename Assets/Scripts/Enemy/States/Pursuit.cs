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
            return combat;
        }
        else 
        {
            enemy.agent.enabled = true;
            enemy.agent.SetDestination(enemy.target.transform.position);
        }
        return this;
    }
}
