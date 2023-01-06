using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableCanvas : MonoBehaviour
{
    [SerializeField] private Color healthColor;
    [SerializeField] private Color armorColor;
    [SerializeField] private Color ammoColor;
    [SerializeField] private Color secondaryColor;
    [SerializeField] private Color currencyColor;
    [SerializeField] private float flashTime = 0.1f;
    [SerializeField] private float colorAlpha = 0.25f;

    private Image myImage;

    // Start is called before the first frame update
    void Start()
    {
        myImage = GetComponent<Image>();
        myImage.enabled = false;
    }

    public void FlashHealthColor()
    {
        StartCoroutine(StartFlash(healthColor));
    }

    public void FlashArmorColor()
    {
        StartCoroutine(StartFlash(armorColor));
    }

    public void FlashAmmoColor()
    {
        StartCoroutine(StartFlash(ammoColor));
    }

    public void FlashSecondaryColor()
    {
        StartCoroutine(StartFlash(secondaryColor));
    }

    public void FlashCurrencyColor()
    {
        StartCoroutine(StartFlash(currencyColor));
    }

    private IEnumerator StartFlash(Color color)
    {
        color.a = colorAlpha;
        myImage.color = color;
        myImage.enabled = true;
        yield return new WaitForSeconds(flashTime);
        myImage.enabled = false;
    }
}
