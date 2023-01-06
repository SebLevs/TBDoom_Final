using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { MAIN, SECONDARY, MELEE }

[CreateAssetMenu(menuName = "Scriptable/Weapons/Weapon", fileName = "WeaponSO")]
public class WeaponSO : ItemSO
{
    // SECTION - Field ===================================================================
    [Header("Information")]
    [SerializeField] private string weaponName;
    [SerializeField] private string weaponDescription;
    [SerializeField] private bool infiniteAmmo = false;
    [SerializeField] private bool infiniteClip = false;

    [Header("Weapon EnemyType")]
    [SerializeField] private WeaponType m_weaponType;
    [SerializeField] private bool isMain = false;
    [SerializeField] private bool isSecondary = false;
    [SerializeField] private bool isMelee = false;

    [SerializeField] private SO_ProjectileDefinition m_projectileDefinition;

    [Header("Value")]
    [SerializeField] private int currencyValue;

    [Header("Statistics")]
    [SerializeField] private bool canFireContinuously;
    [SerializeField] private bool canBeCharged;
    [SerializeField] private int startingAmmo;
    [SerializeField] private int currentAmmo;
    [SerializeField] private int maxAmmo;
    [SerializeField] private int startingClip;
    [SerializeField] private int currentClip;
    [SerializeField] private int maxClip;
    [SerializeField] private float firingRate;
    [SerializeField] private float reloadTime;
    [SerializeField] private Vector3 impulsionDirection;
    [SerializeField] private float impulsionForce;
    [SerializeField] private float damage;
    [SerializeField] private float knockback;
    [SerializeField] private float spread;
    [SerializeField] private float range;
    [SerializeField] private int bulletsNumber;
    // [SerializeField] private Projectile projectile;
    [SerializeField] private float chargeTime;
    [SerializeField] private bool needsFullCharge;
    [SerializeField] private IShootingStrategy shootingStrategy;
    [SerializeField] private List<IProjectileStrategy> projectileStrategies = new List<IProjectileStrategy>();

    private float currentChargeTime;

    [Header("Visual")]
    [SerializeField] private BulletHole bulletHole;
    [SerializeField] private Sprite weaponUISprite;
    [SerializeField] private Sprite weaponPlayerSprite;
    [SerializeField] private Sprite weaponFiringPlayerSprite;
    [SerializeField] private Animator animator;

    [Header("Sound")]
    [SerializeField] private AudioClip[] shootingSound;
    [SerializeField] private AudioClip[] reloadSentenceSound;
    [SerializeField] private AudioClip reloadSound;
    [SerializeField] private AudioClip emptyClickSound;

    // SECTION - Property ===================================================================
    public string WeaponName { get => weaponName; set => weaponName = value; }
    public string WeaponDescription { get => weaponDescription; set => weaponDescription = value; }
    public bool IsMain { get => isMain; set => isMain = value; }
    public bool IsSecondary { get => isSecondary; set => isSecondary = value; }
    public bool IsMelee { get => isMelee; set => isMelee = value; }
    public bool InfiniteAmmo { get => infiniteAmmo; set => infiniteAmmo = value; }
    public bool InfiniteClip { get => infiniteClip; set => infiniteClip = value; }

    public int CurrencyValue { get => currencyValue; set => currencyValue = value; }

