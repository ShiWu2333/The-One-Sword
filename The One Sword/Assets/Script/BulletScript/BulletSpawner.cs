using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{

    //子弹发射的节奏列表
    private float[] fireIntervals = { 1f, 1f, 0.5f, 0.5f, 0.5f };
    private BulletType[] bulletSequence = { BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Normal };
    private bool isFiring = false; // 添加一个标志位，防止重复启动协程

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
    [SerializeField] private AnimationCurve traAniCurve;
    [SerializeField] private AnimationCurve axisCorrectionAniCurve;
    [SerializeField] private AnimationCurve speedAniCurve;
    

    [SerializeField] private bool canSpawn;
    [SerializeField] private GameObject player;

    private void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        StartCoroutine(FireBulletGroup());
    }


    void Update()
    {
        //Debug.Log(timer);
        if (canSpawn && !isFiring)
        {
            if (timer < spawnRate)
            {
                timer = timer + Time.deltaTime;
            }
            else
            {
                StartCoroutine(FireBulletGroup());
                timer = 0;
            }
        }
        
    }

    IEnumerator FireBulletGroup()
    {
        isFiring = true;
        for (int i = 0; i < fireIntervals.Length; i++)
        {
            BulletType currentBulletType = bulletSequence[i];
            SpawnBullet(currentBulletType);
            yield return new WaitForSeconds(fireIntervals[i]); // 等待设定的间隔时间
        }
        isFiring = false;
    }

    public void SpawnBullet(BulletType bulletType)
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
            bullet.InitializeAniCurve(traAniCurve, axisCorrectionAniCurve, speedAniCurve);
        }
        
    }

    public void DestroyBullet()
    {
        DestroyImmediate(bullet, true);
    }

}
