using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicalProjectile : Projectile
{
    // [SerializeField] private float colliderActivationDelay = 0.1f;
    [SerializeField] private bool isStartAsCollider = false;
    [SerializeField] private bool affectedByGravity = true;

    [SerializeField] private float impulsion = 5f;
    [SerializeField] private Vector3 impulsionDirection;

    [SerializeField] private float projectileSpeed;

    [SerializeField] private bool hasLifespanTimer;
    [SerializeField] private float lifespanTimer;

    [SerializeField] private bool explodeOnImpact = false;

    [SerializeField] private bool hasExplosionTimer;
    [SerializeField] private float explosionTimer;

    private Rigidbody myRigidbody;
    private Collider myCollider;
    private Collision myCollision;

    public Vector3 ImpulsionDirection { get => impulsionDirection; set => impulsionDirection = value; }

    public Rigidbody MyRigidbody { get => myRigidbody; set => myRigidbody = value; }
    public Collider MyCollider { get => myCollider; }
    public bool HasLifespanTimer { get => hasLifespanTimer; set => hasLifespanTimer = value; }
    public Collision MyCollision { get => myCollision; set => myCollision = value; }

    //public override void InitializeProjectile(WeaponSO weapon, List<IProjectileStrategy> strategies, ProjectileSpawner projectileSpawner)
    //{
    //    Debug.Log("Initializing Physical Projectile");
    //    base.InitializeProjectile(weapon, strategies, projectileSpawner);

    //    myRigidbody = GetComponent<Rigidbody>();
    //    myCollider = GetComponent<Collider>();

    //    myRigidbody.useGravity = affectedByGravity;

    //    if (projectileSpeed > 0)
    //    {
    //        myRigidbody.velocity = transform.forward * projectileSpeed;
    //    }

    //    if (isStartAsCollider)
    //        myCollider.isTrigger = false;

    //    ExecuteSpawnStrategies(myCollision);
    //    foreach (IProjectileStrategy strategy in projectileStrategies)
    //    {
    //        if (strategy is PS_SpreadShot)
    //            Destroy(projectileSpawner.gameObject);
    //    }
    //}

    //public void UpdateVelocity(Vector3 forward)
    //{
    //    transform.forward = forward;
    //    myRigidbody.velocity = transform.forward * projectileSpeed;
    //}

    //public void AddImpulsion(Vector3 impulsion)
    //{
    //    if (!myRigidbody)
    //        myRigidbody = GetComponent<Rigidbody>();
    //    myRigidbody.AddForce(impulsion, ForceMode.Impulse);
    //}

    //public void AddVelocity(Vector3 velocity)
    //{
    //    if (!myRigidbody)
    //        myRigidbody = GetComponent<Rigidbody>();
    //    myRigidbody.velocity += velocity;
    //}

    // Update is called once per frame
    void Update()
    {
        // ExecuteFlightStrategies(myCollision);
        if (hasExplosionTimer)
        {
            if (explosionTimer > 0)
            {
                explosionTimer -= Time.deltaTime;
            }
            else
            {
                // Death();
            }
        }

        if (hasLifespanTimer)
        {
            if (lifespanTimer > 0)
            {
                lifespanTimer -= Time.deltaTime;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        myCollision = collision;
        // ExecuteImpactStrategies(myCollision);
        //var livingEntity = collision.gameObject.GetComponent<LivingEntityContext>();
        //if (livingEntity != null)
        //    ExecuteImpactStrategies(transform.position, livingEntity);
        //else
        //    ExecuteImpactStrategies(transform.position, null);
        onDeathEvents.Invoke();
    }

    //public override void Death()
    //{
    //    myRigidbody.velocity = Vector3.zero;
    //    gameObject.SetActive(false);
    //    // this.enabled = false;
    //    Recycle();
    //    // Destroy(gameObject);

    //    myRigidbody.constraints = RigidbodyConstraints.FreezePosition;
    //    myCollider.enabled = false;
    //}
}
