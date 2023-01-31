using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Character
{
    public State currentState;
    public NavMeshAgent agent;
    public PlayerMovement target;
    public void Update()
    {
        currentState = currentState.StateTick(this);
    }
}
