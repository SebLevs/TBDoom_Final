using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeleeWeaponImage : MonoBehaviour
{
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private float shakeAmount = 30f;
    [SerializeField] private float shakeTime = 0.5f;

    private float shakeTimer = 0;
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private Image weaponImage;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        currentPosition = initialPosition;
        weaponImage = GetComponent<Image>();
        weaponImage.enabled = false;
        UpdateWeaponImage();
    }

    public void UseWeapon()
    {
        StartCoroutine(StartUsingWeapon());
    }

    private IEnumerator StartUsingWeapon()
    {
        weaponImage.enabled = true;
        ShakeWeapon();
        yield return new WaitForSeconds(weaponInventory.EquippedMeleeWeapon.FiringRate / 2);
        weaponImage.enabled = false;
    }

    public void UpdateWeaponImage()
    {
        weaponImage.sprite = weaponInventory.EquippedMeleeWeapon.WeaponPlayerSprite;
    }

    public void ShakeWeapon()
    {
        shakeTimer = shakeTime;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.position = Vector3.zero;
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            var shakeOffset = new Vector3(Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            transform.localPosition = currentPosition + shakeOffset * Time.timeScale; ;
        }
        else
        {
            transform.localPosition = currentPosition;
        }
    }
}
