using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/SpreadShot", fileName = "SpreadShotProjectileStrategySO")]
public class PS_SpreadShotOnSpawn : IProjectileStrategy
{
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
        var weapon = m_projectile.Weapon;
        List<IProjectileStrategy> newStrategies = m_projectile.ExtractAllPS(this);
        for (int i = 0; i < weapon.BulletsNumber; i++)
        {
            var spreadDirection = m_projectile.transform.TransformVector(new Vector3(Random.Range(-weapon.Spread, weapon.Spread), Random.Range(-weapon.Spread, weapon.Spread), 0));
            var newRotation = Quaternion.LookRotation(m_projectile.transform.forward, Vector3.up) * Quaternion.Euler(spreadDirection);
            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(m_projectile.transform.position, newRotation);
            newProjectile.Initialize(weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, null, this);
        }
    }
}
