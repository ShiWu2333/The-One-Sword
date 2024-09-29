using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] Enemy enemy;

    // ������ƵԴ
    public AudioSource musicSource;
    public float deathPitch = 0.5f;      // �������ʱ������
    public float deathVolume = 0f;     // �������ʱ������
    public float playerDeadTransitionDuration = 2f;  // �����������仯�ĳ���ʱ��
    public float enemyDeadTransitionDuration = 1f; // ��������ʱ�����������仯�ĳ���ʱ��
    public float enemyDeadPitch = 1.5f;

    // �ӳ�ʱ�䣨�룩
    public float delayTime = 2.0f;

    void Start()
    {
        // ����Э���ӳٲ�������
        StartCoroutine(PlayMusicWithDelay());
        playerController.OnPlayerDie += PlayerController_OnPlayerDie;
        enemy.OnEnemyDie += Enemy_OnEnemyDie;
    }


    void OnDestroy()
    {
        if (playerController != null)
        {
            playerController.OnPlayerDie -= PlayerController_OnPlayerDie;
        }
        enemy.OnEnemyDie -= Enemy_OnEnemyDie;
    }
    private void Enemy_OnEnemyDie(object sender, System.EventArgs e)
    {
        StartCoroutine(ChangeMusicOnDeath(enemyDeadTransitionDuration, enemyDeadPitch));
    }

    private void PlayerController_OnPlayerDie(object sender, System.EventArgs e)
    {
        StartCoroutine(ChangeMusicOnDeath(playerDeadTransitionDuration, deathPitch));
    }

    // Э�����ڿ����ӳٲ���
    IEnumerator PlayMusicWithDelay()
    {
        // �ȴ�ָ�����ӳ�ʱ��
        yield return new WaitForSeconds(delayTime);

        // ��������
        musicSource.Play();
    }


    private IEnumerator ChangeMusicOnDeath(float transitionDuration, float deathPitch)
    {
        float elapsedTime = 0f;
        float initialPitch = musicSource.pitch;
        float initialVolume = musicSource.volume;

        // ƽ���ظı�����������
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            musicSource.pitch = Mathf.Lerp(initialPitch, deathPitch, elapsedTime / transitionDuration);
            musicSource.volume = Mathf.Lerp(initialVolume, deathVolume, elapsedTime / transitionDuration);
            yield return null;
        }

        // ȷ������ֵ��ȷ����
        musicSource.pitch = deathPitch;
        musicSource.volume = deathVolume;

        // ��ȫֹͣ���֣���ѡ��
        if (deathVolume == 0f)
        {
            musicSource.Stop();
        }
    }

}
