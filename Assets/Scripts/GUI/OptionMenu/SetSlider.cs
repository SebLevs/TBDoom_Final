using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSlider : MonoBehaviour
{
    [SerializeField] private string nameParam;
    [SerializeField] private float defaultValue = 0.3f;
    [SerializeField] private FloatReference floatReference;
    private Slider slider;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        float value;
        if (PlayerPrefs.HasKey(nameParam))
        {
            value = PlayerPrefs.GetFloat(nameParam, defaultValue);
        }
        else
        {
            value = defaultValue;
        }
        slider.value = value;
        SetSliderValue(value);
    }

    public void SetSliderValue(float value)
    {
        if (floatReference != null)
        {
            floatReference.Value = value / 100;
        }
        PlayerPrefs.SetFloat(nameParam, value);
        PlayerPrefs.Save();
        slider.value = value;
    }

    public void AllDefaultValue()
    {
        var vols = FindObjectsOfType<SetVol>();
        foreach (SetVol vol in vols)
        {
            vol.ApplyDefault();
        }
    }

    public void ApplyDefault()
    {
        SetSliderValue(defaultValue);
    }
}
