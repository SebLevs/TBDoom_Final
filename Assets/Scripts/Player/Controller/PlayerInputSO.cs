using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Player/Input", fileName = "PlayerInputSO")]
public class PlayerInputSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    [Header("Movement")]
    [SerializeField] private FloatReference moveFactor;
    [SerializeField] private FloatReference jumpFactor;

    [SerializeField] private FloatReference mouseSensitivity;


    // SECTION - Property ===================================================================
    #region Movement
    public FloatReference MoveFactor { get => moveFactor; set => moveFactor = value; }
    public FloatReference JumpFactor { get => jumpFactor; set => jumpFactor = value; }
    public FloatReference MouseSensitivity { get => mouseSensitivity; set => mouseSensitivity = value; }

    public float DirX { get; set; }
    public float DirZ { get; set; }
    public float LookX { get; set; }
    public float LookY { get; set; }
    #endregion

    #region Interaction
    public bool FireMeleeWeapon { get; set; }
    public bool FireMainWeapon { get; set; }
    public bool Jump { get; set; }
    public bool Reload { get; set; }
    public bool Interact { get; set; }
    public bool FireSecondaryWeapon { get; set; }
    #endregion
 
    #region Option
    public bool OptionMenu { get; set; }
    public bool ShowMap { get; set; }
    #endregion

    #region Weapon Switching
    public bool WeaponOne { get; set; }
    public bool WeaponTwo { get; set; }
    public bool WeaponThree { get; set; }
    public bool WeaponScrollBackward { get; set; }
    public bool WeaponScrollForward { get; set; }
    #endregion

    #region Misc
    public bool AnyKey { get; set; }
    #endregion
}
