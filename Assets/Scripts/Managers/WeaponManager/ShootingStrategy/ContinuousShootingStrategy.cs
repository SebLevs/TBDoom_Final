using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ShootingStrategy/Continuous", fileName = "ContinuousShootingStrategySO")]
public class ContinuousShootingStrategy : IShootingStrategy
{
    public override bool Press(WeaponManager weaponManager)
    {
        weaponManager.TriggerWeapon();
        return true;
    }

    public override void Release(WeaponManager weaponManager) { }
}
