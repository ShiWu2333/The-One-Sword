using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float enemyHealthCurrent;
    [SerializeField] private float enemyHealthMax = 5;
    [SerializeField] GameObject bulletSpawner;

    private void Start()
    {
        enemyHealthCurrent = enemyHealthMax;
        enemyHealth.UpdateHealthBar();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyHealthCurrent -= 1;
        enemyHealth.UpdateHealthBar();
        Debug.Log("Enemy Hit!");
        if (enemyHealthCurrent <= 0)
        {
            Debug.Log("Enemy is dead!");
            bulletSpawner.SetActive(false);
        }

    }

    public float GetEnemyHealthCurrent()
    {
        return enemyHealthCurrent;
    }

    public float GetEnemyHealthMax()
    { 
        return enemyHealthMax;
    }
}
