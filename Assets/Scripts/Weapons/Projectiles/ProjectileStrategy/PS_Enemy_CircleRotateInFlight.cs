using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/Enemy/CircleRotate", fileName = "CircleRotateProjectileStrategySO")]
public class PS_Enemy_CircleRotateInFlight : IProjectileStrategy
{
    [SerializeField] private int shots = 40;
    [SerializeField] private float shotsInterval = 0.1f;
    [SerializeField] private int shotsWaves = 3;

    private float shotsIntervalTimer;
    private int currentShot = 0;

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
        if (currentShot >= shots)
        {
            currentShot = 0;
            shotsWaves--;
        }
        shotsIntervalTimer = shotsInterval;
        var splitAngleIncrement = 360 / shots;
        var currentSplitAngle = m_projectile.PhysicalProjectile.transform.forward;
        currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement * currentShot, 0) * currentSplitAngle;
        var newPosition = m_projectile.transform.position;
        var newRotation = Quaternion.LookRotation(currentSplitAngle, Vector3.up);
        List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
        var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(newPosition, newRotation);
        newProjectile.Initialize(m_projectile.Weapon, m_projectile.Weapon.ProjectileDefinition, newStrategies, m_projectile.TargetMask, m_projectile.LivingEntity);
        currentShot++;
        // currentSplitAngle = Quaternion.Euler(0, splitAngleIncrement, 0) * currentSplitAngle;
    }
}
