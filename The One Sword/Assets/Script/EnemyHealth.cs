using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] private Image healthBarFill;

    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateHealthBar()
    {
        float enemyHealthCurrent = enemy.GetEnemyHealthCurrent();
        float enemyHealthMax = enemy.GetEnemyHealthMax(); 
        healthBarFill.fillAmount = enemyHealthCurrent / enemyHealthMax;
    }
}
