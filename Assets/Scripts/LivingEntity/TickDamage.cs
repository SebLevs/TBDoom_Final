using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickDamage : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float duration;
    [SerializeField] private bool respawnPlayer = false;
    
    [SerializeField] private STATUS_EFFECT statusEffect;
    [SerializeField] private float statusEffectActivationDelay;
    [SerializeField] private float statusEffectDamage;
    [SerializeField] private float statusEffectDelay;
    [SerializeField] private float statusEffectDuration;

    private bool causeStatusEffect = false;
    private bool toBeRemoved = false;
    private PlayerContext player;
    private bool isPlayer = false;

    private LivingEntityContext myLivingEntity;
    private TickDamageManager myTickDamageManager;

    public bool ToBeRemoved { get => toBeRemoved; set => toBeRemoved = value; }
    
    public float Damage { get => damage; set => damage = value; }
    public float Delay { get => delay; set => delay = value; }
    public float Duration { get => duration; set => duration = value; }
    public bool RespawnPlayer { get => respawnPlayer; set => respawnPlayer = value; }

    public STATUS_EFFECT StatusEffect { get => statusEffect; set => statusEffect = value; }
    public float StatusEffectActivationDelay { get => statusEffectActivationDelay; set => statusEffectActivationDelay = value; }
    public float StatusEffectDamage { get => statusEffectDamage; set => statusEffectDamage = value; }
    public float StatusEffectDelay { get => statusEffectDelay; set => statusEffectDelay = value; }
    public float StatusEffectDuration { get => statusEffectDuration; set => statusEffectDuration = value; }

    // Start is called before the first frame update
    void Start()
    {
        myLivingEntity = GetComponentInParent<LivingEntityContext>();
        myTickDamageManager = GetComponentInParent<TickDamageManager>();
        if (statusEffect != STATUS_EFFECT.NONE)
            causeStatusEffect = true;
        player = GetComponentInParent<PlayerContext>();
        if (player != null)
            isPlayer = true;
        StartCoroutine(ExecuteDamage());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        duration -= Time.fixedDeltaTime;
        if (duration < 0)
            toBeRemoved = true;
        if (causeStatusEffect)
        {
            statusEffectActivationDelay -= Time.fixedDeltaTime;
            if (statusEffectActivationDelay < 0)
            {
                DAMAGE_TYPE tempDamageType;
                Enum.TryParse(statusEffect.ToString(), out tempDamageType);
                myTickDamageManager.CreateTickDamage(statusEffectDamage, statusEffectDelay, statusEffectDuration, tempDamageType);
            }
        }
    }

    private IEnumerator ExecuteDamage()
    {
        if (toBeRemoved)
            Destroy(gameObject);
        else
        {
            myLivingEntity.TakeDamage(damage, Vector3.zero);
            if (isPlayer && respawnPlayer)
            {
                player.Respawn();
            }
        }

        yield return new WaitForSeconds(delay);

        StartCoroutine(ExecuteDamage());
    }
}
