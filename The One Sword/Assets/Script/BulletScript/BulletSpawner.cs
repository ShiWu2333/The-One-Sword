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
        54.66141f, 55.85972f, 56.6611f, 
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
        },// Level 0 的数据
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
        }, // Level 1 的数据
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
        }, // Level 2 的数据
        new float[] // Level 3 的数据
        {
            5.61982f, 6.42154f, 7.62034f, 8.02092f, 12.01855f,
            12.81869f, 14.01848f, 15.6208f, 16.02003f, 16.81967f,
            18.42186f, 19.21983f, 19.62014f, 20.19958f, 20.81809f,
            21.21966f, 21.61943f, 22.02132f, 22.41989f, 22.81894f,
            24.82199f, 25.21864f, 25.61905f, 26.0195f, 26.41889f,
            26.99883f, 27.62164f, 28.42032f, 29.62193f, 30.01962f,
            30.81888f, 31.22164f, 31.61848f, 32.4217f, 33.61852f,
            34.41929f, 34.81967f, 35.6211f, 36.01834f, 36.4209f,
            37.22024f, 37.61978f, 38.01971f, 38.8194f, 39.4196f,
            40.01867f, 40.82176f, 41.22103f, 41.62105f, 42.41902f,
            42.81942f, 43.61867f, 44.02078f, 44.42055f, 44.81946f,
            45.41986f, 45.82062f, 46.21929f, 46.81942f, 48.01898f,
            48.4211f, 48.82152f, 49.62185f, 50.02191f, 50.42115f,
            51.01968f, 51.22152f, 52.22153f, 60.82025f, 61.62122f,
            62.42056f, 66.41884f, 70.41835f, 71.8186f, 72.01877f,
            76.02093f, 77.22151f, 78.41836f, 78.81916f, 81.22179f,
            83.2181f, 83.62045f, 84.41885f, 85.21939f, 85.61854f,
            86.01917f, 87.6193f, 88.81896f, 89.62182f, 90.41908f,
            90.81996f, 91.62039f, 92.61994f, 94.02121f, 95.21898f,
            96.81959f, 97.22058f, 98.41962f, 99.22191f, 100.01877f,
            100.81871f, 101.22019f, 101.61882f, 103.21961f, 104.02001f,
            104.82194f, 105.61949f, 106.4183f, 106.82068f, 107.22097f,
            108.42194f, 109.21983f, 110.01857f, 110.41979f, 110.81915f,
            111.62141f, 112.42075f, 113.2189f, 114.01992f, 115.22138f,
            116.02132f, 116.41878f, 117.2202f, 117.62115f, 118.42114f,
            119.22052f, 120.42115f, 121.21829f, 121.62064f, 122.21842f, 122.61989f,
            123.21821f, 124.02119f, 124.81983f, 125.22082f, 125.61882f,
            126.42139f, 127.21973f, 128.01958f, 128.82113f, 129.21866f,
            130.01988f, 131.2182f, 132.01858f, 132.82193f, 134.01998f,
            134.41923f, 135.02164f, 136.02138f, 137.2193f, 138.41936f,
            139.62098f, 140.42105f, 141.21807f, 142.4219f, 143.22091f, 144.01869f,
            145.22044f, 146.41872f, 147.22172f, 148.42146f, 149.62167f,
            150.4206f, 151.21888f, 152.41823f, 153.62192f, 154.8215f,
            156.02069f, 157.22172f, 158.42036f, 159.62195f, 160.82068f,
            161.22156f, 162.02047f, 162.81953f, 164.02048f, 165.22011f,
            166.02092f, 166.42024f, 167.02187f, 167.6214f, 168.42154f,
            169.01985f, 169.6191f, 170.01985f, 170.62071f, 171.02095f,
            171.61919f, 172.02047f, 172.61906f, 173.02097f, 173.62106f,
            174.01891f, 174.62008f, 175.02056f, 175.62085f, 176.02056f,
            176.82192f, 177.22192f, 178.01814f, 178.62043f, 179.21999f,
            179.81988f, 180.42057f, 181.02095f, 181.62116f, 182.42114f,
            183.02167f, 184.02132f, 184.82181f, 185.42063f, 186.02092f,
            186.41868f, 187.01837f, 188.01837f, 189.01985f, 190.02011f,
            191.21997f, 192.42137f, 193.02017f, 193.62096f, 194.22033f,
            195.22192f, 196.02132f, 196.62106f, 197.62106f, 198.42076f,
            199.01985f, 199.82181f, 200.42067f, 200.82197f, 201.62156f,
            202.01849f, 202.61994f, 203.22047f, 203.81988f, 204.41931f,
            205.01992f, 205.6191f, 206.21849f, 206.81868f, 207.41932f




        }, // Level 3 的数据
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
