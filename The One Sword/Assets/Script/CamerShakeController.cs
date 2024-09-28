using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerShakeController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float reflectShakeAmplitude;
    [SerializeField] private float damagedShakeAmplitude;

    private float initialAmplitude;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        initialAmplitude = perlinNoise.m_AmplitudeGain;

        playerController.OnPlayerHit += PlayerController_OnPlayerHit;
        playerController.OnPlayerDamaged += PlayerController_OnPlayerDamaged;
    }


    void OnDestroy()
    {
        // 确保在对象销毁时移除事件监听器，防止内存泄漏
        playerController.OnPlayerHit -= PlayerController_OnPlayerHit;
        playerController.OnPlayerDamaged -= PlayerController_OnPlayerDamaged;
    }

    private void PlayerController_OnPlayerHit(object sender, System.EventArgs e)
    {
        StartCoroutine(ShakeCamera(shakeDuration, reflectShakeAmplitude));
    }

    private void PlayerController_OnPlayerDamaged(object sender, System.EventArgs e)
    {
        StartCoroutine(ShakeCamera(shakeDuration, damagedShakeAmplitude));
    }

    IEnumerator ShakeCamera(float duration, float amplitude)
    {
        float elapsedTime = 0f;
        perlinNoise.m_AmplitudeGain = amplitude;  // 设置抖动强度

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 恢复初始的振幅值
        perlinNoise.m_AmplitudeGain = initialAmplitude;
    }
}
