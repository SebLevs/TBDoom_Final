using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFactory : MonoBehaviour
{
    public static ItemFactory instance;

    [SerializeField] private PickableSO[] pickables;
    [SerializeField] private WeaponSO[] weapons;
    [SerializeField] private IProjectileStrategy[] projectileStrategies;
    [SerializeField] private IProjectileStrategy[] meleeProjectileStrategies;

    private RandomSeed rng;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void Initialize()
    {
        rng = RandomManager.Instance.OtherRandom;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ItemSO CreateRandomItem()
    {
        ItemSO newItem;
        if (rng.Random.Next(0, 100) < 25)
        {
            newItem = CreateRandomPickable();
        }
        else
        {
            newItem = CreateRandomWeapon();
        }
        return newItem;
    }    

    public PickableSO CreateRandomPickable()
    {
        Debug.Log("Generating new Random Pickable");
        var newPickable = pickables[rng.Random.Next(0, pickables.Length)].GetCopy();

        return newPickable;
    }

    public WeaponSO CreateRandomWeapon()
    {
        Debug.Log("Generating new Random Weapon");
        var newWeapon = weapons[rng.Random.Next(0, weapons.Length)].GetCopy();
        
        // 0 = Normal = 100%, 1 = Unusual = 25%, 2 = Rare = 10%, 3 = Legendary = 5%
        var quality = 0;
        if (rng.Random.Next(0, 100) < 25)
        {
            quality++;
            if (rng.Random.Next(0, 100) < 10)
            {
                quality++;
                if (rng.Random.Next(0, 100) < 5)
                {
                    quality++;
                }
            }
        }
            
        for (int i = newWeapon.ProjectileStrategies.Count; i < quality; i++)
        {
            //var newCheck = false;
            IProjectileStrategy newStrategy = null;
            do
            {
                switch (newWeapon.ProjectileDefinition.ProjectileType)
                {
                    case ProjectileType.MELEE:
                        newStrategy = Instantiate(meleeProjectileStrategies[rng.Random.Next(0, meleeProjectileStrategies.Length)]);
                        break;
                    default:
                        newStrategy = Instantiate(projectileStrategies[rng.Random.Next(0, projectileStrategies.Length)]);
                        break;
                }
            }
            while (newWeapon.ProjectileStrategies.Exists(i => i.GetType() == newStrategy.GetType()));
            //do
            //{
            //    newCheck = false;
            //    newStrategy = Instantiate(projectileStrategies[rng.Random.Next(0, projectileStrategies.Length)]);
            //    foreach (IProjectileStrategy strategy in newWeapon.ProjectileStrategies)
            //    {
            //        if (strategy.GetType() == newStrategy.GetType())
            //        {
            //            newCheck = true;
            //            break;
            //        }
            //    }
            //}
            //while (newCheck);
            newWeapon.ProjectileStrategies.Add(newStrategy);
            Debug.Log("Adding " + newStrategy.name + " to weapon");
        }
        return newWeapon;
    }

    public WeaponSO CreateRandomLegendaryWeapon()
    {
        Debug.Log("Generating new Random Legendary Weapon");
        var newWeapon = weapons[rng.Random.Next(0, weapons.Length)].GetCopy();

        // 0 = Normal = 100%, 1 = Unusual = 25%, 2 = Rare = 10%, 3 = Legendary = 5%
        var quality = 3;

        for (int i = newWeapon.ProjectileStrategies.Count; i < quality; i++)
        {
            //var newCheck = false;
            IProjectileStrategy newStrategy = null;
            do
            {
                switch (newWeapon.ProjectileDefinition.ProjectileType)
                {
                    case ProjectileType.MELEE:
                        newStrategy = Instantiate(meleeProjectileStrategies[rng.Random.Next(0, meleeProjectileStrategies.Length)]);
                        break;
                    default:
                        newStrategy = Instantiate(projectileStrategies[rng.Random.Next(0, projectileStrategies.Length)]);
                        break;
                }
            }
            while (newWeapon.ProjectileStrategies.Exists(i => i.GetType() == newStrategy.GetType()));
            Debug.Log("Adding " + newStrategy.name + " to weapon");
            newWeapon.ProjectileStrategies.Add(newStrategy);
        }

        return newWeapon;
    }
}
