using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DAMAGE_TYPE { FIRE, SPIKE, LAVA, BURNING, POISON, BLEED }
public enum STATUS_EFFECT { NONE, BURNING, POISON, BLEED }

public class TickDamageManager : MonoBehaviour
{
    [SerializeField] private TickDamage tickDamagePrefab;

    private GameObject tickDamageParent;

    private void Start()
    {
        tickDamageParent = new GameObject("TickDamageParent");
        tickDamageParent.transform.SetParent(transform);
    }

    public void CreateTickDamage(float damage, DAMAGE_TYPE damageType)
    {
        var tickDamageCheck = CheckIfTickExists(damageType.ToString());
        if (tickDamageCheck == null)
        {
            var newTickDamage = Instantiate(tickDamagePrefab);
            newTickDamage.transform.SetParent(tickDamageParent.transform);
            newTickDamage.name = damageType.ToString();
            newTickDamage.Damage = damage;
            newTickDamage.RespawnPlayer = true;
            newTickDamage.StatusEffect = STATUS_EFFECT.NONE;
        }
    }

    public void CreateTickDamage(float damage, float delay, float duration, DAMAGE_TYPE damageType)
    {
        var tickDamageCheck = CheckIfTickExists(damageType.ToString());
        if (tickDamageCheck == null)
        {
            var newTickDamage = Instantiate(tickDamagePrefab);
            newTickDamage.transform.SetParent(tickDamageParent.transform);
            newTickDamage.name = damageType.ToString();
            newTickDamage.Damage = damage;
            newTickDamage.Delay = delay;
            newTickDamage.Duration = duration;
            newTickDamage.StatusEffect = STATUS_EFFECT.NONE;
        }
        else
        {
            tickDamageCheck.Duration = duration;
        }
    }

    public void CreateTickDamage(float damage, float delay, float duration, DAMAGE_TYPE damageType, STATUS_EFFECT statusEffect, float statusEffectActivationDelay, float statusEffectDamage, float statusEffectDelay, float statusEffectDuration)
    {
        var tickDamageCheck = CheckIfTickExists(damageType.ToString());
        if (tickDamageCheck == null)
        {
            var newTickDamage = Instantiate(tickDamagePrefab);
            newTickDamage.transform.SetParent(tickDamageParent.transform);
            newTickDamage.name = damageType.ToString();
            newTickDamage.Damage = damage;
            newTickDamage.Delay = delay;
            newTickDamage.Duration = duration;
            newTickDamage.StatusEffect = statusEffect;
            newTickDamage.StatusEffectActivationDelay = statusEffectActivationDelay;
            newTickDamage.StatusEffectDamage = statusEffectDamage;
            newTickDamage.StatusEffectDelay = statusEffectDelay;
            newTickDamage.StatusEffectDuration = statusEffectDuration;
        }
        else
        {
            tickDamageCheck.Duration = duration;
        }
    }

    private TickDamage CheckIfTickExists(string name)
    {
        var tickDamages = tickDamageParent.GetComponentsInChildren<TickDamage>();
        foreach (TickDamage tickDamage in tickDamages)
        {
            if (tickDamage.name == name)
                return tickDamage;
        }
        return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        var damageOnContact = other.GetComponent<DamageOnContact>();
        if (damageOnContact != null)
        {
            var tickDamageCheck = CheckIfTickExists(damageOnContact.DamageType.ToString());
            if (tickDamageCheck == null)
            {
                var player = GetComponentInParent<PlayerContext>();
                if (player != null && damageOnContact.RespawnsPlayer)
                {
                    CreateTickDamage(damageOnContact.Damage, damageOnContact.DamageType);
                    // player.GetComponent<LivingEntityContext>().TakeDamage(damageOnContact.Damage);
                    // player.Respawn();
                }
                else
                {
                    if (damageOnContact.StatusEffect == STATUS_EFFECT.NONE)
                        CreateTickDamage(damageOnContact.Damage, damageOnContact.Delay, damageOnContact.Duration, damageOnContact.DamageType);
                    else
                        CreateTickDamage(damageOnContact.Damage, damageOnContact.Delay, damageOnContact.Duration, damageOnContact.DamageType, damageOnContact.StatusEffect, damageOnContact.StatusEffectActivationDelay, damageOnContact.StatusEffectDamage, damageOnContact.StatusEffectDelay, damageOnContact.StatusEffectDuration);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var damageOnContact = other.GetComponent<DamageOnContact>();
        if (damageOnContact != null)
        {
            var tickDamageCheck = CheckIfTickExists(damageOnContact.DamageType.ToString());
            if (tickDamageCheck != null)
            {
                tickDamageCheck.ToBeRemoved = false;
            }
        }
    }
}
