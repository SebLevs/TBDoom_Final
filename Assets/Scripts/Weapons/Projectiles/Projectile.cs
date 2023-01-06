using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour, IPoolableObject
{
    private WeaponSO m_weapon;
    private SO_ProjectileDefinition m_projectileDefinition;

    private List<IProjectileStrategy> m_projectileStrategies = new List<IProjectileStrategy>();
    private LayerMask m_targetMask;
    private float m_damage;

    [SerializeField] private GameObject m_physicalProjectile;
    [SerializeField] private Rigidbody m_physicalProjectileRigidbody;
    [SerializeField] private CapsuleCollider m_physicalProjectileCapsuleCollider;
    [SerializeField] private SpriteRenderer m_physicalProjectileSpriteRenderer;
    [SerializeField] private TrailRenderer m_physicalProjectileTrailRenderer;
    private LivingEntityContext m_previousLivingEntity;
    private LivingEntityContext m_livingEntity;
    private Collider m_collider;
    private float m_projectileOffset;
    private Vector3 m_pointOfImpact;
    private Vector3 m_normalOfImpact;
    private Vector3 m_impactOffset;
    private Vector3 m_physicalProjectilePreviousPosition;
    private Vector3 m_previousVelocity;
    private bool m_readyForPooling = true;

    [SerializeField] private GameObject m_raycastProjectile;
    [SerializeField] private LineRenderer m_lineRenderer;
    private RaycastHit m_raycastHit;

    [SerializeField] private GameObject m_meleeProjectile;
    //[SerializeField] private BoxCollider m_meleeProjectileBoxCollider;
    [SerializeField] private SpriteRenderer m_meleeProjectileSpriteRenderer;

    [SerializeField] protected bool isEnemyProjectile;
    [SerializeField] protected UnityEvent onDeathEvents;

    public WeaponSO Weapon { get => m_weapon; set => m_weapon = value; }
    // public float Damage { get => damage; set => damage = value; }
    public LayerMask TargetMask { get => m_targetMask; set => m_targetMask = value; }
    public List<IProjectileStrategy> ProjectileStrategies { get => m_projectileStrategies; set => m_projectileStrategies = value; }
    // public ProjectileSpawner ProjectileSpawner { get => projectileSpawner; set => projectileSpawner = value; }
    public bool IsEnemyProjectile { get => isEnemyProjectile; set => isEnemyProjectile = value; }
    public GameObject PhysicalProjectile { get => m_physicalProjectile; set => m_physicalProjectile = value; }
    public RaycastHit RaycastHit { get => m_raycastHit; set => m_raycastHit = value; }
    public SO_ProjectileDefinition ProjectileDefinition { get => m_projectileDefinition; set => m_projectileDefinition = value; }
    public IObjectPool Origin { get; set; }
    public float Damage { get => m_damage; set => m_damage = value; }
    public bool ReadyForPooling { get => m_readyForPooling; set => m_readyForPooling = value; }
    public Vector3 PointOfImpact { get => m_pointOfImpact; set => m_pointOfImpact = value; }
    public Vector3 NormalOfImpact { get => m_normalOfImpact; set => m_normalOfImpact = value; }
    public Vector3 ImpactOffset { get => m_impactOffset; set => m_impactOffset = value; }
    public LivingEntityContext LivingEntity { get => m_livingEntity; set => m_livingEntity = value; }
    public float ProjectileOffset { get => m_projectileOffset; set => m_projectileOffset = value; }
    public LivingEntityContext PreviousLivingEntity { get => m_previousLivingEntity; set => m_previousLivingEntity = value; }
    public Rigidbody Rigidbody { get => m_physicalProjectileRigidbody; set => m_physicalProjectileRigidbody = value; }

    public void PrepareToUse(Vector3 _position, Quaternion _rotation)
    {
        transform.position = _position;
        transform.rotation = _rotation;
    }

    private void Reset()
    {
        m_livingEntity = default;
        m_previousLivingEntity = default;
        m_targetMask = 0;

        m_projectileStrategies.Clear();
        m_physicalProjectileTrailRenderer.Clear();
        m_lineRenderer.positionCount = 2;
        m_lineRenderer.SetPosition(0, Vector3.zero);
        m_lineRenderer.SetPosition(1, Vector3.one);

        m_physicalProjectile.transform.localPosition = Vector3.zero;
        m_physicalProjectile.transform.localRotation = Quaternion.identity;
        m_physicalProjectileRigidbody.velocity = Vector3.zero;
        m_pointOfImpact = default;
        m_normalOfImpact = default;
        m_impactOffset = default;
        m_physicalProjectilePreviousPosition = default;
        m_previousVelocity = default;
        m_readyForPooling = true;
        m_physicalProjectile.SetActive(false);
        m_collider = default;

        m_raycastProjectile.SetActive(false);
        m_raycastHit = default;

        m_meleeProjectile.transform.localPosition = Vector3.zero;
        m_meleeProjectile.transform.localRotation = Quaternion.identity;
        m_meleeProjectile.SetActive(false);
    }

    public void ReturnToPool()
    {
        Reset();
        Origin.ReturnToPool(this);
    }

    private IEnumerator DelayedRemove()
    {
        yield return new WaitForSeconds(0.2f);
        ReturnToPool();
    }

    public void Initialize(WeaponSO _weapon, SO_ProjectileDefinition _projectileDefinition, List<IProjectileStrategy> _projectileStrategies, LayerMask _targetMask, LivingEntityContext _previousLivingEntity, IProjectileStrategy _activatingProjectileStrategy = null, Vector3 _previousVelocity = default)
    {
        m_weapon = _weapon;
        if (_activatingProjectileStrategy == null)
        {
            m_damage = _weapon.Damage;
            Debug.Log("Damage was set to " + m_damage);
        }
        else if (_activatingProjectileStrategy is PS_SpreadShotOnSpawn)
        {
            m_damage = _weapon.Damage * _activatingProjectileStrategy.DamageMultiplier / m_weapon.BulletsNumber;
            Debug.Log("Damage was set to " + m_damage);
        }
        else
        {
            m_damage = _weapon.Damage * _activatingProjectileStrategy.DamageMultiplier;
            Debug.Log("Damage was set to " + m_damage);
        }
        m_targetMask = _targetMask;
        m_projectileDefinition = _projectileDefinition;
        m_previousLivingEntity = _previousLivingEntity;
        m_previousVelocity = _previousVelocity;

        foreach (IProjectileStrategy strategy in _projectileStrategies)
        {
            var newProjectileStrategy = strategy.GetCopy();
            if (strategy is PS_ChainLightningOnImpact)
            {
                if ((strategy as PS_ChainLightningOnImpact).MainChainLightning == null)
                {
                    (newProjectileStrategy as PS_ChainLightningOnImpact).MainChainLightning = newProjectileStrategy as PS_ChainLightningOnImpact;
                }
                else
                {
                    (newProjectileStrategy as PS_ChainLightningOnImpact).MainChainLightning = (strategy as PS_ChainLightningOnImpact).MainChainLightning;
                }
            }
            m_projectileStrategies.Add(newProjectileStrategy);
        }

        switch (m_projectileDefinition.ProjectileType)
        {
            case ProjectileType.PHYSICAL:
                InitializePhysicalProjectile();
                break;
            case ProjectileType.RAYCAST:
                InitializeRaycastProjectile();
                break;
            case ProjectileType.MELEE:
                InitializeMeleeProjectile();
                break;
            default:
                Debug.Log("Projectile Type is unknown");
                break;
        }
    }

    private void InitializePhysicalProjectile()
    {
        Debug.Log("Initializing Physical Projectile");

        m_physicalProjectile.SetActive(true);
        m_raycastProjectile.SetActive(false);
        m_meleeProjectile.SetActive(false);
        gameObject.SetActive(true);

        m_physicalProjectileCapsuleCollider.radius = m_projectileDefinition.ColliderRadius;
        m_physicalProjectileCapsuleCollider.height = m_projectileDefinition.ColliderHeight;
        m_physicalProjectileSpriteRenderer.sprite = m_projectileDefinition.PhysicalProjectileSprites[0];
        m_physicalProjectileRigidbody.useGravity = m_projectileDefinition.AffectedByGravity;
        if (m_projectileDefinition.AffectedByGravity)
        {
            if (m_previousVelocity == default)
            {
                var impulsion = transform.TransformDirection(m_weapon.ImpulsionDirection.normalized) * m_weapon.ImpulsionForce;
                m_physicalProjectileRigidbody.AddForce(impulsion, ForceMode.Impulse);
            }
            else
            {
                var velocity = transform.forward * m_previousVelocity.magnitude;
                m_physicalProjectileRigidbody.velocity = velocity;
            }
        }
        else
        {
            m_physicalProjectileRigidbody.velocity = transform.forward * m_projectileDefinition.ProjectileSpeed;
        }
        m_projectileOffset = m_projectileDefinition.ColliderHeight / 2 - m_projectileDefinition.ColliderRadius;

        ExecuteSpawnStrategies(m_collider);
        foreach (IProjectileStrategy strategy in m_projectileStrategies)
        {
            if (strategy is PS_SpreadShotOnSpawn)
            {
                ReturnToPool();
                return;
            } 
        }
    }

    public void UpdateVelocity(Vector3 _targetForward)
    {
        if (m_projectileDefinition.AffectedByGravity)
        {
            var currentVelocity = m_physicalProjectileRigidbody.velocity.normalized;
            var targetVelocity = new Vector3(_targetForward.normalized.x, m_physicalProjectileRigidbody.velocity.normalized.y, _targetForward.normalized.z);
            var newVelocity = Vector3.Lerp(currentVelocity, targetVelocity, 0.05f); // TODO: Move Lerp variable into the Projectile Definition SO
            m_physicalProjectileRigidbody.velocity = newVelocity * m_physicalProjectileRigidbody.velocity.magnitude;
        }
        else
        {
            var newForward = Vector3.Lerp(m_physicalProjectile.transform.forward, _targetForward, 0.05f); // TODO: Move Lerp variable into the Projectile Definition SO
            m_physicalProjectile.transform.forward = newForward;
            m_physicalProjectileRigidbody.velocity = m_physicalProjectile.transform.forward * m_projectileDefinition.ProjectileSpeed;
        }
    }

    public void AddImpulsion(Vector3 _impulsion)
    {
        m_physicalProjectileRigidbody.AddForce(_impulsion, ForceMode.Impulse);
    }

    public void AddVelocity(Vector3 _velocity)
    {
        m_physicalProjectileRigidbody.velocity += _velocity;
    }

    private void LateUpdate()
    {
        if (m_projectileDefinition.ProjectileType == ProjectileType.PHYSICAL)
        {
            ChangeSprite();
        }
    }

    private void ChangeSprite()
    {
        var x = m_physicalProjectile.transform.position.x - PlayerContext.instance.transform.position.x;
        var z = m_physicalProjectile.transform.position.z - PlayerContext.instance.transform.position.z;
        var forward = new Vector3(x, 0, z);
        float directionAngle = Vector3.SignedAngle(m_physicalProjectile.transform.forward, forward, Vector3.up);
        
        m_physicalProjectileSpriteRenderer.sprite = m_projectileDefinition.PhysicalProjectileSprites[StaticEnemyAnimHandler.GetIndex(directionAngle)];
        Vector3 projection = Vector3.ProjectOnPlane(m_physicalProjectile.transform.forward, forward);
        m_physicalProjectileSpriteRenderer.transform.rotation = Quaternion.LookRotation(m_physicalProjectileSpriteRenderer.transform.forward, projection);
    }

    public float CalcSignedCentralAngle(Vector3 dir1, Vector3 dir2, Vector3 normal)
    {
        return Mathf.Atan2(Vector3.Dot(Vector3.Cross(dir1, dir2), normal), Vector3.Dot(dir1, dir2));
    }

    private void FixedUpdate()
    {
        StartCoroutine(StorePreviousPosition());
        if (m_projectileDefinition.ProjectileType == ProjectileType.PHYSICAL)
        {
            // m_physicalProjectile.transform.Translate(PhysicalProjectile.transform.forward * m_projectileDefinition.ProjectileSpeed * Time.deltaTime);
            ExecuteFlightStrategies(m_collider);
        }
    }

    private IEnumerator StorePreviousPosition()
    {
        yield return new WaitForFixedUpdate();
        m_physicalProjectilePreviousPosition = m_physicalProjectile.transform.position;
    }

    public void Impact(Collider _collider)
    {
        if (((1 << _collider.gameObject.layer) & m_targetMask) == 0)
        {
            return;
        }

        m_collider = _collider;
        m_livingEntity = _collider.GetComponent<LivingEntityContext>();

        if (m_previousLivingEntity != null && m_livingEntity == m_previousLivingEntity)
        {
            m_previousLivingEntity = null;
            return;
        }

        if (m_livingEntity != null)
        {
            m_livingEntity.TakeDamage(m_damage, transform.position);
        }

        var newPosition = m_physicalProjectilePreviousPosition - (m_physicalProjectile.transform.position - m_physicalProjectilePreviousPosition);
        var offsetPosition = newPosition + m_physicalProjectile.transform.forward * m_projectileOffset;
        var raycastDirection = _collider.transform.position - offsetPosition;
        var raycastHit = StaticRayCaster.IsLineCastTouching(offsetPosition, raycastDirection, 1000, m_targetMask);
        m_normalOfImpact = raycastHit.normal;
        
        raycastHit = StaticRayCaster.IsLineCastTouching(offsetPosition, -m_normalOfImpact, 1000, m_targetMask);
        m_pointOfImpact = raycastHit.point;
        m_impactOffset = m_normalOfImpact.normalized * (m_projectileDefinition.ColliderRadius + 0.01f);

        ExecuteImpactStrategies(m_collider);
        if (m_readyForPooling)
        {
            ReturnToPool();
        }
        else
        {
            m_readyForPooling = true;
        }
    }

    private void InitializeRaycastProjectile()
    {
        Debug.Log("Initializing Raycast Projectile");

        m_physicalProjectile.SetActive(false);
        m_raycastProjectile.SetActive(true);
        m_meleeProjectile.SetActive(false);
        gameObject.SetActive(true);

        m_lineRenderer.gameObject.SetActive(m_projectileDefinition.ShowRaycastTrajectory);

        ExecuteSpawnStrategies(m_raycastHit);
        foreach (IProjectileStrategy strategy in m_projectileStrategies)
        {
            if (strategy is PS_SpreadShotOnSpawn)
            {
                ReturnToPool();
                return;
            } 
        }
        
        ShootRaycast();
        StartCoroutine(DelayedRemove());
    }

    public void ShootRaycast()//WeaponSO weapon)
    {
        // RaycastHit myRaycastHit;
        m_raycastHit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, 1000, m_targetMask);
        if (m_raycastHit.collider != null)
        {
            ExecuteFlightStrategies(m_raycastHit);
            m_livingEntity = m_raycastHit.collider.GetComponent<LivingEntityContext>();
            m_pointOfImpact = m_raycastHit.point;
            m_normalOfImpact = m_raycastHit.normal;

            // StaticDebugger.SimpleDebugger(isDebugOn, m_raycastHit.collider.name + " was hit");
            if (m_projectileDefinition.ShowRaycastTrajectory)
            {
                if ((m_raycastHit.point - transform.position).normalized == transform.forward)
                {
                    m_lineRenderer.SetPosition(0, transform.position);
                    m_lineRenderer.SetPosition(1, m_raycastHit.point);
                }
                else
                {
                    m_lineRenderer.positionCount = 10;
                    float t = 0f;
                    var point0 = transform.position;
                    var point1 = transform.position + transform.forward * (m_raycastHit.point - transform.position).magnitude;
                    var point2 = m_raycastHit.point;
                    Vector3 B = new Vector3(0, 0, 0);
                    for (int i = 0; i < m_lineRenderer.positionCount; i++)
                    {
                        B = (1 - t) * (1 - t) * point0 + 2 * (1 - t) * t * point1 + t * t * point2;
                        m_lineRenderer.SetPosition(i, B);
                        t += (1 / (float)m_lineRenderer.positionCount);
                    }
                }
            }

            ExecuteImpactStrategies(m_raycastHit);
            var livingEntity = m_raycastHit.collider.GetComponent<LivingEntityContext>();
            // ExecuteImpactStrategies(hit.point, livingEntity);
            if (livingEntity != null)
            {
                livingEntity.TakeDamage(m_damage, transform.position);
            }
            else
            {
                if (m_weapon.BulletHole != null)
                {
                    var newBulletHole = Instantiate(m_weapon.BulletHole, m_raycastHit.point + m_raycastHit.normal * 0.001f, Quaternion.LookRotation(m_raycastHit.normal, Vector3.up));
                    newBulletHole.transform.parent = m_raycastHit.collider.gameObject.transform;
                }
            }
        }
    }

    private void InitializeMeleeProjectile()
    {
        Debug.Log("Initializing Melee Projectile");

        m_physicalProjectile.SetActive(false);
        m_raycastProjectile.SetActive(false);
        m_meleeProjectile.SetActive(true);
        gameObject.SetActive(true);

        var randomRotation = RandomManager.Instance.OtherRandom.Random.Next(-10, 10);
        m_meleeProjectile.transform.Rotate(new Vector3(0, 0, randomRotation));
        m_meleeProjectileSpriteRenderer.sprite = m_projectileDefinition.MeleeProjectileSprite;
        if (m_previousLivingEntity.IsEnemy)
        {
            m_meleeProjectileSpriteRenderer.sortingOrder = 0;
            m_meleeProjectileSpriteRenderer.transform.localPosition = new Vector3(0, 0, Weapon.Range * 0.5f);
        }
        else
        {
            m_meleeProjectileSpriteRenderer.sortingOrder = 1;
            m_meleeProjectileSpriteRenderer.transform.localPosition = new Vector3(0, 0, Weapon.Range);
        }

        HitMelee();
    }

    private void HitMelee()
    {
        var center = m_meleeProjectile.transform.position + m_meleeProjectile.transform.forward * Weapon.Range / 2;
        var halfExtents = new Vector3(m_projectileDefinition.MeleeProjectileWidth / 2, m_projectileDefinition.MeleeProjectileThickness / 2, Weapon.Range / 2);
        var hitEntities = Physics.OverlapBox(center, halfExtents, m_meleeProjectile.transform.rotation, m_targetMask);
        foreach (Collider hitEntity in hitEntities)
        {
            var livingEntity = hitEntity.GetComponent<LivingEntityContext>();
            if (livingEntity != null && livingEntity != m_previousLivingEntity)
            {
                m_livingEntity = livingEntity;
                livingEntity.TakeDamage(Weapon.Damage, m_meleeProjectile.transform.position);
                ExecuteImpactStrategies(hitEntity);
                // break;
            }
        }
        
        StartCoroutine(DelayedRemove());
    }

    public void ExecuteSpawnStrategies(RaycastHit _raycastHit)
    {
        var temp = m_projectileStrategies.FindAll(x => x.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.SPAWN);
        temp.ForEach(x => x.BaseExecuteStrategy(_raycastHit, this));
        //foreach (IProjectileStrategy strategy in m_projectileStrategies)
        //{
        //    if (strategy.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.SPAWN)
        //    {
        //        strategy.BaseExecuteStrategy(_raycastHit, this);
        //    }   
        //}
    }

    public void ExecuteFlightStrategies(RaycastHit _raycastHit)
    {
        var temp = m_projectileStrategies.FindAll(x => x.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.FLIGHT);
        temp.ForEach(x => x.BaseExecuteStrategy(_raycastHit, this));
        //foreach (IProjectileStrategy strategy in m_projectileStrategies)
        //{
        //    if (strategy.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.FLIGHT)
        //    {
        //        strategy.BaseExecuteStrategy(_raycastHit, this);
        //    } 
        //}
    }

    public void ExecuteImpactStrategies(RaycastHit _raycastHit)
    {
        var temp = m_projectileStrategies.FindAll(x => x.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.IMPACT);
        temp.ForEach(x => x.BaseExecuteStrategy(_raycastHit, this));
        //foreach (IProjectileStrategy strategy in m_projectileStrategies)
        //{
        //    if (strategy.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.IMPACT)
        //    {
        //        strategy.BaseExecuteStrategy(_raycastHit, this);
        //    }  
        //}
    }

    public void ExecuteSpawnStrategies(Collider _collider)
    {
        var temp = m_projectileStrategies.FindAll(x => x.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.SPAWN);
        temp.ForEach(x => x.BaseExecuteStrategy(_collider, this));
        //foreach (IProjectileStrategy strategy in m_projectileStrategies)
        //{
        //    if (strategy.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.SPAWN)
        //    {
        //        strategy.BaseExecuteStrategy(_collider, this);
        //    }  
        //}
    }

    public void ExecuteFlightStrategies(Collider _collider)
    {
        var temp = m_projectileStrategies.FindAll(x => x.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.FLIGHT);
        temp.ForEach(x => x.BaseExecuteStrategy(_collider, this));
        //foreach (IProjectileStrategy strategy in m_projectileStrategies)
        //{
        //    if (strategy.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.FLIGHT)
        //    {
        //        strategy.BaseExecuteStrategy(_collider, this);
        //    }
        //}
    }

    public void ExecuteImpactStrategies(Collider _collider)
    {
        var temp = m_projectileStrategies.FindAll(x => x.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.IMPACT);
        temp.ForEach(x => x.BaseExecuteStrategy(_collider, this));
        //foreach (IProjectileStrategy strategy in m_projectileStrategies)
        //{
        //    if (strategy.ProjectileStrategyExecutionType == ProjectileStrategyExecutionType.IMPACT)
        //    {
        //        strategy.BaseExecuteStrategy(_collider, this);
        //    }
        //}
    }

    public List<IProjectileStrategy> ExtractAllPS()
    {
        List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
        foreach (IProjectileStrategy strategy in m_projectileStrategies)
        {
            newStrategies.Add(strategy.GetCopy());
        }
        return newStrategies;
    }

    public List<IProjectileStrategy> ExtractAllPS(IProjectileStrategy _exception)
    {
        List<IProjectileStrategy> newStrategies = new List<IProjectileStrategy>();
        foreach (IProjectileStrategy strategy in m_projectileStrategies)
        {
            if (strategy.GetType() != _exception.GetType())
            {
                newStrategies.Add(strategy.GetCopy());
            }    
        }
        return newStrategies;
    }
}
