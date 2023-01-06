using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickableManager : MonoBehaviour
{
    public static PickableManager instance;

    [SerializeField] private WeaponsInventorySO weaponsInventorySO;
    [SerializeField] private FloatReference currentHealth;
    [SerializeField] private FloatReference maxHealth;
    [SerializeField] private FloatReference currentArmor;
    [SerializeField] private FloatReference maxArmor;
    [SerializeField] private FloatReference currentCurrency;
    [SerializeField] private FloatReference maxCurrency;

    [SerializeField] private UnityEvent healthAsChange;
    [SerializeField] private UnityEvent armorAsChange;
    [SerializeField] private UnityEvent ammoAsChange;
    [SerializeField] private UnityEvent secondaryAsChange;
    [SerializeField] private UnityEvent currencyAsChange;

    [SerializeField] private AudioClip pickupSound;

    private AudioSource myAudioSource;
    private float myAudioSourcePitch;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSourcePitch = myAudioSource.pitch;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPickableValues()
    {
        currentHealth.Value = maxHealth.Value;
        currentArmor.Value = 0;
        currentCurrency.Value = 0;
    }

    public void SellWeapon(WeaponSO weaponSO)
    {
        currentCurrency.Value += weaponSO.CurrencyValue;
        currencyAsChange.Invoke();
    }

    public bool BuyPickable(PickableSO pickableSO)
    {
        if (currentCurrency.Value >= pickableSO.MerchantPrice)
        {
            if (PickPickable(pickableSO))
            {
                currentCurrency.Value -= pickableSO.MerchantPrice;
                currencyAsChange.Invoke();
                return true;
            }
            return false;
        }
        return false;
    }

    public bool PickPickable(PickableSO pickableSO)
    {
        bool verification = false;
        if (pickableSO.HealthValue > 0)
        {
            if (currentHealth.Value < maxHealth.Value)
            {
                if (pickableSO.HealthValueIsPercent)
                {
                    currentHealth.Value += maxHealth.Value * (pickableSO.HealthValue / 100);
                }
                else
                {
                    currentHealth.Value += pickableSO.HealthValue;
                }
                if (currentHealth.Value > maxHealth.Value)
                {
                    currentHealth.Value = maxHealth.Value;
                }
                verification = true;
                healthAsChange.Invoke();
            }
        }
        if (pickableSO.ArmorValue > 0)
        {
            if (currentArmor.Value < maxArmor.Value)
            {
                if (pickableSO.ArmorValueIsPercent)
                {
                    currentArmor.Value += maxArmor.Value * (pickableSO.ArmorValue / 100);
                }
                else
                {
                    currentArmor.Value += pickableSO.ArmorValue;
                }
                if (currentArmor.Value > maxArmor.Value)
                {
                    currentArmor.Value = maxArmor.Value;
                }
                verification = true;
                armorAsChange.Invoke();
            }
        }
        if (pickableSO.AmmoValue > 0)
        {
            if (weaponsInventorySO.EquippedMainWeapon.CurrentAmmo < weaponsInventorySO.EquippedMainWeapon.MaxAmmo)
            {
                if (pickableSO.AmmoValueIsPercent)
                {
                    weaponsInventorySO.EquippedMainWeapon.CurrentAmmo += (int)(weaponsInventorySO.EquippedMainWeapon.MaxAmmo * (pickableSO.AmmoValue / 100));
                }
                else
                {
                    weaponsInventorySO.EquippedMainWeapon.CurrentAmmo += (int)pickableSO.AmmoValue;
                }
                if (weaponsInventorySO.EquippedMainWeapon.CurrentAmmo > weaponsInventorySO.EquippedMainWeapon.MaxAmmo)
                {
                    weaponsInventorySO.EquippedMainWeapon.CurrentAmmo = weaponsInventorySO.EquippedMainWeapon.MaxAmmo;
                }
                verification = true;
                ammoAsChange.Invoke();
            }
        }
        if (pickableSO.SecondaryValue > 0)
        {
            if (weaponsInventorySO.EquippedSecondaryWeapon.CurrentClip < weaponsInventorySO.EquippedSecondaryWeapon.MaxClip)
            {
                if (pickableSO.SecondaryValueIsPercent)
                {
                    weaponsInventorySO.EquippedSecondaryWeapon.CurrentClip += (int)(weaponsInventorySO.EquippedSecondaryWeapon.MaxClip * (pickableSO.SecondaryValue / 100));
                }
                else
                {
                    weaponsInventorySO.EquippedSecondaryWeapon.CurrentClip += (int)pickableSO.SecondaryValue;
                }
                if (weaponsInventorySO.EquippedSecondaryWeapon.CurrentClip > weaponsInventorySO.EquippedSecondaryWeapon.MaxClip)
                {
                    weaponsInventorySO.EquippedSecondaryWeapon.CurrentClip = weaponsInventorySO.EquippedSecondaryWeapon.MaxClip;
                }
                verification = true;
                secondaryAsChange.Invoke();
            }
        }
        if (pickableSO.CurrencyValue > 0 && currentCurrency.Value < maxCurrency.Value)
        {
            if (currentCurrency.Value < maxCurrency.Value)
            {
                currentCurrency.Value += pickableSO.CurrencyValue;
                if (currentCurrency.Value > maxCurrency.Value)
                {
                    currentCurrency.Value = maxCurrency.Value;
                }
                verification = true;
                currencyAsChange.Invoke();
            }
        }
        if (verification)
        {
            StopCoroutine(ResetPitch());
            myAudioSource.PlayOneShot(pickupSound);
            myAudioSource.pitch += 0.05f;
            StartCoroutine(ResetPitch());
        }
        return verification;
    }

    private IEnumerator ResetPitch()
    {
        yield return new WaitForSeconds(1.0f);
        myAudioSource.pitch = myAudioSourcePitch;
    }
}
