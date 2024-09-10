using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [SerializeField] private float maxBulletSpeed;
    [SerializeField] private float traMaxHeight;
    [SerializeField] private float spawnRate = 3;
    [SerializeField] private Transform target;
    [SerializeField] private AnimationCurve yTraAniCurve;
    [SerializeField] private AnimationCurve xTraAniCurve;
    [SerializeField] private AnimationCurve axisCorrectionAniCurve;
    [SerializeField] private AnimationCurve speedAniCurve;

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
            BaseBullet bullet = Instantiate(bulletToSpawn, spawnPoint.transform.position, spawnPoint.transform.rotation).GetComponent<BaseBullet>();
            bullet.InitializeBullet(target, maxBulletSpeed, traMaxHeight);
            bullet.InitializeAniCurve(yTraAniCurve, xTraAniCurve, axisCorrectionAniCurve, speedAniCurve);
        }
        
    }

    public void DestroyBullet()
    {
        DestroyImmediate(bullet, true);
    }

}
