using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] GameObject playerCharacter;
    [SerializeField] Image healthFill;
    float pHealth, pHealthMax;
    public string gameoverSceneName = "TestGameOverScreen";
    // Start is called before the first frame update
    void Start()
    {
        pHealth = playerCharacter.GetComponent<MovementScript>().Health;
        pHealth = pHealth/100;
        pHealthMax = playerCharacter.GetComponent<MovementScript>().MaxHealth;
        pHealthMax = pHealthMax/100;
    }

    // Update is called once per frame
    void Update()
    {
        pHealth = playerCharacter.GetComponent<MovementScript>().Health;
        pHealth = pHealth/100;

        healthFill.fillAmount = Mathf.Clamp(pHealth/pHealthMax, 0.0f, 1.0f);
        if(pHealth <= 0)
        {
            SceneSwitcher.GoToSceneName(gameoverSceneName);
        }
    }
}
