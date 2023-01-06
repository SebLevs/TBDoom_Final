using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


// NOTE
//      - ADD & CHANGE methods could be refactored into single generic-ish method (enum?) to aleviate eye bleeding

[CreateAssetMenu(menuName = "Scriptable/Weapons/WeaponInventory", fileName = "WeaponsInventorySO")]
public class WeaponsInventorySO : ScriptableObject
{
    [SerializeField] private GameEvent mainWeaponHasChangedGE;
    [SerializeField] private GameEvent secondaryWeaponHasChangedGE;

    [SerializeField] private int maxMeleeWeapons = 1;
    [SerializeField] private int maxMainWeapons = 2;
    [SerializeField] private int maxSecondaryWeapons = 1;

    [SerializeField] private List<WeaponSO> defaultMeleeWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedMeleeWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedMeleeWeapon;

    [SerializeField] private List<WeaponSO> defaultMainWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedMainWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedMainWeapon;

    [SerializeField] private List<WeaponSO> defaultSecondaryWeapons = new List<WeaponSO>();
    [SerializeField] private List<WeaponSO> carriedSecondaryWeapons = new List<WeaponSO>();
    [SerializeField] private WeaponSO equippedSecondaryWeapon;

    public bool IsMeleeFull => carriedMeleeWeapons.Count == maxMeleeWeapons;
    public int MaxMeleeWeapons { get => maxMeleeWeapons; set => maxMeleeWeapons = value; }
    public bool IsMainFull => carriedMainWeapons.Count == maxMainWeapons;
    public int MaxMainWeapons { get => maxMainWeapons; set => maxMainWeapons = value; }
    public bool IsSecondaryFull => carriedSecondaryWeapons.Count == maxSecondaryWeapons;
    public int MaxSecondaryWeapons { get => maxSecondaryWeapons; set => maxSecondaryWeapons = value; }

    public List<WeaponSO> DefaultMeleeWeapons { get => defaultMeleeWeapons; set => defaultMeleeWeapons = value; }
    public List<WeaponSO> CarriedMeleeWeapons { get => carriedMeleeWeapons; set => carriedMeleeWeapons = value; }
    public WeaponSO EquippedMeleeWeapon { get => equippedMeleeWeapon; set => equippedMeleeWeapon = value; }

    public List<WeaponSO> DefaultMainWeapons { get => defaultMainWeapons; set => defaultMainWeapons = value; }
    public List<WeaponSO> CarriedMainWeapons { get => carriedMainWeapons; set => carriedMainWeapons = value; }
    public WeaponSO EquippedMainWeapon { get => equippedMainWeapon; set => equippedMainWeapon = value; }

    public List<WeaponSO> DefaultSecondaryWeapons { get => defaultSecondaryWeapons; set => defaultSecondaryWeapons = value; }
    public List<WeaponSO> CarriedSecondaryWeapons { get => carriedSecondaryWeapons; set => carriedSecondaryWeapons = value; }
    public WeaponSO EquippedSecondaryWeapon { get => equippedSecondaryWeapon; set => equippedSecondaryWeapon = value; }

    // Start is called before the first frame update
    void OnEnable()
    {
        SetDefaultWeapons();
    }

    public void SetDefaultWeapons()
    {
        CarriedMeleeWeapons.Clear();
        for (int i = 0; i < DefaultMeleeWeapons.Count; i++)
        {
            CarriedMeleeWeapons.Add(DefaultMeleeWeapons[i].GetCopy());
            // CarriedMeleeWeapons.Add(Instantiate(DefaultMeleeWeapons[i]));
        }
        EquippedMeleeWeapon = CarriedMeleeWeapons[0];

        CarriedMainWeapons.Clear();
        for (int i = 0; i < DefaultMainWeapons.Count; i++)
        {
            var newCarriedMainWeapon = DefaultMainWeapons[i].GetCopy();
            newCarriedMainWeapon.InfiniteAmmo = true;
            CarriedMainWeapons.Add(newCarriedMainWeapon);
        }
        EquippedMainWeapon = CarriedMainWeapons[0];

        CarriedSecondaryWeapons.Clear();
        for (int i = 0; i < DefaultSecondaryWeapons.Count; i++)
        {
            CarriedSecondaryWeapons.Add(DefaultSecondaryWeapons[i].GetCopy());
        }
        EquippedSecondaryWeapon = CarriedSecondaryWeapons[0];
    }