    public bool CanFireContinuously { get => canFireContinuously; set => canFireContinuously = value; }
    public bool CanBeCharged { get => canBeCharged; set => canBeCharged = value; }
    public int CurrentAmmo { get => currentAmmo; set => currentAmmo = value; }
    public int MaxAmmo { get => maxAmmo; set => maxAmmo = value; }
    public int CurrentClip { get => currentClip; set => currentClip = value; }
    public int MaxClip { get => maxClip; set => maxClip = value; }
    public float FiringRate { get => firingRate; set => firingRate = value; }
    public float ReloadTime { get => reloadTime; set => reloadTime = value; }
    public Vector3 ImpulsionDirection { get => impulsionDirection; set => impulsionDirection = value; }
    public float ImpulsionForce { get => impulsionForce; set => impulsionForce = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Knockback { get => knockback; set => knockback = value; }
    public float Spread { get => spread; set => spread = value; }
    public float Range { get => range; set => range = value; }
    public int BulletsNumber { get => bulletsNumber; set => bulletsNumber = value; }

    // public Projectile Projectile { get => projectile; set => projectile = value; }
    public BulletHole BulletHole { get => bulletHole; set => bulletHole = value; }
    public Sprite WeaponUISprite { get => weaponUISprite; set => weaponUISprite = value; }
    public Sprite WeaponPlayerSprite { get => weaponPlayerSprite; set => weaponPlayerSprite = value; }
    public Sprite WeaponFiringPlayerSprite { get => weaponFiringPlayerSprite; set => weaponFiringPlayerSprite = value; }
    public Animator Animator { get => animator; set => animator = value; }
    
    public AudioClip[] ShootingSound { get => shootingSound; set => shootingSound = value; }
    public AudioClip[] ReloadSentenceSound { get => reloadSentenceSound; set => reloadSentenceSound = value; }
    public AudioClip ReloadSound { get => reloadSound; set => reloadSound = value; }
    public AudioClip EmptyClickSound { get => emptyClickSound; set => emptyClickSound = value; }
    
    public float ChargeTime { get => chargeTime; set => chargeTime = value; }
    public bool NeedsFullCharge { get => needsFullCharge; set => needsFullCharge = value; }
    public float CurrentChargeTime { get => currentChargeTime; set => currentChargeTime = value; }
    public IShootingStrategy ShootingStrategy { get => shootingStrategy; set => shootingStrategy = value; }
    public List<IProjectileStrategy> ProjectileStrategies { get => projectileStrategies; set => projectileStrategies = value; }
    public SO_ProjectileDefinition ProjectileDefinition { get => m_projectileDefinition; set => m_projectileDefinition = value; }
    public WeaponType WeaponType { get => m_weaponType; set => m_weaponType = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        currentAmmo = startingAmmo;
        currentClip = startingClip;
        currentChargeTime = 0;

    }

    public bool ChargeCheck()
    {
        currentChargeTime += Time.deltaTime;
        if (currentChargeTime < chargeTime && needsFullCharge)
            return false;
        return true;
    }

    public bool ShootCheck()
    {
        if (currentClip > 0)
        {
            if (!isMelee && !infiniteClip)
            {
                currentClip--;
            }
            return true;
        }
        return false;
    }

    public bool ReloadCheck()
    {
        if (currentAmmo > 0 && !isMelee)
        {
            return true;
        }
        return false;
    }

    public void Reload()
    {
        if (currentAmmo >= maxClip - currentClip)
        {
            if (!infiniteAmmo)
            {
                currentAmmo -= (maxClip - currentClip);
            }
            currentClip = maxClip;
        }
        else
        {
            currentClip += currentAmmo;
            currentAmmo = 0;
        }
    }

    public void RefillAmmoAndClip()
    {
        currentAmmo = maxAmmo;
        currentClip = MaxClip;
    }

    public WeaponSO GetCopy() // WeaponSO copyFrom)
    {
        WeaponSO copy = Instantiate(this);

        copy.name = weaponName + " (clone)";
        copy.canFireContinuously = canFireContinuously;
        copy.CanBeCharged = canBeCharged;
        copy.startingAmmo = startingAmmo;
        copy.currentAmmo = currentAmmo;
        copy.maxAmmo = maxAmmo;
        copy.startingClip = startingClip;
        copy.currentClip = currentClip;
        copy.maxClip = maxClip;
        copy.firingRate = firingRate;
        copy.reloadTime = reloadTime;
        copy.damage = damage;
        copy.knockback = knockback;
        copy.spread = spread;
        copy.range = range;
        copy.bulletsNumber = bulletsNumber;
        // copy.projectile = projectile;
        List<IProjectileStrategy> tempStrategies = new List<IProjectileStrategy>();
        foreach (IProjectileStrategy strategy in projectileStrategies)
        {
            tempStrategies.Add(Instantiate(strategy));
        }
        copy.projectileStrategies = tempStrategies;

        return copy;
    }
}
