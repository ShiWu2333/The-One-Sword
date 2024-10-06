using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboCounter : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] private GameObject perfectTextPrefab; // ������ʾ "Perfect" ����ʾԤ����
    [SerializeField] private GameObject normalHitTextPrefab; // ������ʾ "Normal Hit" ����ʾԤ����
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
        comboText.text = currentCombo.ToString(); // ���� Combo �� UI ��ʾ

        // ���������������������С
        comboText.fontSize = Mathf.Clamp(30 + currentCombo * 0.5f, 30, 50); // ���������СΪ 30��ÿ����һ������������������ 0.5����������СΪ 100
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
        Vector3 endPosition = startPosition + new Vector3(0, moveDistance, 0); // �����ƶ�ָ���ĵ�λ
        TextMeshProUGUI textMesh = textInstance.GetComponent<TextMeshProUGUI>();
        Color startColor = textMesh.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0); // ������͸��

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