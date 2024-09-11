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
    public GameObject bullet; // ��ͨ�ӵ���Ԥ����
    public GameObject heavyBullet; // �����ӵ���Ԥ����
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
        if (canSpawn)
        {
            SpawnBullet();
        }
        
        PlayerController playerController = GetComponent<PlayerController>();
    }


    void Update()
    {
        //Debug.Log(timer);
        if (canSpawn)
        {
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
