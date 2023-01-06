using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private bool isDebugOn = false;

    [Header("Enemy Weapon ShootingRangeHandler Section")]
    [SerializeField] private bool isEnemyWeaponManager = true; 
    [SerializeField] private bool tracksPlayer = true;

    [Space(10)]
    [SerializeField] private TransformSO playerTransform;

    [SerializeField] private LayerMask m_targetMask;

    [SerializeField] private WeaponSO weapon;
    [SerializeField] private ProjectileSpawner projectileSpawner;
    // [SerializeField] private WeaponSO secondaryWeapon;
    [SerializeField] private WeaponsInventorySO weaponsInventory;

    [SerializeField] private UnityEvent weaponHasChanged;
    [SerializeField] private UnityEvent weaponFinishedReloading;
    [SerializeField] private UnityEvent weaponHasShot;
    [SerializeField] private UnityEvent weaponStartedReloading;
    // [SerializeField] private UnityEvent secondaryWeaponHasShot;

    private LivingEntityContext myLivingEntity;

    private float fireRateDelay;
    private float reloadDelay;
    // private float secondaryFireRateDelay;

    private bool weaponIsReloading = false;
    private AudioSource weaponAudioSource;

    public WeaponSO Weapon { get => weapon; set => weapon = value; }
    // public WeaponSO SecondaryWeapon { get => secondaryWeapon; set => secondaryWeapon = value; }
    public bool WeaponIsReloading { get => weaponIsReloading; }
    public LayerMask MyTargetMask { get => m_targetMask; }
    public bool TracksPlayer { get => tracksPlayer; set => tracksPlayer = value;  }
    public UnityEvent WeaponHasChanged { get => weaponHasChanged; }
    public UnityEvent WeaponFinishedReloading { get => weaponFinishedReloading; }
    public UnityEvent WeaponHasShot { get => weaponHasShot; }
    public UnityEvent WeaponStartedReloading { get => weaponStartedReloading; }

    // public float SecondaryFireRateDelay { get => secondaryFireRateDelay; set => secondaryFireRateDelay = value; }

    private void Awake()
    {
        weaponAudioSource = GetComponent<AudioSource>();
        myLivingEntity = GetComponentInParent<LivingEntityContext>();
    }

    private void Update()
    {
        fireRateDelay -= Time.deltaTime;
        // secondaryFireRateDelay -= Time.deltaTime;

        // Reload for enemy before they try to shoot | Prevents launching animation when they can't attack
        if (isEnemyWeaponManager && !WeaponIsReloading && weapon.CurrentClip == 0)
        {
            ReloadWeapon();
            weaponIsReloading = true;
        }

        if (reloadDelay > 0)
        {
            reloadDelay -= Time.deltaTime;
            weaponIsReloading = true;
        }
        else if (weaponIsReloading)
        {
            weapon.Reload();
            weaponFinishedReloading.Invoke();
            weaponIsReloading = false;
        }
        
        if (tracksPlayer)
        {
            transform.forward = playerTransform.Transform.position - transform.position;
        }
    }
 

    public void UpdateWeapon() // WeaponSO weapon
    {
        this.weapon = weaponsInventory.EquippedMainWeapon; //weapon;
        //weaponHasChanged.Invoke();
    }

    public void UpdateSecondaryWeapon() // WeaponSO weapon
    {
        this.weapon = weaponsInventory.EquippedSecondaryWeapon; //weapon;
        //weaponHasChanged.Invoke();
    }

    public void UpdateMeleeWeapon() // WeaponSO weapon
    {
        this.weapon = weaponsInventory.EquippedMeleeWeapon; //weapon;
        //weaponHasChanged.Invoke();
    }

    public void ResetReload()
    {
        reloadDelay = 0;
        weaponIsReloading = false;
    }

    public bool ChargeWeapon()
    {
        return weapon.ChargeCheck();
    }

    public bool TriggerWeapon()
    {
        bool validationBool = false;
        if (fireRateDelay <= 0 && reloadDelay <= 0)
        {
            if (weapon.ShootCheck())
            {
                if (weaponAudioSource != null)
                {
                    weaponAudioSource.PlayOneShot(weapon.ShootingSound[Random.Range(0, weapon.ShootingSound.Length)]);
                }

                StaticDebugger.SimpleDebugger(isDebugOn, $" {weapon.WeaponName} ... FIRED");

                fireRateDelay = weapon.FiringRate;
                validationBool = ShootWeapon(weapon);
                weaponHasShot.Invoke();
                return true;
            }
            else if (!weapon.CanFireContinuously || weapon.CurrentClip == 0)
            {
                if (weaponAudioSource != null && CompareTag("Player"))
                {
                    weaponAudioSource.PlayOneShot(weapon.EmptyClickSound);
                }
                ReloadWeapon();
            }
        }
        else if (fireRateDelay <= 0 && reloadDelay > 0)
        {
            if (weaponAudioSource != null && CompareTag("Player"))
            {
                weaponAudioSource.PlayOneShot(weapon.EmptyClickSound);
            }
            fireRateDelay = weapon.FiringRate;
        }         
        return false;
    }

    public void ReloadWeapon()
    {
        if (!weaponIsReloading && weapon.CurrentClip < weapon.MaxClip)
        {
            if (weapon.ReloadCheck())
            {
                if (weaponAudioSource != null && CompareTag("Player"))
                {
                    weaponAudioSource.PlayOneShot(weapon.ReloadSentenceSound[Random.Range(0, weapon.ReloadSentenceSound.Length)]);
                }

                StaticDebugger.SimpleDebugger(isDebugOn, $" {weapon.WeaponName} ... RELOADED");

                weaponStartedReloading.Invoke();
                reloadDelay = weapon.ReloadTime;

                weaponIsReloading = true;
            }
        }
    }

    public void PlayReloadSound()
    {
        if (weaponAudioSource != null && CompareTag("Player"))
        {
            weaponAudioSource.PlayOneShot(weapon.ReloadSound);
        }
    }

    private bool ShootWeapon(WeaponSO weapon)
    {
        if (weapon.ProjectileDefinition != null)
        {
            ShootProjectile(weapon);

            return true;
        }
        return false;
    }

    public void ShootProjectile(WeaponSO _weapon)
    {
        var newProjectile = ObjectPoolManager.instance.ProjectilePool.GetPrefabInstance(transform.position, transform.rotation);
        newProjectile.Initialize(_weapon, _weapon.ProjectileDefinition, _weapon.ProjectileStrategies, m_targetMask, myLivingEntity);
    }

    public bool IsTargetInFront()
    {
        RaycastHit hit;
        hit = StaticRayCaster.IsLineCastTouching(transform.position, transform.forward, Weapon.Range, m_targetMask, isDebugOn);

        //Debug.Log($"Is raycast hit null: {hit.transform == null}");

        return hit.transform != null;
    }

    public bool IsTargetAround()
    {
        Collider[] hit;
        hit = StaticRayCaster.IsOverlapSphereTouching(transform.position, Weapon.Range, m_targetMask, isDebugOn);

        Debug.Log($"Is overlapsphere hit null: {hit == null}");

        return hit != null;
        //return !(hit == null && hit[0].transform != null);
    }
}
