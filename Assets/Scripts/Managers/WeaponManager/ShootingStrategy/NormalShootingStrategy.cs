using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ShootingStrategy/Normal", fileName = "NormalShootingStrategySO")]
public class NormalShootingStrategy : IShootingStrategy
{
    public override bool Press(WeaponManager weaponManager)
    {
        weaponManager.TriggerWeapon();
        return false;
    }

    public override void Release(WeaponManager weaponManager) { }
}
