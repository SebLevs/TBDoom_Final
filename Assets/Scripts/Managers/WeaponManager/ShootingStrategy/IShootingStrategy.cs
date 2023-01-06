using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IShootingStrategy : ScriptableObject
{
    public abstract bool Press(WeaponManager weaponManager);
    public abstract void Release(WeaponManager weaponManager);
}
