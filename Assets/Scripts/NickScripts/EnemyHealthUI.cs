using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    public Enemy parentEnemy;
    public Image curHP;
    private float floatMax, floatCur;
    // Start is called before the first frame update
    void Start()
    {
        floatMax = (float)parentEnemy.MaxHealth/100;
        curHP.fillAmount = floatMax;
    }
    // Update is called once per frame
    void Update()
    {
        floatCur = (float)parentEnemy.Health/100;
        curHP.fillAmount = floatCur;
    }
}
