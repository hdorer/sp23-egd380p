using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/AttackAction")]
public class AttackAction : ScriptableObject
{
    public string attackAnimation;
    public int animationLayer;
    public int attackScore;
    public float recoveryTime;
    public float minDistance;
    public float maxDistance;
    public float pushAmount;
}
