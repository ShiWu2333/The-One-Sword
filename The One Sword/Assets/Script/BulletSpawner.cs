using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spawnPoint;

    private float timer = 0;

    [SerializeField] private int bulletSpeed;
    [SerializeField] private float spawnRate = 3;

    private void Start()
    {
        SpawnBullet();
    }

    void Update()
    {
        //Debug.Log(timer);
        if (timer < spawnRate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            SpawnBullet();
            timer = 0;
        }
    }

    public void SpawnBullet()
    {
        Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    public void DestroyBullet()
    {
        DestroyImmediate(bullet, true);
    }

}
