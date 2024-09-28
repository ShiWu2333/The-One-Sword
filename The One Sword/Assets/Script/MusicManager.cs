using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    // 音乐音频源
    public AudioSource musicSource;
    public float deathPitch = 0.5f;      // 玩家死亡时的音调
    public float deathVolume = 0f;     // 玩家死亡时的音量
    public float transitionDuration = 2f;  // 音调和音量变化的持续时间

    // 延迟时间（秒）
    public float delayTime = 2.0f;

    void Start()
    {
        // 启动协程延迟播放音乐
        StartCoroutine(PlayMusicWithDelay());
        playerController.OnPlayerDie += PlayerController_OnPlayerDie;
    }

    void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnPlayerDie -= PlayerController_OnPlayerDie;
        }
    }

    private void PlayerController_OnPlayerDie(object sender, System.EventArgs e)
    {
        StartCoroutine(ChangeMusicOnDeath());
    }

    // 协程用于控制延迟播放
    IEnumerator PlayMusicWithDelay()
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delayTime);

        // 播放音乐
        musicSource.Play();
    }

    private IEnumerator ChangeMusicOnDeath()
    {
        float elapsedTime = 0f;
        float initialPitch = musicSource.pitch;
        float initialVolume = musicSource.volume;

        // 平滑地改变音量和音调
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            musicSource.pitch = Mathf.Lerp(initialPitch, deathPitch, elapsedTime / transitionDuration);
            musicSource.volume = Mathf.Lerp(initialVolume, deathVolume, elapsedTime / transitionDuration);
            yield return null;
        }

        // 确保最终值精确设置
        musicSource.pitch = deathPitch;
        musicSource.volume = deathVolume;

        // 完全停止音乐（可选）
        if (deathVolume == 0f)
        {
            musicSource.Stop();
        }
    }

}
