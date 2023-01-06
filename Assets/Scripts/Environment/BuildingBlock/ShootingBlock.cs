using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION { NORTH, SOUTH, EAST, WEST }

public class ShootingBlock : MonoBehaviour
{
    [SerializeField] private Transform myPivot;
    [SerializeField] private DIRECTION myDirection;
    [SerializeField] private float firingRate = 1;

    private WeaponManager myWeaponManager;

    // Start is called before the first frame update
    void Start()
    {
        switch (myDirection)
        {
            case DIRECTION.NORTH:
                myPivot.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case DIRECTION.EAST:
                myPivot.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case DIRECTION.SOUTH:
                myPivot.rotation = Quaternion.Euler(0, 180, 0);
                break;
            case DIRECTION.WEST:
                myPivot.rotation = Quaternion.Euler(0, 270, 0);
                break;
        }
        myWeaponManager = GetComponentInChildren<WeaponManager>();
        var newWeapon = Instantiate(myWeaponManager.Weapon);
        newWeapon.InfiniteClip = true;
        newWeapon.FiringRate = firingRate;
        myWeaponManager.Weapon = newWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        myWeaponManager.TriggerWeapon();
    }
}
