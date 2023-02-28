using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int health = 100;
    public int Health { get => health; }
    [SerializeField] protected int maxHealth = 100;
    public int MaxHealth { get => maxHealth; }
    [SerializeField] protected float moveSpeed = 1;
}
