using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerState
{
    // SECTION - Method - State Specific ===================================================================
    public void OnLook(PlayerContext context);
    public void OnMove(PlayerContext context);

    public void OnFireWeaponMain(PlayerContext context);
    public void OnFireWeaponSecondary(PlayerContext context);
    public void OnWeaponChange(PlayerContext context);
    public void OnWeaponReload(PlayerContext context);

    public void OnJump(PlayerContext context);
    public void OnInteract(PlayerContext context);
    public void OnShowMap(PlayerContext context);



    // SECTION - Method - General ===================================================================
    public void OnStateEnter(PlayerContext context);
    public void OnStateUpdate(PlayerContext context);
    public IPlayerState OnStateExit(PlayerContext context);
}
