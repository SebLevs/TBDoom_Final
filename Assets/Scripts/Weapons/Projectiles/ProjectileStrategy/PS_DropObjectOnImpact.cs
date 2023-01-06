using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Weapons/ProjectileStrategy/DropFire", fileName = "DropFireProjectileStrategySO")]
public class PS_DropObjectOnImpact : IProjectileStrategy
{
    // SECTION - Field ============================================================
    [Header("Drops")]
    [SerializeField] private GameObject[] myDrops;
    [SerializeField] private float impulseForce = 1f;
    [SerializeField] private int minDrop = 5;
    [SerializeField] private int maxDrop = 10;
    private Room myRoom;

    [Header("Behaviour Values")]
    [SerializeField] private float damage;

    public override void ExecuteColliderStrategy()
    {
        ExecuteStrategy();
        //// var livingEntity = _collider.gameObject.GetComponent<LivingEntityContext>();
        //if (_projectile.LivingEntity != null)
        //{
        //    myRoom = _projectile.LivingEntity.GetComponentInParent<Room>();
        //}

        //SpawnParticuleSystem(_projectile.PointOfImpact);

        //var limit = Random.Range(minDrop, maxDrop);
        //for (int i = 0; i < limit; i++)
        //{
        //    var newDrop = Instantiate(myDrops[Random.Range(0, myDrops.Length)], _projectile.PointOfImpact, Quaternion.identity);

        //    // Temporary solution for the spawning of object in a room.
        //    // Would need to be fixed for turret or other physical objects that sapwns from the player.
        //    if (myRoom != null)
        //    {
        //        newDrop.transform.SetParent(myRoom.transform);
        //    }
        //    else
        //    {
        //        newDrop.transform.SetParent(null);
        //    }
        //    // **********************************************************

        //    newDrop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0, 0.5f), Random.Range(-1f, 1f)).normalized * impulseForce, ForceMode.Impulse);
        //}
    }

    public override void ExecuteRaycastStrategy()
    {
        ExecuteStrategy();
        //// var livingEntity = _raycastHit.collider.GetComponent<LivingEntityContext>();
        //if (_projectile.LivingEntity != null)
        //{
        //    myRoom = _projectile.LivingEntity.GetComponentInParent<Room>();
        //} 

        //SpawnParticuleSystem(_raycastHit.point);

        //var limit = Random.Range(minDrop, maxDrop);
        //for (int i = 0; i < limit; i++)
        //{
        //    var newDrop = Instantiate(myDrops[Random.Range(0, myDrops.Length)], _raycastHit.point, Quaternion.identity);

        //    // Temporary solution for the spawning of object in a room.
        //    // Would need to be fixed for turret or other physical objects that sapwns from the player.
        //    if (myRoom != null)
        //    {
        //        newDrop.transform.SetParent(myRoom.transform);
        //    } 
        //    else
        //    {
        //        newDrop.transform.SetParent(null);
        //    }
        //    // **********************************************************

        //    newDrop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0, 0.5f), Random.Range(-1f, 1f)).normalized * impulseForce, ForceMode.Impulse);
        //}
    }

    private void ExecuteStrategy()
    {
        // var livingEntity = _raycastHit.collider.GetComponent<LivingEntityContext>();
        if (m_projectile.LivingEntity != null)
        {
            myRoom = m_projectile.LivingEntity.GetComponentInParent<Room>();
        }

        SpawnParticuleSystem(m_projectile.PointOfImpact);

        var limit = Random.Range(minDrop, maxDrop);
        for (int i = 0; i < limit; i++)
        {
            var newDrop = Instantiate(myDrops[Random.Range(0, myDrops.Length)], m_projectile.PointOfImpact, Quaternion.identity);

            // Temporary solution for the spawning of object in a room.
            // Would need to be fixed for turret or other physical objects that sapwns from the player.
            if (myRoom != null)
            {
                newDrop.transform.SetParent(myRoom.transform);
            }
            else
            {
                newDrop.transform.SetParent(null);
            }
            // **********************************************************

            newDrop.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0, 0.5f), Random.Range(-1f, 1f)).normalized * impulseForce, ForceMode.Impulse);
        }
    }
}
