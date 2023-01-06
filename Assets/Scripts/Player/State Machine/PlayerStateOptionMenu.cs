using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateOptionMenu : IPlayerState
{
    // SECTION - Constructor =========================================================
    public PlayerStateOptionMenu(IPlayerState oldState)
    {
        this.oldState = oldState;
    }
    
    // SECTION - Field =========================================================
    public IPlayerState oldState;


    // SECTION - Method - State Specific =========================================================
    #region REGION - Movement
    public void OnLook(PlayerContext context) { }

    public void OnMove(PlayerContext context) { }

    public void OnJump(PlayerContext context) { }
    #endregion

    #region REGION - Weapon
    public void OnFireWeaponMelee(PlayerContext context) { }

    public void OnFireWeaponMain(PlayerContext context) { }

    public void OnFireWeaponSecondary(PlayerContext context) { }

    public void OnWeaponChange(PlayerContext context) { }

    public void OnWeaponReload(PlayerContext context) { }
    #endregion

    #region REGION - Misc
    public void OnInteract(PlayerContext context) { }

    public void OnShowMap(PlayerContext context) { }
    #endregion

    // SECTION - Method - General =========================================================
    public void OnStateEnter(PlayerContext context) 
    {
        context.Input.OptionMenu = false;
        GameManager.instance.SetMouseCursor_Manual(CursorLockMode.None, true);
        GameManager.instance.SetTimeScale(0.0f);
        GameManager.instance.ShowMenu();

        GameManager.instance.ToggleSeed();
    }

    public void OnStateUpdate(PlayerContext context)
    {
        /*
        OnLook(context);
        OnMove(context);
        OnJump(context);

        OnFireWeaponMelee(context);
        OnFireWeaponMain(context);
        OnFireWeaponSecondary(context);
        OnWeaponChange(context);
        OnWeaponReload(context);

        OnInteract(context);
        OnShowMap(context);
        */
    }

    public IPlayerState OnStateExit(PlayerContext context)
    {
        // Option Menu
        if (context.Input.OptionMenu)
        {
            context.Input.OptionMenu = false;
            GameManager.instance.SetMouseCursor_Manual(CursorLockMode.Locked, false);
            GameManager.instance.SetTimeScale(1.0f);
            GameManager.instance.QuitMenu();
            GameManager.instance.ToggleSeed();
            return oldState;
        }

        return this;
    }
}
