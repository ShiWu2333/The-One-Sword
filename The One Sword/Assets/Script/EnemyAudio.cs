using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] AudioSource audioSource;

    void Start()
    {
        enemy.OnEnemyDie += Enemy_OnEnemyDie;
    }

    private void Enemy_OnEnemyDie(object sender, System.EventArgs e)
    {
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
