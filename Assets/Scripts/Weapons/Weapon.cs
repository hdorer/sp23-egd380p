using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : ScriptableObject {
    private PlayerWeapon player;
    
    [Header("UI Properties")]
    [SerializeField] private string gunName;
    [SerializeField] private string flavorText;

    protected PlayerWeapon Player { get => player; }
    public string Name { get => gunName; }
    public int gunIndex;

    public void setPlayer(PlayerWeapon player) {
        if(this.player == null) {
            this.player = player;
        }
    }

    public abstract void start();
    public abstract void update();
    public abstract void equip();
    public abstract void startFiring();
    public abstract void stopFiring();
    public abstract IEnumerator reload();
}
