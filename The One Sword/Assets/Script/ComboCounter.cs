using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameObject perfectTextPrefab; // 用于显示 "Perfect" 的提示预制体
    [SerializeField] private GameObject normalHitTextPrefab; // 用于显示 "Normal Hit" 的提示预制体
    [SerializeField] private TextMeshProUGUI comboText;
    [SerializeField] private GameObject hitTextSpawnPoint;
    [SerializeField] private GameObject comboTextGB;
    private int currentCombo;

    private void Start()
    {
        currentCombo = 0;
        UpdateComboUI();
        playerController.OnPlayerHit += PlayerController_OnPlayerHit;
        playerController.OnPlayerDamaged += PlayerController_OnPlayerDamaged;
    }

    private void Update()
    {
        if (currentCombo > 0)
        {
            comboTextGB.gameObject.SetActive(true);
        }
        else
        {
            comboTextGB.gameObject.SetActive(false);
        }
    }

    private void PlayerController_OnPlayerDamaged(object sender, System.EventArgs e)
    {
        UpdateComboUI();
    }

    private void PlayerController_OnPlayerHit(object sender, System.EventArgs e)
    {
        currentCombo = playerController.attackCombo;
        UpdateComboUI();
        if (playerController.isPerfectHit)
        {
            ShowPerfectText();
        }
        else
        {
            ShowNormalHitText();
        }
    }

    private void UpdateComboUI()
    {
        if (currentCombo > 0)
        {
            comboTextGB.gameObject.SetActive(true);
        }
        else
        {
            comboTextGB.gameObject.SetActive(false);
        }
        comboText.text = currentCombo.ToString(); // 更新 Combo 的 UI 显示

        // 根据连击数量调整字体大小
        comboText.fontSize = Mathf.Clamp(30 + currentCombo * 0.5f, 30, 50); // 基础字体大小为 30，每增加一个连击数，字体增加 0.5，最大字体大小为 100
    }

    private void ShowPerfectText()
    {
        GameObject perfectTextInstance = Instantiate(perfectTextPrefab, hitTextSpawnPoint.transform.position, Quaternion.identity, transform);
        StartCoroutine(AnimateAndDestroyText(perfectTextInstance, 1.0f, 50));
    }

    private void ShowNormalHitText()
    {
        GameObject normalHitTextInstance = Instantiate(normalHitTextPrefab, hitTextSpawnPoint.transform.position, Quaternion.identity, transform);
        StartCoroutine(AnimateAndDestroyText(normalHitTextInstance, 0.5f, 20));
    }

    private IEnumerator AnimateAndDestroyText(GameObject textInstance, float duration, float moveDistance)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = textInstance.transform.position;
        Vector3 endPosition = startPosition + new Vector3(0, moveDistance, 0); // 向上移动指定的单位
        TextMeshProUGUI textMesh = textInstance.GetComponent<TextMeshProUGUI>();
        Color startColor = textMesh.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // 渐隐至透明

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            textInstance.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            textMesh.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        Destroy(textInstance);
    }
}