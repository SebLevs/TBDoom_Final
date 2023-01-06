using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ShootingStrategy/Charge", fileName = "ChargeShootingStrategySO")]
public class ChargeShootingStrategy : IShootingStrategy
{
    // ********** Attributes are currently only for ideas **********
    //[SerializeField] private bool needsFullCharge;
    //[SerializeField] private bool firesOnFullCharge;
    //[SerializeField] private bool chargeInfluenceDistance;
    // *************************************************************

    public override bool Press(WeaponManager weaponManager)
    {
        weaponManager.ChargeWeapon();
        return true;
    }

    public override void Release(WeaponManager weaponManager)
    {
        if (weaponManager.Weapon.ChargeTime != 0 && weaponManager.Weapon.CurrentChargeTime != 0)
        {
            if (weaponManager.ChargeWeapon())
            {
                weaponManager.TriggerWeapon();
            }
            weaponManager.Weapon.CurrentChargeTime = 0;
        }
    }
}
