using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public enum BulletType
    {
        Normal,
        Heavy,
    }

    public BulletType bulletType;
    public GameObject bullet; // 普通子弹的预制体
    public GameObject heavyBullet; // 重型子弹的预制体
    public GameObject spawnPoint;

    private float timer = 0;
    private GameObject bulletToSpawn;

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
        GameObject bulletToSpawn = null;
        switch (bulletType)
        {
            case BulletType.Normal:
                bulletToSpawn = bullet;
                break;
            case BulletType.Heavy:
                bulletToSpawn = heavyBullet;
                break;
        }

        // 确保选择了一个子弹
        if (bulletToSpawn != null)
        {
            Instantiate(bulletToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        
    }

    public void DestroyBullet()
    {
        DestroyImmediate(bullet, true);
    }

}
