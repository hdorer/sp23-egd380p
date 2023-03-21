using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour {
    [SerializeField] Text ammoText;
    [SerializeField] Text[] weaponNameTexts;

    public void updateAmmoText(HeldWeapon weapon) {
        ammoText.text = "Ammo: " + weapon.bulletsInClip + "/" + weapon.weapon.ClipSize;
    }

    public void updateWeaponNameText(HeldWeapon[] weapons, int currentWeapon) {
        int index = currentWeapon;
        for(int i = 0; i < weaponNameTexts.Length; i++) {
            weaponNameTexts[i].text = weapons[index].weapon.WeaponName;
            
            index++;
            if(index >= weapons.Length) {
                index = 0;
            }
        }
    }
}
