using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetGraphic : MonoBehaviour
{
    private string[] gfxNames;
    [SerializeField] private TMP_Dropdown gfxDropdown;

    private Resolution[] resNames;
    [SerializeField] private TMP_Dropdown resDropdown;

    [SerializeField] private Toggle fullscreenToggle;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_WEBGL
        resDropdown.interactable = false;
        fullscreenToggle.interactable = false;
#endif
        gfxNames = QualitySettings.names;
        List<string> gfxDropOptions = new List<string>();
        foreach (string s in gfxNames)
        {
            gfxDropOptions.Add(s);
        }
        gfxDropdown.ClearOptions();
        gfxDropdown.AddOptions(gfxDropOptions);
        gfxDropdown.value = QualitySettings.GetQualityLevel();

        resNames = Screen.resolutions;
        List<string> resDropOptions = new List<string>();
        int i = 0;
        int pos = 0;
        Resolution currentRes = Screen.currentResolution;
        foreach (Resolution r in resNames)
        {
            string val = r.width.ToString() + " x " + r.height.ToString();
            resDropOptions.Add(val);
            if (r.width == Screen.width &&
                r.height == Screen.height)
            {
                pos = i;
                Debug.Log(resNames[pos]);
            }
            i++;
        }
        resDropdown.ClearOptions();
        resDropdown.AddOptions(resDropOptions);
        resDropdown.value = pos;

        fullscreenToggle.isOn = Screen.fullScreen;
    }

    public void SetGraphics()
    {
        SetQuality();
#if !UNITY_WEBGL
        SetResolution();
        SetFullscreen();
#endif
    }

    private void SetQuality()
    {
        QualitySettings.SetQualityLevel(gfxDropdown.value, true);
    }

    private void SetResolution()
    {
        Resolution res = resNames[resDropdown.value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);

    }

    private void SetFullscreen()
    {
        if (fullscreenToggle.isOn)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }
}
