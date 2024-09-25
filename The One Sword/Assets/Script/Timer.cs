using TMPro;  // ����TextMeshPro�����ռ�
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timer;
    private TextMeshProUGUI timerText;

    void Start()
    {
        // ��ȡTextMeshPro���
        timerText = GetComponent<TextMeshProUGUI>();
        timer = 0f; // ��ʼ����ʱ��
    }

    void Update()
    {
        // ÿ֡����ʱ��
        timer += Time.deltaTime;

        // ��ʱ��ת��Ϊ���Ӻ���
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer % 60F);

        // ����TextMeshPro��ʾʱ��
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
