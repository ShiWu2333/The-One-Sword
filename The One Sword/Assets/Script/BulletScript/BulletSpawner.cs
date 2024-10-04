using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private float[][] fireIntervals =
    {
        new float[] // Level 0 的数据
        {
        3.23928f, 5.05905f, 6.33922f, 
        // 第二节：轻微增加密度，增加一些节奏感
        8.43899f, 9.66062f, 11.9186f, 
        // 第三节：重复节奏块，给玩家熟悉的感觉
        13.20078f, 13.63966f, 14.48155f, 15.33805f, 
        // 第四节：突然密集，让玩家紧张起来
        17.05953f, 18.76107f, 19.62014f, 
        // 第五节：节奏稍微放松，但不完全
        20.48059f, 21.75944f, 23.48037f, 
        // 第六节：快速连发
        24.76171f, 25.258f, 25.758f, 26.258f,
        // 第七节：再次重复熟悉的节奏
        28.61884f, 30.33804f, 31.20065f, 32.48061f,
        // 第八节：提升难度和密集度
        33.75813f, 35.06029f, 36.34128f, 
        // 第九节：进入放松阶段
        38.06166f, 39.3409f, 40.61951f, 
        // 第十节：重新加速，短暂的紧张感
        41.9002f, 43.20017f, 44.47963f,
        45.76067f, 47.48015f, 48.7618f, 
        // 重复节奏块
        45.76067f, 47.48015f, 48.7618f, 
        // 第十一节：随机发射间隔，打乱节奏
        50.05839f, 51.33855f, 53.4787f,
        54.66141f, 55.85972f, 56.661f, 
        // 密集短暂增加
        58.46082f, 60.05936f, 62.47803f, 
        // 第十二节：再次放松
        64.61875f, 66.05854f, 68.48097f, 
        // 结束阶段：节奏紧张，渐入尾声
        70.61822f, 72.34047f, 74.05933f,
        75.76078f, 77.4801f, 80.06018f,
        82.62055f, 85.19813f, 86.90083f,
        88.61874f, 90.33879f, 92.48165f,
        94.61969f, 96.75916f, 99.34111f,
        101.47984f, 104.05989f, 106.62055f, 
        // 尾声阶段
        109.20194f, 113.05999f, 116.48191f,
        119.90013f, 123.33808f, 126.7602f,
        130.20008f, 133.61925f, 137.05944f,
        140.48101f, 143.89854f, 147.33865f,
        150.75856f, 154.20153f, 157.62098f,
        161.06049f, 164.48158f, 169.18041f,
        172.19977f, 175.62085f, 179.05862f, 
        // 最终的渐入放松
        182.47843f, 185.92098f, 189.3386f,
        193.19909f, 196.62032f, 200.06168f,
        203.47912f, 206.92153f, 210.34059f
        },

        new float[] // Level 1 的数据
        {
            3.23928f, 4.84074f, 5.23957f, 6.43965f, 8.43899f, 9.66062f,
            11.65851f, 12.86023f, 15.65967f, 17.2611f, 18.06067f, 18.86094f,
            19.25846f, 20.46166f, 21.06058f, 21.25927f, 28.06085f, 29.2581f,
            34.46102f, 34.8586f, 40.85802f, 41.2582f, 45.26099f, 45.66127f,
            47.2609f, 48.05972f, 48.85911f, 50.46057f, 50.85854f, 52.46176f,
            53.66116f, 54.66141f, 54.85971f, 56.86167f, 57.25937f, 58.05953f,
            58.46082f, 60.05936f, 60.46052f, 62.86091f, 63.25886f, 66.05854f,
            66.45992f, 67.65833f, 68.45888f, 69.26047f, 69.65891f, 70.85992f,
            71.26131f, 74.86176f, 76.45898f, 77.25887f, 77.66088f, 80.06018f,
            80.45903f, 81.65985f, 85.26169f, 86.06137f, 86.86044f, 87.65833f,
            88.46028f, 89.25889f, 90.06084f, 90.85818f, 92.461f, 93.25893f,
            94.03821f, 94.86069f, 95.66087f, 100.0614f, 103.66101f, 112.86084f,
            115.65918f, 117.2592f, 118.85825f, 120.86156f, 122.06025f, 124.06149f,
            124.45855f, 125.65976f, 129.26044f, 130.45963f, 133.25974f, 134.86135f,
            136.85897f, 137.26066f, 138.46052f, 139.65826f, 143.26046f, 147.65847f,
            148.05886f, 149.65933f, 150.45864f, 150.85996f, 152.44115f, 152.8594f,
            154.06046f, 154.86023f, 155.63866f, 156.05806f, 157.25846f, 158.8399f,
            159.25848f, 160.45978f, 160.86185f, 162.46128f, 162.86104f, 163.65927f,
            164.45958f, 165.23997f, 165.65954f, 166.85805f, 167.66113f, 168.43838f,
            168.85826f, 170.0597f, 171.65961f, 172.05953f, 173.26059f, 173.65849f,
        174.45865f, 174.65808f, 176.04042f, 177.23857f
        },
        new float[] // Level 2 的数据
        {
            4.718311f, 7.080926f, 8.159974f, 8.579452f, 9.878297f, 10.501001f, 11.998129f, 13.300236f,
            13.938791f, 15.439067f, 17.358687f, 19.720610f, 20.160456f, 20.801320f, 21.440952f, 22.299606f,
            24.661146f, 25.718342f, 26.581840f, 26.801836f, 28.301326f, 29.160495f, 30.001938f, 31.941241f,
            32.578182f, 38.160761f, 39.019601f, 40.740362f, 42.441899f, 44.159806f, 45.878564f, 49.300944f,
            51.019681f, 51.879254f, 52.738950f, 57.880576f, 58.720795f, 59.379721f, 59.578935f, 59.798711f,
            60.019028f, 60.241643f, 61.298335f, 63.020977f, 63.438301f, 64.300395f, 64.741865f, 66.880596f,
            68.158044f, 69.880016f, 71.601919f, 73.299053f, 75.020519f, 76.741601f, 77.160859f, 78.018607f,
            78.440114f, 80.160446f, 81.879148f, 83.598637f, 84.879698f, 85.301735f, 86.159302f, 86.801040f,
            87.019378f, 87.438598f, 88.740945f, 89.161227f, 89.581306f, 90.439329f, 91.301395f, 92.161714f,
            92.578124f, 93.019452f, 93.858048f, 95.579363f, 96.860749f, 97.300460f, 99.021608f, 99.880971f,
            103.301571f, 104.160885f, 104.580608f, 105.018151f, 105.878671f, 106.300535f, 106.740939f, 107.581342f,
            108.440604f, 109.299074f, 110.579226f, 111.021043f, 111.440027f, 112.719661f, 113.579138f, 114.439979f,
            115.301512f, 116.158026f, 117.661337f, 117.880107f, 119.580367f, 119.798579f, 121.079530f, 121.299743f,
            121.500977f, 123.018522f, 123.218211f, 124.081571f, 124.499086f, 124.738400f, 125.138593f, 125.580357f,
            126.439071f, 126.859013f, 127.301346f, 128.161793f, 129.881171f, 131.380982f, 133.301388f, 134.799052f,
            135.021638f, 136.738173f, 138.218926f, 138.460518f, 139.301726f, 140.579408f, 141.018723f, 141.659870f,
            141.880901f, 147.021790f, 147.880077f, 148.738884f, 152.158663f, 153.881676f, 159.019726f, 161.600904f,
            162.441984f, 164.161742f, 165.881288f, 167.581380f, 168.438381f, 169.300609f, 169.521602f, 172.718554f,
            176.161332f, 179.581453f, 183.020978f, 184.741158f, 186.439885f, 188.160478f, 189.861325f, 191.580979f,
            193.298194f, 195.019672f, 196.521305f, 196.741926f, 197.160522f, 197.380054f, 199.300614f, 200.579964f
        },
        new float[] { 0.6f, 0.9f, 1.2f, 0.8f, 1.5f, 0.3f }, // Level 3 的数据
        // 可以继续添加更多关卡的发射间隔
    };

    // 定义每个关卡的子弹类型序列
    public BulletType[][] bulletSequences =
    {
        new BulletType[] { BulletType.Normal, BulletType.Heavy },
        new BulletType[] { BulletType.Normal, BulletType.Heavy },
        new BulletType[] { BulletType.Normal, BulletType.Heavy },
        new BulletType[] { BulletType.Normal, BulletType.Heavy },
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

    private float startTime;
    private GameObject bulletToSpawn;
    [SerializeField] private float HeavyBulletThreshold; //发射重型子弹的阈值，间隔大于多少秒会发射重型子弹

    [SerializeField] AudioSource audioSource;

    [SerializeField] private float maxBulletSpeed;
    [SerializeField] private float traMaxHeight;
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
        startTime = Time.time;
    }


    void Update()
    {
        //Debug.Log(timer);
        if (canSpawn && !isFiring)
        {
            StartCoroutine(FireBulletGroup());
             
        }
        
    }

    IEnumerator FireBulletGroup()
    {
        startTime = Time.time;
        isFiring = true;
        float[] fireTimes = fireIntervals[currentLevel]; // 记录子弹的发射时间，单位为 beat
        BulletType[] currentBulletSequence = new BulletType[fireTimes.Length]; // 用于存放发射的子弹类型

        float beatDuration = 60f / bpm; // 每个 beat 对应的时间（秒）
        int bulletIndex = 0;

        while (bulletIndex < fireTimes.Length)
        {
            float currentTimeInBeats = GetTimeInBeats();

            // 如果当前的 beat 数达到了下一个子弹的发射时间
            if (currentTimeInBeats >= fireTimes[bulletIndex])
            {
                // 计算当前子弹与前一个子弹的时间间隔
                if (bulletIndex > 0)
                {
                    float interval = fireTimes[bulletIndex] - fireTimes[bulletIndex - 1];

                    // 如果间隔大于等于 3 beat，发射重型子弹，否则发射普通子弹
                    if (interval >= HeavyBulletThreshold)
                    {
                        currentBulletSequence[bulletIndex] = BulletType.Heavy;
                    }
                    else
                    {
                        currentBulletSequence[bulletIndex] = BulletType.Normal;
                    }
                }
                else
                {
                    currentBulletSequence[bulletIndex] = BulletType.Normal;
                }

                SpawnBullet(currentBulletSequence[bulletIndex]); // 发射子弹
                bulletIndex++; // 继续下一个子弹

                // 等待一帧再检查是否需要发射更多子弹
                yield return null;
            }

            // 等待一帧再继续检查时间
            yield return null;
        }

        isFiring = false;
    }

    public float GetTimeInBeats()
    {
        float beatDuration = 60f / bpm; // 每个 beat 对应的时间（秒）
        float elapsedTime = Time.time - startTime; // 计算从场景开始到现在经过的时间（秒）
        float currentTimeInBeats = elapsedTime / beatDuration; // 将场景时间转换为 beat

        return currentTimeInBeats;
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
