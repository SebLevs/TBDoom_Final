using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [SerializeField] private DAMAGE_TYPE damageType;
    [SerializeField] private float damage = 0;
    [SerializeField] private float delay = 0;
    [SerializeField] private float duration = 0;
    [SerializeField] private bool respawnsPlayer = false;

    [SerializeField] private STATUS_EFFECT statusEffect;
    [SerializeField] private float statusEffectActivationDelay = 0;
    [SerializeField] private float statusEffectDamage = 0;
    [SerializeField] private float statusEffectDelay = 0;
    [SerializeField] private float statusEffectDuration = 0;

    public DAMAGE_TYPE DamageType { get => damageType; set => damageType = value; }
    public float Damage { get => damage; set => damage = value; }
    public float Delay { get => delay; set => delay = value; }
    public float Duration { get => duration; set => duration = value; }
    public bool RespawnsPlayer { get => respawnsPlayer; set => respawnsPlayer = value; }

    public STATUS_EFFECT StatusEffect { get => statusEffect; set => statusEffect = value; }
    public float StatusEffectActivationDelay { get => statusEffectActivationDelay; set => statusEffectActivationDelay = value; }
    public float StatusEffectDamage { get => statusEffectDamage; set => statusEffectDamage = value; }
    public float StatusEffectDelay { get => statusEffectDelay; set => statusEffectDelay = value; }
    public float StatusEffectDuration { get => statusEffectDuration; set => statusEffectDuration = value; }
}
