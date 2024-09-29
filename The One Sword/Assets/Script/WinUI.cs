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
        // 等待指定的时间
        yield return new WaitForSeconds(delay);

        // 设置 GameObject 的激活状态
        winUI.SetActive(isActive);
    }
}
