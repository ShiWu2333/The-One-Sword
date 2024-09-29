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
        // ȷ���ڶ�������ʱ�Ƴ��¼�����������ֹ�ڴ�й©
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
        perlinNoise.m_AmplitudeGain = amplitude;  // ���ó�ʼ����ǿ��

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            // ���㵱ǰ���ǿ�ȣ�����ʱ���𽥼�С
            float currentAmplitude = Mathf.Lerp(amplitude, 0f, elapsedTime / duration);
            perlinNoise.m_AmplitudeGain = currentAmplitude;

            yield return null; // �ȴ�һ֡
        }

        // ȷ�������������Ϊ0
        perlinNoise.m_AmplitudeGain = 0f;
    }
}
