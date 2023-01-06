using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private bool isDebugOn = false;

    private WeaponSO myWeapon;
    private List<IProjectileStrategy> myProjectileStrategies;

    private float bulletColliderSize = 0.0f;
    private float objectMainColliderSize = 0.0f;

    public void SpawnProjectile(WeaponSO weapon, Projectile projectile, List<IProjectileStrategy> projectileStrategies, LivingEntityContext livingEntity, LayerMask targetMask, bool isFromEnemy, bool isEnemyProjectile)
    {
        myWeapon = weapon;
        myProjectileStrategies = projectileStrategies;

        // First setter
        if (bulletColliderSize == 0)
            bulletColliderSize = GetColliderSize(projectile.transform);
        if (objectMainColliderSize == 0 && livingEntity != null)
            objectMainColliderSize = GetColliderSize(livingEntity.transform);

        var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(transform.position, transform.rotation);
        newProjectile.gameObject.SetActive(true);
        newProjectile.IsEnemyProjectile = isEnemyProjectile;
        if (!isEnemyProjectile)
        {
            if (newProjectile.gameObject.layer == LayerMask.NameToLayer("EnemyPhysicalProjectile"))
                newProjectile.gameObject.layer = LayerMask.NameToLayer("PlayerPhysicalProjectile");
            else if (newProjectile.gameObject.layer == LayerMask.NameToLayer("EnemyEtherialProjectile"))
                newProjectile.gameObject.layer = LayerMask.NameToLayer("PlayerEtherialProjectile");
        }
        newProjectile.transform.SetParent(transform);
        newProjectile.TargetMask = targetMask;
        // newProjectile.InitializeProjectile(weapon, myProjectileStrategies, this);

        if (newProjectile is RaycastProjectile)
        {
            (newProjectile as RaycastProjectile).ShootRaycast();// weapon);
        }
        else if (newProjectile is PhysicalProjectile)
        {
            if (isFromEnemy)
                newProjectile.transform.position = transform.position + transform.forward * (objectMainColliderSize + bulletColliderSize + 0.05f);

            //if (weapon.Impulsion > 0)
            //{
            //    (newProjectile as PhysicalProjectile).AddImpulsion(transform.TransformVector((newProjectile as PhysicalProjectile).ImpulsionDirection).normalized * weapon.Impulsion);
            //}

            if (weapon.Spread > 0)
            {
                var spreadDirection = transform.TransformVector(new Vector3(Random.Range(-weapon.Spread, weapon.Spread), Random.Range(-weapon.Spread, weapon.Spread), 0));
                (newProjectile as PhysicalProjectile).AddVelocity(spreadDirection);
            }

            if (GetComponent<Rigidbody>() != null)
            {
                var addedVelocity = GetComponent<Rigidbody>().velocity * 0.25f;
                (newProjectile as PhysicalProjectile).AddVelocity(addedVelocity);
            }
        }
        StaticDebugger.SimpleDebugger(isDebugOn, newProjectile.name + " was instantiated");
    }

    private float GetColliderSize(Transform otherTransform)
    {
        if (!otherTransform)
            return 0.0f;

        float radius = 0.0f;
        float tempRadius = 0.0f;
        Collider[] otherColliders = otherTransform.GetComponents<Collider>();

        // Because Collider class is ugly and stupid and boring, we must get collider's enemyType and typecast to access data
        // Get radius and compare against last biggest radius found
        foreach (var collider in otherColliders)
        {
            if (collider is SphereCollider)                             // SPHERE
                tempRadius = (collider as SphereCollider).radius;
            else if (collider is CapsuleCollider)                       // CAPSULE
                tempRadius = (collider as CapsuleCollider).radius;
            else if (collider is BoxCollider)                           // BOX
                tempRadius = (collider as BoxCollider).size.z / 2;
            else if (collider is MeshCollider)                          // MESH
                tempRadius = (collider as MeshCollider).bounds.size.z;      // May need further testing

            if (tempRadius > radius)
                radius = tempRadius;
        }

        return radius;
    }

    //public void SetColliderSize_MainObject()
    //{
    //    objectMainColliderSize = GetColliderSize(transform.parent);
    //}

    //public void SetColliderSize_Bullet()
    //{
    //    bulletColliderSize = GetColliderSize(myWeapon.Projectile.transform);
    //}
}
