using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private float[][] fireIntervals =
    {
        new float[] { 1, 1, 1, 1 },// Level 1 的数据
        new float[]
        {
             4.718311f, 4.718311f, 7.080926f, 8.159974f, 8.579452f, 9.878297f, 10.501001f, 11.998129f,
            13.300236f, 13.938791f, 15.439067f, 17.358687f, 17.358687f, 19.720610f, 20.160456f, 20.801320f,
            21.440952f, 22.299606f, 24.661146f, 25.718342f, 26.581840f, 26.801836f, 28.301326f, 29.160495f,
            30.001938f, 31.941241f, 32.578182f, 38.160761f, 39.019601f, 40.740362f, 42.441899f, 44.159806f,
            45.878564f, 49.300944f, 51.019681f, 51.879254f, 52.738950f, 57.880576f, 58.720795f, 59.379721f,
            59.578935f, 59.798711f, 60.019028f, 60.241643f, 61.298335f, 63.020977f, 63.438301f, 64.300395f,
            64.741865f, 66.880596f, 68.158044f, 69.880016f, 71.601919f, 73.299053f, 75.020519f, 76.741601f,
            77.160859f, 78.018607f, 78.440114f, 80.160446f, 80.160446f, 81.879148f, 83.598637f, 84.879698f,
            85.301735f, 86.159302f, 86.801040f, 87.019378f, 87.438598f, 88.740945f, 89.161227f, 89.581306f,
            90.439329f, 91.301395f, 92.161714f, 92.578124f, 93.019452f, 93.858048f, 95.579363f, 96.860749f,
            97.300460f, 99.021608f, 99.880971f, 103.301571f, 104.160885f, 104.580608f, 105.018151f, 105.878671f,
            106.300535f, 106.740939f, 107.581342f, 108.440604f, 108.440604f, 109.299074f, 109.299074f, 110.579226f,
            111.021043f, 111.440027f, 112.719661f, 113.579138f, 114.439979f, 115.301512f, 116.158026f, 117.661337f,
            117.880107f, 119.580367f, 119.798579f, 121.079530f, 121.299743f, 121.500977f, 123.018522f, 123.218211f,
            124.081571f, 124.499086f, 124.738400f, 125.138593f, 125.580357f, 126.439071f, 126.859013f, 127.301346f,
            127.301346f, 128.161793f, 128.161793f, 129.881171f, 129.881171f, 131.380982f, 131.380982f, 131.380982f,
            131.380982f, 133.301388f, 134.799052f, 135.021638f, 136.738173f, 138.218926f, 138.460518f, 139.301726f,
            139.301726f, 140.579408f, 141.018723f, 141.659870f, 141.880901f, 147.021790f, 147.880077f, 148.738884f,
            152.158663f, 153.881676f, 159.019726f, 161.600904f, 162.441984f, 164.161742f, 165.881288f, 167.581380f,
            168.438381f, 169.300609f, 169.521602f, 172.718554f, 172.718554f, 176.161332f, 176.161332f, 179.581453f,
            179.581453f, 183.020978f, 183.020978f, 184.741158f, 184.741158f, 186.439885f, 186.439885f, 188.160478f,
            188.160478f, 189.861325f, 189.861325f, 191.580979f, 191.580979f, 193.298194f, 193.298194f, 195.019672f,
            195.019672f, 196.521305f, 196.741926f, 197.160522f, 197.380054f, 199.300614f, 200.579964f, 200.579964f
        },
        new float[] { 0.6f, 0.9f, 1.2f, 0.8f, 1.5f, 0.3f }, // Level 3 的数据
        // 可以继续添加更多关卡的发射间隔
    };

    // 定义每个关卡的子弹类型序列
    public BulletType[][] bulletSequences =
    {
        new BulletType[] { BulletType.Normal, BulletType.Heavy, BulletType.Normal },
        new BulletType[] 
        {
            BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal,
            BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal,
            BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Normal,
            BulletType.Normal, BulletType.Normal, BulletType.Heavy, BulletType.Heavy, BulletType.Normal,
            BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal,
            BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Normal,
            BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Normal,
            BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Normal,
            BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Normal,
            BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Heavy, BulletType.Normal,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Normal,
            BulletType.Normal, BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Heavy,
            BulletType.Normal, BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy,
            BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Heavy, BulletType.Normal,
            BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Heavy,
            BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Normal, BulletType.Heavy,
            BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Heavy,
            BulletType.Heavy, BulletType.Heavy, BulletType.Heavy, BulletType.Normal, BulletType.Heavy,
            BulletType.Normal, BulletType.Normal, BulletType.Heavy, BulletType.Heavy, BulletType.Normal,
            BulletType.Heavy, BulletType.Normal, BulletType.Heavy, BulletType.Heavy, BulletType.Normal,
            BulletType.Normal, BulletType.Normal, BulletType.Heavy, BulletType.Normal, BulletType.Heavy,
            BulletType.Heavy, BulletType.Normal
        },
    new BulletType[] { BulletType.Heavy, BulletType.Normal, BulletType.Heavy }
        // 可以继续添加更多关卡的子弹类型序列
    };

    public int currentLevel = 0; // 当前关卡
    public float bpm;

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

    [SerializeField] AudioSource audioSource;

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
        float[] fireTimes = fireIntervals[currentLevel]; // 记录子弹的发射时间，单位为 beat
        BulletType[] currentBulletSequence = bulletSequences[currentLevel];

        float beatDuration = 60f / bpm; // 每个 beat 对应的时间（秒）

        float startTime = Time.time; // 记录场景开始的时间
        int bulletIndex = 0;

        while (bulletIndex < fireTimes.Length)
        {
            float elapsedTime = Time.time - startTime; // 计算从场景开始到现在经过的时间（秒）
            float currentTimeInBeats = elapsedTime / beatDuration; // 将场景时间转换为 beat

            // 如果当前的 beat 数达到了下一个子弹的发射时间
            if (currentTimeInBeats >= fireTimes[bulletIndex])
            {
                SpawnBullet(currentBulletSequence[bulletIndex]); // 发射子弹
                bulletIndex++; // 继续下一个子弹
            }

            yield return null; // 等待一帧
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
