using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerShakeController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Enemy enemy;

    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin perlinNoise;
    [SerializeField] private float shakeDuration;
    [SerializeField] private float reflectShakeAmplitude;
    [SerializeField] private float damagedShakeAmplitude;
    [SerializeField] private float playerShakeFrequency;

    [SerializeField] private float enemyDieShakeDuration;
    [SerializeField] private float enemyDieShakeAmplitude;
    [SerializeField] private float enemyDieShakeFrequency;

    private float initialAmplitude;

    void Start()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        perlinNoise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        initialAmplitude = perlinNoise.m_AmplitudeGain;

        playerController.OnPlayerHit += PlayerController_OnPlayerHit;
        playerController.OnPlayerDamaged += PlayerController_OnPlayerDamaged;
        enemy.OnEnemyDie += Enemy_OnEnemyDie;
    }

    private void Update()
    {
        if (enemy.enemyIsDead == true)
        {
            playerController.OnPlayerHit -= PlayerController_OnPlayerHit;
            playerController.OnPlayerDamaged -= PlayerController_OnPlayerDamaged;
        }
    }

    void OnDestroy()
    {
        // 确保在对象销毁时移除事件监听器，防止内存泄漏
        playerController.OnPlayerHit -= PlayerController_OnPlayerHit;
        playerController.OnPlayerDamaged -= PlayerController_OnPlayerDamaged;
        enemy.OnEnemyDie -= Enemy_OnEnemyDie;
    }
    private void Enemy_OnEnemyDie(object sender, System.EventArgs e)
    {
        StartCoroutine(ShakeCamera(enemyDieShakeDuration, enemyDieShakeAmplitude, enemyDieShakeFrequency));
    }

    private void PlayerController_OnPlayerHit(object sender, System.EventArgs e)
    {
        StartCoroutine(ShakeCamera(shakeDuration, reflectShakeAmplitude, playerShakeFrequency));
    }

    private void PlayerController_OnPlayerDamaged(object sender, System.EventArgs e)
    {
        StartCoroutine(ShakeCamera(shakeDuration, damagedShakeAmplitude, playerShakeFrequency));
    }

    IEnumerator ShakeCamera(float duration, float amplitude, float frequency)
    {
        float elapsedTime = 0f;
        perlinNoise.m_AmplitudeGain = amplitude;  // 设置初始抖动强度

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // 计算当前振幅强度，随着时间逐渐减小
            float currentAmplitude = Mathf.Lerp(amplitude, 0f, elapsedTime / duration);
            perlinNoise.m_AmplitudeGain = currentAmplitude;

            yield return null; // 等待一帧
        }

        // 确保最终振幅设置为0
        perlinNoise.m_AmplitudeGain = 0f;
    }
}
