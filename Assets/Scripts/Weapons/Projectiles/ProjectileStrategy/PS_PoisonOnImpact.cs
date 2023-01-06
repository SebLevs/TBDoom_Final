using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/PoisonOnImpact", fileName = "PoisonOnImpactProjectileStrategySO")]
public class PS_PoisonOnImpact : IProjectileStrategy
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
        m_livingEntity.Poison();
        // m_livingEntity.ForEach(x => x.Poison());
    }
}
