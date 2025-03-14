using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplay : MonoBehaviour {
    [SerializeField] Text ammoText;
    [SerializeField] Text[] weaponNameTexts;

    public void updateAmmoText(string resourceName, int resource, int maxResource) {
        ammoText.text = resourceName + ": " + resource + "/" + maxResource;
    }

    public void updateWeaponNameText(Weapon[] weapons, int currentWeapon) {
        int index = currentWeapon;
        for(int i = 0; i < weaponNameTexts.Length; i++) {
            weaponNameTexts[i].text = weapons[index].Name;
            
            index++;
            if(index >= weapons.Length) {
                index = 0;
            }
        }
    }
}
