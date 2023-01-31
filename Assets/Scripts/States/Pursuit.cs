using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursuit : State
{
    public override State StateTick(Enemy enemy)
    {
        enemy.agent.SetDestination(enemy.target.transform.position);
        return this;
    }
}
