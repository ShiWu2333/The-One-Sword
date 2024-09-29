using TMPro;  // ����TextMeshPro�����ռ�
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timer;
    private TextMeshProUGUI timerText;
    [SerializeField] BulletSpawner spawner;

    void Start()
    {
        // ��ȡTextMeshPro���
        timerText = GetComponent<TextMeshProUGUI>();
        timer = 0f; // ��ʼ����ʱ��
    }

    void Update()
    {
        timer = spawner.GetTimeInBeats();

        timerText.text = timer.ToString("F2");
    }
}
