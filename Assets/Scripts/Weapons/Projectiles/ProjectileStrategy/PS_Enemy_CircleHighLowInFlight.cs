using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/Enemy/CircleHighLow", fileName = "CircleHighLowProjectileStrategySO")]
public class PS_Enemy_CircleHighLowInFlight : IProjectileStrategy
{
    [SerializeField] private int shots = 20;
    [SerializeField] private float shotsInterval = 1;
    [SerializeField] private int shotsWaves = 5;

    private float shotsIntervalTimer;

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
        shotsIntervalTimer -= Time.deltaTime;
        if (shotsIntervalTimer > 0)
        {
            return;
        }
        if (shotsWaves <= 0)
        {
            m_projectile.ReturnToPool();
            return;
        }
        shotsWaves--;
        shotsIntervalTimer = shotsInterval;
        var splitAngleIncrement = 360 / shots;
        var currentSplitAngle = m_projectile.PhysicalProjectile.transform.forward;
        var newPosition = m_projectile.transform.position;
        if (shotsWaves%2 == 1)
        {
            currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement / 2, 0) * currentSplitAngle;
            newPosition += new Vector3(0, 0.4f, 0);
        }
        for (int i = 0; i < shots; i++)
        {
            var newRotation = Quaternion.LookRotation(currentSplitAngle, Vector3.up);
            List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
            var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition, newRotation);
            newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, m_projectile.LivingEntity);
            currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement, 0) * currentSplitAngle;
        }
    }
}
