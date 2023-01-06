using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SetVol : MonoBehaviour
{
    [SerializeField] private AudioMixer audioM;
    [SerializeField] private string nameParam;
    [SerializeField] private float defaultValue = 0.3f;
    private Slider slider;

    // Start is called before the first frame update
    void Awake()
    {
        slider = GetComponent<Slider>();
        float vol;
        if (PlayerPrefs.HasKey(nameParam))
        {
            vol = PlayerPrefs.GetFloat(nameParam, defaultValue);
        }
        else
        {
            vol = defaultValue;
        }
        slider.value = vol;
        SetVolume(vol);
    }

    public void SetVolume(float vol)
    {
        if (vol > 0)
        {
            audioM.SetFloat(nameParam, Mathf.Log10(vol) * 30f);
        }
        else
        {
            audioM.SetFloat(nameParam, -100f);
        }
        PlayerPrefs.SetFloat(nameParam, vol);
        PlayerPrefs.Save();
        slider.value = vol;
    }

    public void AllDefaultValue()
    {
        var vols = FindObjectsOfType<SetVol>();
        foreach(SetVol vol in vols)
        {
            vol.ApplyDefault();
        }
    }

    public void ApplyDefault()
    {
        SetVolume(defaultValue);
    }
}