    public bool AddWeapon_Main(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        if (carriedMainWeapons.Count >= maxMainWeapons)
            return false;

        carriedMainWeapons.Add(otherWeaponSO.GetCopy());

        if (alsoEquip)
        {
            equippedMainWeapon = carriedMainWeapons[carriedMainWeapons.Count-1];

            if (mainWeaponHasChangedGE != null)
                mainWeaponHasChangedGE.Raise();
        }

        return true;
    }

    public WeaponSO ChangeWeapon_Main(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        WeaponSO returnWeapon = equippedMainWeapon.GetCopy(); // Does not copy properly???

        // Find Weapon from carried list and switcharoo with pedestal weapon before equipping it
        for (int i = 0; i < carriedMainWeapons.Count; i++)
            if (carriedMainWeapons[i].GetInstanceID() == equippedMainWeapon.GetInstanceID())
            {
                Destroy(carriedMainWeapons[i]);
                carriedMainWeapons[i] = Instantiate(otherWeaponSO);

                if (alsoEquip)
                {
                    equippedMainWeapon = carriedMainWeapons[i];

                    if (mainWeaponHasChangedGE != null)
                        mainWeaponHasChangedGE.Raise();
                }

                break;
            }

        return returnWeapon;
    }


    public bool AddWeapon_Secondary(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        if (carriedSecondaryWeapons.Count >= maxSecondaryWeapons)
            return false;

        carriedSecondaryWeapons.Add(otherWeaponSO.GetCopy());

        if (alsoEquip)
        {
            equippedSecondaryWeapon = carriedSecondaryWeapons[carriedSecondaryWeapons.Count - 1];

            if (secondaryWeaponHasChangedGE != null)
                secondaryWeaponHasChangedGE.Raise();
        }

        return true;
    }

    public WeaponSO ChangeWeapon_Secondary(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        WeaponSO returnWeapon = equippedSecondaryWeapon.GetCopy(); // Does not copy properly???

        // Find Weapon from carried list and switcharoo with pedestal weapon before equipping it
        for (int i = 0; i < carriedSecondaryWeapons.Count; i++)
            if (carriedSecondaryWeapons[i].GetInstanceID() == equippedSecondaryWeapon.GetInstanceID())
            {
                Destroy(carriedSecondaryWeapons[i]);
                carriedSecondaryWeapons[i] = Instantiate(otherWeaponSO);

                if (alsoEquip)
                {
                    equippedSecondaryWeapon = carriedSecondaryWeapons[i];

                    if (secondaryWeaponHasChangedGE != null)
                        secondaryWeaponHasChangedGE.Raise();
                }
                    
                break;
            }

        return returnWeapon;
    }


    public bool AddWeapon_Melee(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        if (carriedMeleeWeapons.Count >= maxMeleeWeapons)
            return false;

        carriedMeleeWeapons.Add(Instantiate(otherWeaponSO));

        if (alsoEquip)
            equippedMeleeWeapon = carriedMeleeWeapons[carriedMeleeWeapons.Count-1];

        return true;
    }

    public WeaponSO ChangeWeapon_Melee(WeaponSO otherWeaponSO, bool alsoEquip = true)
    {
        WeaponSO returnWeapon = equippedMeleeWeapon.GetCopy(); // Does not copy properly???

        // Find Weapon from carried list and switcharoo with pedestal weapon before equipping it
        for (int i = 0; i < carriedMeleeWeapons.Count; i++)
            if (carriedMeleeWeapons[i].GetInstanceID() == equippedMeleeWeapon.GetInstanceID())
            {
                Destroy(carriedMeleeWeapons[i]);
                carriedMeleeWeapons[i] = Instantiate(otherWeaponSO);

                if (alsoEquip)
                    equippedMeleeWeapon = carriedMeleeWeapons[i];

                break;
            }

        return returnWeapon;
    }
}
