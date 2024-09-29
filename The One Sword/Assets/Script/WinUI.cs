using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinUI : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] GameObject winUI;

    [SerializeField] private float delayTime;

    void Start()
    {
        enemy.OnEnemyDie += Enemy_OnEnemyDie;
        winUI.SetActive(false);
    }

    private void Enemy_OnEnemyDie(object sender, System.EventArgs e)
    {
        StartCoroutine(SetActiveAfterDelay(delayTime, true));
    }

    IEnumerator SetActiveAfterDelay(float delay, bool isActive)
    {
        // �ȴ�ָ����ʱ��
        yield return new WaitForSeconds(delay);

        // ���� GameObject �ļ���״̬
        winUI.SetActive(isActive);
    }
}
