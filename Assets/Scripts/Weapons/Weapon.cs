using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject {
    [Header("UI Properties")]
    [SerializeField] string gunName;
    [SerializeField] string flavorText;

    public string Name { get => gunName; }

    public abstract void fire(Transform bulletSpawnPoint);
}
