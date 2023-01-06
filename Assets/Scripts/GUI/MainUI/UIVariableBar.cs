using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class UIVariableBar : MonoBehaviour
{
    [SerializeField] private FloatReference maxValue;
    [SerializeField] private FloatReference currentValue;

    [SerializeField] private Image imageValue;
    [SerializeField] private TextMeshProUGUI textValue;

    private void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        imageValue.fillAmount = currentValue.Value / maxValue.Value;
        textValue.text = currentValue.Value + " / " + maxValue.Value;
    }
}
