using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UICurrency : MonoBehaviour
{
    [SerializeField] private FloatReference currentValue;

    [SerializeField] private Image imageValue;
    [SerializeField] private TextMeshProUGUI textValue;

    private void Start()
    {
        UpdateValue();
    }

    public void UpdateValue()
    {
        textValue.text = currentValue.Value.ToString();
    }
}
