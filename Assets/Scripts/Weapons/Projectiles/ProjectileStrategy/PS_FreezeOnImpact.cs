using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/FreezeOnImpact", fileName = "FreezeOnImpactProjectileStrategySO")]
public class PS_FreezeOnImpact : IProjectileStrategy
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
        m_livingEntity.Freeze();
        // m_livingEntity.ForEach(x => x.Freeze());
    }
}
