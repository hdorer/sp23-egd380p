using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected float health = 100;
    public float Health { get => health; }
    [SerializeField] protected float maxHealth = 100;
    public float MaxHealth { get => maxHealth; }
    [SerializeField] protected float moveSpeed = 1;

    private float damageModifier = 1f;

    public virtual void takeDamage(float damage) {
        health -= damage * damageModifier;
    }
}
