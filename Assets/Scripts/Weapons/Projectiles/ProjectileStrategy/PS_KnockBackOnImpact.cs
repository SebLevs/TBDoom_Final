using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/KnockBack", fileName = "KnockDownProjectileStrategySO")]
public class PS_KnockBackOnImpact : IProjectileStrategy
{
    [Header("Behaviour Values")]
    [SerializeField] private float damage;

    public override void ExecuteColliderStrategy()
    {
        ExecuteStrategy();
    }

    public override void ExecuteRaycastStrategy()
    {
        ExecuteStrategy();
    }

    private void ExecuteStrategy()
    {
        m_livingEntity.KnockBack(m_projectile.Weapon.Knockback, m_projectile.transform.forward);
        // m_livingEntity.ForEach(x => x.KnockBack(m_projectile.Weapon.Knockback, m_projectile.transform.forward));
    }
}
