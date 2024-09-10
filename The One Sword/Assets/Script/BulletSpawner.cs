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
    public GameObject bullet; // ��ͨ�ӵ���Ԥ����
    public GameObject heavyBullet; // �����ӵ���Ԥ����
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

        // ȷ��ѡ����һ���ӵ�
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
