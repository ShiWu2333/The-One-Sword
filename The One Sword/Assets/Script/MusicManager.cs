using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // ������ƵԴ
    public AudioSource musicSource;

    // �ӳ�ʱ�䣨�룩
    public float delayTime = 2.0f;

    void Start()
    {
        // ����Э���ӳٲ�������
        StartCoroutine(PlayMusicWithDelay());
    }

    // Э�����ڿ����ӳٲ���
    IEnumerator PlayMusicWithDelay()
    {
        // �ȴ�ָ�����ӳ�ʱ��
        yield return new WaitForSeconds(delayTime);

        // ��������
        musicSource.Play();
    }
}
