using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyHealth;
    [SerializeField] GameObject bulletSpawner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemyHealth -= 1;
        Debug.Log("Enemy Hit!");
        if (enemyHealth <= 0)
        {
            Debug.Log("Enemy is dead!");
            bulletSpawner.SetActive(false);
        }
    }
}
