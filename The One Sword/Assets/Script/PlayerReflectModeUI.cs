using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReflectModeUI : MonoBehaviour
{
    [SerializeField] PlayerController controller;
    [SerializeField] Image chargeFill;

    public void UpdateChargeFill()
    {
        float currentCharge = controller.GetReflectModeCharge();
        float maxCharge = controller.GetReflectModeMax();
        chargeFill.fillAmount = currentCharge / maxCharge;
    }


}
