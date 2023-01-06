using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RaycastType { MELEE, SINGLE, SPREAD }

public class RaycastProjectile : Projectile
{
    //[SerializeField] private bool isDebugOn = false;

    //[SerializeField] private RaycastType raycastType;
    //[SerializeField] private float shootDelay = 0.1f;
    //[SerializeField] private bool showTrajectory = false;
    //[SerializeField] private LineRenderer trajectoryLineRenderer;
    
    //private RaycastHit myRaycastHit;
    //private bool isSpread = false;

    //public RaycastHit MyRaycastHit { get => myRaycastHit; set => myRaycastHit = value; }
    //public bool IsSpread { get => isSpread; set => isSpread = value; }

    //// [SerializeField] private bool copyStrategies = true;

    //// Start is called before the first frame update
    //public void ShootRaycast()//WeaponSO weapon)
    //{
    //    //if (copyStrategies)
    //    //    strategies = weapon.ProjectileStrategies;
    //    StartCoroutine(Shoot());
    //}

    //private IEnumerator Shoot()
    //{
    //    yield return new WaitForSeconds(shootDelay);
    //    ExecuteSpawnStrategies(myRaycastHit);
    //    foreach (IProjectileStrategy strategy in projectileStrategies)
    //    {
    //        if (strategy is PS_SpreadShotOnSpawn)
    //        {
    //            // Destroy(projectileSpawner.gameObject);
    //            StopAllCoroutines();
    //        }  
    //    }
    //    switch (raycastType)
    //    {
    //        case RaycastType.MELEE:
    //            ShootShortRayCast();// weapon);
    //            break;
    //        case RaycastType.SINGLE:
    //            ShootSingleRayCast();// weapon);
    //            break;
    //        case RaycastType.SPREAD:
    //            ShootMultipleRayCasts();// weapon);
    //            break;
    //    }
    //    // Death();
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    //public bool ShootShortRayCast()//WeaponSO weapon)
    //{
    //    //RaycastHit myRaycastHit;
    //    myRaycastHit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, m_weapon.Range, targetMask, isDebugOn);
    //    if (myRaycastHit.collider != null)
    //    {
    //        ExecuteFlightStrategies(myRaycastHit);
    //        StaticDebugger.SimpleDebugger(isDebugOn, myRaycastHit.collider.name + " was hit");
    //        if (showTrajectory)
    //        {
    //            var newTrajectory = Instantiate(trajectoryLineRenderer);
    //            newTrajectory.SetPosition(0, transform.position);
    //            newTrajectory.SetPosition(1, myRaycastHit.point);
    //        }

    //        ExecuteImpactStrategies(myRaycastHit);
    //        var livingEntity = myRaycastHit.collider.GetComponent<LivingEntityContext>();
    //        //ExecuteImpactStrategies(hit.point, livingEntity);
    //        if (livingEntity != null)
    //        {
    //            // livingEntity.TakeDamage(m_weapon.Damage, projectileSpawner.transform.position);
    //            //if (weapon.Knockback > 0)
    //            //{
    //            //    hit.collider.GetComponent<LivingEntityContext>().KnockBack(weapon.Knockback, transform.forward);
    //            //}
    //            return true;
    //        }
    //        else // if (!isEnemyWeaponManager)
    //        {
    //            if (m_weapon.BulletHole != null)
    //            {
    //                var newBulletHole = Instantiate(m_weapon.BulletHole, myRaycastHit.point + myRaycastHit.normal * 0.001f, Quaternion.LookRotation(myRaycastHit.normal, Vector3.up));
    //                newBulletHole.transform.parent = myRaycastHit.collider.gameObject.transform;
    //            }
    //        }
    //    }
    //    return false;
    //}

    //public bool ShootSingleRayCast()//WeaponSO weapon)
    //{
    //    if (!isSpread)
    //    {
    //        // RaycastHit myRaycastHit;
    //        myRaycastHit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, 1000, targetMask, isDebugOn);
    //        if (myRaycastHit.collider != null)
    //        {
    //            ExecuteFlightStrategies(myRaycastHit);
    //            StaticDebugger.SimpleDebugger(isDebugOn, myRaycastHit.collider.name + " was hit");
    //            if (showTrajectory)
    //            {
    //                if ((myRaycastHit.point - transform.position).normalized == transform.forward)
    //                {
    //                    var newTrajectory = Instantiate(trajectoryLineRenderer);
    //                    newTrajectory.SetPosition(0, transform.position);
    //                    newTrajectory.SetPosition(1, myRaycastHit.point);
    //                }
    //                else
    //                {
    //                    var newTrajectory = Instantiate(trajectoryLineRenderer);
    //                    newTrajectory.positionCount = 20;
    //                    float t = 0f;
    //                    var point0 = transform.position;
    //                    var point1 = transform.position + transform.forward * (myRaycastHit.point - transform.position).magnitude;
    //                    var point2 = myRaycastHit.point;
    //                    Vector3 B = new Vector3(0, 0, 0);
    //                    for (int i = 0; i < newTrajectory.positionCount; i++)
    //                    {
    //                        B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
    //                        newTrajectory.SetPosition(i, B);
    //                        t += (1 / (float)newTrajectory.positionCount);
    //                    }
    //                }
    //            }

