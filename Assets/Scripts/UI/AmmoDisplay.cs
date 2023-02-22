using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour {
    [SerializeField] Text text;

    public void updateUiText(int ammo, int maxAmmo) {
        text.text = "Ammo: " + ammo + "/" + maxAmmo;
    }
}
