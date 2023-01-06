using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class Merchant : MonoBehaviour
{
    [SerializeField] private WeaponsInventorySO weaponInventory;
    [SerializeField] private UnityEvent mainWeaponHasChanged;

    [SerializeField] private TextMeshPro myPrice;

    public void SellWeapon()
    {
        if (weaponInventory.CarriedMainWeapons.Count > 1)
        {
            PickableManager.instance.SellWeapon(weaponInventory.EquippedMainWeapon);
            weaponInventory.CarriedMainWeapons.Remove(weaponInventory.EquippedMainWeapon);
            weaponInventory.EquippedMainWeapon = weaponInventory.CarriedMainWeapons[0];
            mainWeaponHasChanged.Invoke();
        }
    }

    public void UpdatePrice()
    {
        if (weaponInventory.CarriedMainWeapons.Count > 1)
        {
            myPrice.text = "I offer you " + weaponInventory.EquippedMainWeapon.CurrencyValue + "$ for this weapon";
        }
        else
        {
            myPrice.text = "I can't buy off your last weapon...";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdatePrice();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