    //            ExecuteImpactStrategies(myRaycastHit);
    //            var livingEntity = myRaycastHit.collider.GetComponent<LivingEntityContext>();
    //            // ExecuteImpactStrategies(hit.point, livingEntity);
    //            if (livingEntity != null)
    //            {
    //                // livingEntity.TakeDamage(m_weapon.Damage, projectileSpawner.transform.position);
    //                return true;
    //            }
    //            else // if (!isEnemyWeaponManager)
    //            {
    //                if (m_weapon.BulletHole != null)
    //                {
    //                    var newBulletHole = Instantiate(m_weapon.BulletHole, myRaycastHit.point + myRaycastHit.normal * 0.001f, Quaternion.LookRotation(myRaycastHit.normal, Vector3.up));
    //                    newBulletHole.transform.parent = myRaycastHit.collider.gameObject.transform;
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    //public bool ShootMultipleRayCasts()//WeaponSO weapon)
    //{
    //    bool validationBool = false;

    //    for (int i = 0; i < m_weapon.BulletsNumber; i++)
    //    {
    //        // RaycastHit myRaycastHit;
    //        var spreadDirection = transform.TransformVector(new Vector3(Random.Range(-m_weapon.Spread, m_weapon.Spread), Random.Range(-m_weapon.Spread, m_weapon.Spread), 0));
    //        myRaycastHit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward + spreadDirection, 1000, targetMask, isDebugOn);
    //        if (myRaycastHit.collider != null)
    //        {
    //            ExecuteFlightStrategies(myRaycastHit);
    //            StaticDebugger.SimpleDebugger(isDebugOn, myRaycastHit.collider.name + " was hit");
    //            if ((myRaycastHit.point - transform.position).normalized == transform.forward)
    //            {
    //                var newTrajectory = Instantiate(trajectoryLineRenderer);
    //                newTrajectory.SetPosition(0, transform.position);
    //                newTrajectory.SetPosition(1, myRaycastHit.point);
    //            }
    //            else
    //            {
    //                var newTrajectory = Instantiate(trajectoryLineRenderer);
    //                newTrajectory.positionCount = 20;
    //                float t = 0f;
    //                var point0 = transform.position;
    //                var point1 = transform.position + (transform.forward + spreadDirection) * (myRaycastHit.point - transform.position).magnitude;
    //                var point2 = myRaycastHit.point;
    //                Vector3 B = new Vector3(0, 0, 0);
    //                for (int j = 0; j < newTrajectory.positionCount; j++)
    //                {
    //                    B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
    //                    newTrajectory.SetPosition(j, B);
    //                    t += (1 / (float)newTrajectory.positionCount);
    //                }
    //            }

    //            ExecuteImpactStrategies(myRaycastHit);
    //            var livingEntity = myRaycastHit.collider.GetComponent<LivingEntityContext>();
    //            // ExecuteImpactStrategies(hit.point, livingEntity);
    //            if (livingEntity != null)
    //            {
    //                // livingEntity.TakeDamage(m_weapon.Damage / m_weapon.BulletsNumber, projectileSpawner.transform.position);
    //                validationBool = true;
    //            }
    //            else
    //            {
    //                if (m_weapon.BulletHole != null)
    //                {
    //                    var newBulletHole = Instantiate(m_weapon.BulletHole, myRaycastHit.point + myRaycastHit.normal * 0.001f, Quaternion.LookRotation(myRaycastHit.normal, Vector3.up));
    //                    newBulletHole.transform.parent = myRaycastHit.collider.gameObject.transform;
    //                }
    //            }
    //        }
    //    }
    //    return validationBool;
    //}

    //public void DetroyRaycast()
    //{
    //    Destroy(gameObject);
    //}
}
