using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyHealth enemyHealth;
    [SerializeField] private float enemyHealthCurrent;
    [SerializeField] private float enemyHealthMax = 5;
    [SerializeField] GameObject bulletSpawner;
    private Collider2D enemyCollider;

    private void Start()
    {
        enemyHealthCurrent = enemyHealthMax;
        enemyHealth.UpdateHealthBar();
        enemyCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enemyCollider != null)
        {
            BaseBullet bullet = collision.GetComponent<BaseBullet>();
            if (bullet != null)
            {
                bullet.OnHit();
                enemyHealthCurrent -= bullet.bulletDamage;
                enemyHealth.UpdateHealthBar();
                Debug.Log("Enemy Hit!");
                if (enemyHealthCurrent <= 0)
                {
                    Debug.Log("Enemy is dead!");
                    bulletSpawner.SetActive(false);
                }
            }
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
