using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAirborne : IPlayerState
{
    // SECTION - Method - State Specific =========================================================
    #region REGION - Movement
    public void OnLook(PlayerContext context)
    {
        context.OnDefaultLook();
    }

    public void OnMove(PlayerContext context)
    {
        // Movement
        context.OnDefaultMovement();
    }

    public void OnJump(PlayerContext context) { }
    #endregion

    #region REGION - Weapon
    public void OnFireWeaponMelee(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultFireWeaponMelee();
    }

    public void OnFireWeaponMain(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultFireWeaponMain();
    }

    public void OnFireWeaponSecondary(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultFireWeaponSecondary();
    }

    public void OnWeaponChange(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnDefaultWeaponChange();
    }

    public void OnWeaponReload(PlayerContext context)
    {
        // EVENT GO HERE
        context.OnWeaponReload();
    }
    #endregion

    #region REGION - Misc
    public void OnInteract(PlayerContext context)
    {
        // Set Interactable GUI feedback
        RaycastHit hit = context.TryRayCastInteractable();
        context.InteractCanvasHandler.SetActive(hit);

        // EVENT GO HERE
        context.OnDefaultInteract(hit);
    }

    public void OnShowMap(PlayerContext context)
    {
        context.OnDefaultShowMap();
    }
    #endregion

    // SECTION - Method - General =========================================================
    public void OnStateEnter(PlayerContext context) { context.Input.Jump = false; }

    public void OnStateUpdate(PlayerContext context)
    {
        OnLook(context);
        OnMove(context);
        //OnJump(context);

        OnFireWeaponMelee(context);
        OnFireWeaponMain(context);
        OnFireWeaponSecondary(context);
        OnWeaponChange(context);
        OnWeaponReload(context);

        OnInteract(context);
        OnShowMap(context);
    }

    public IPlayerState OnStateExit(PlayerContext context)
    {
        // Dead
        if (context.LivingEntityContext.IsDead)
            return new PlayerStateDead();

        // Option Menu
        if (context.Input.OptionMenu)
            return new PlayerStateOptionMenu(this);

        // Grounded
        if (context.TryRayCastGround().transform)
            return new PlayerStateGrounded();
          
        return this;
    }
}
