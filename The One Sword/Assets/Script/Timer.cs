using TMPro;  // 引入TextMeshPro命名空间
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timer;
    private TextMeshProUGUI timerText;
    [SerializeField] BulletSpawner spawner;

    void Start()
    {
        // 获取TextMeshPro组件
        timerText = GetComponent<TextMeshProUGUI>();
        timer = 0f; // 初始化计时器
    }

    void Update()
    {
        timer = spawner.GetTimeInBeats();

        timerText.text = timer.ToString("F2");
    }
}
