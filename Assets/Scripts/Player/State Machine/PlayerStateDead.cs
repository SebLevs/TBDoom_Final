using UnityEngine;

public class PlayerStateDead : IPlayerState
{
    // SECTION - Field ===================================================================
    private const string deathCanvasString = "Death Canvas";


    // SECTION - Method - State Specific ===================================================================
    // Movement --------------------
    public void OnLook(PlayerContext context) { }
    public void OnMove(PlayerContext context) { }
    public void OnJump(PlayerContext context) { }

    // Weapon --------------------
    public void OnFireWeaponMain(PlayerContext context) { }

    public void OnFireWeaponSecondary(PlayerContext context) { }

    public void OnWeaponChange(PlayerContext context) { }

    public void OnWeaponReload(PlayerContext context) { }

    // Misc --------------------
    public void OnInteract(PlayerContext context) { }

    public void OnShowMap(PlayerContext context) { }


    // SECTION - Method - General ===================================================================
    public void OnStateEnter(PlayerContext context) { }

    public void OnStateUpdate(PlayerContext context) { }

    public IPlayerState OnStateExit(PlayerContext context)
    {
        // Option Menu
        if (context.Input.OptionMenu)
            return new PlayerStateOptionMenu(this);

        return this;
    }
}
