using TMPro;  // 引入TextMeshPro命名空间
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timer;
    private TextMeshProUGUI timerText;

    void Start()
    {
        // 获取TextMeshPro组件
        timerText = GetComponent<TextMeshProUGUI>();
        timer = 0f; // 初始化计时器
    }

    void Update()
    {
        // 每帧增加时间
        timer += Time.deltaTime;

        // 将时间转换为分钟和秒
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);

        // 更新TextMeshPro显示时间
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
