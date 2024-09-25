using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    // 音乐音频源
    public AudioSource musicSource;

    // 延迟时间（秒）
    public float delayTime = 2.0f;

    void Start()
    {
        // 启动协程延迟播放音乐
        StartCoroutine(PlayMusicWithDelay());
    }

    // 协程用于控制延迟播放
    IEnumerator PlayMusicWithDelay()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delayTime);

        // 播放音乐
        musicSource.Play();
    }
}
