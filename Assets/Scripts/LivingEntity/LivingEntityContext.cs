using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LivingEntityContext : MonoBehaviour
{
    // SECTION - Field =========================================================
    [Header("Health")]
    [SerializeField] private bool infiniteHealth = false;
    [SerializeField] private FloatReference maxHP;
    [SerializeField] private FloatReference currentHP;
    [SerializeField] private FloatReference maxArmor;
    [SerializeField] private FloatReference currentArmor;

    [Header("Animator")]
    [Tooltip("Will keep the last frame of the OnDeath animation")]
    [SerializeField] private bool lingerAfterDeath = false;
    [Tooltip("You may need to add [AB_ManageOnDeathAnim.cs] to animation NodeState")]
    [SerializeField] private bool exitDeathDestroys = false;
    [Tooltip("You may need to add [AB_ManageOnDeathAnim.cs] to animation NodeState")]
    [SerializeField] private bool exitDeathDisablesSprite = false;
    [Space(10)]
    [SerializeField] private float onHitCueDuration = 0.14f;
                     private const string deathAnimStr = "OnDeath";
                     private Animator anim;
    //[SerializeField] private string takeDmgAnimStr;

    [Header("Events")]
    [SerializeField] private UnityEvent onStartEvents;
    [SerializeField] private UnityEvent onTakeDamageEvents;
    [SerializeField] private UnityEvent onDeathEvents;

                     private SpriteRenderer[] spriteRenderer;
                     private Rigidbody myRigidbody;

    [Header("Enemy Section")] // isEnemy should be an editor variable which hides all Enemy variables if false
    [SerializeField] private bool isEnemy = true;
    [Tooltip("Enemy deactivate upon entering a trigger zone managed by [RoomEnemyManager.cs]")]
    [SerializeField] private bool activateEnemyOnTriggerEnter = false;

    [Header("SFX")]
    [SerializeField] private AudioClip[] myHurtSFX;
    [SerializeField] private AudioClip myDeathSFX;
    [SerializeField] private AudioClip[] myAwakeSFX;

    private AudioSource myAudioSource;

    private bool isInContact = false;

    // SECTION - Property =========================================================
    public bool IsDead { get => currentHP.Value <= 0.0f; }
    public bool IsEnemy { get => isEnemy; set => isEnemy = value; }
    public bool ActivateEnemyOnTriggerEnter { get => activateEnemyOnTriggerEnter; set => activateEnemyOnTriggerEnter = value; }
    public SpriteRenderer[] SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }

    public float CurrentHP { get => currentHP.Value; }
    public float MaxHP { get => maxHP.Value; }

    public float CurrentArmor { get => currentArmor.Value; }
    public float MaxArmor { get => maxArmor.Value; }


    // SECTION - Method - Unity Specific =========================================================
    private void Start()
    {
        FullHeal();
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        myRigidbody = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
        anim = GetComponentInChildren<Animator>();
        
        if (!CompareTag("Player"))
        {
            //anim.SetBool("lingerAfterDeath", lingerAfterDeath);
        }  

        onStartEvents.Invoke();
    }


    // SECTION - Method - Context Specific =========================================================
    public void FullHeal()
    {
        currentHP.Value = maxHP.Value;
    }

    public void PlayAwakeSFX()
    {
        if (myAudioSource != null)
        {
            myAudioSource.PlayOneShot(myAwakeSFX[Random.Range(0, myAwakeSFX.Length)]);
        }
    }

    public void FullArmor()
    {
        currentArmor.Value = maxArmor.Value;
        if (onTakeDamageEvents != null)
        {
            onTakeDamageEvents.Invoke();
        }
    }

    public void KnockBack(float knockback, Vector3 direction)
    {
        if (myRigidbody)
        {
            myRigidbody.AddForce(knockback * direction, ForceMode.Impulse);
        } 
    }

    public void TakeContactDamage(float damage)
    {
        if (!isInContact)
        {
            isInContact = true;
            TakeDamage(damage, Vector3.zero);
        }
    }

    public void ExitContact()
    {
        isInContact = false;
    }

    public void TakeDamage(float damage, Vector3 position)
    {
        if (currentArmor.Value > 0.0f)
        {
            currentArmor.Value -= damage;
            if (currentArmor.Value < 0)
            {
                damage = -currentArmor.Value;
                currentArmor.Value = 0;
            }
            else
            {
                damage = 0;
            }
        }
        if (currentHP.Value > 0.0f)
        {
            if (!infiniteHealth)
            {
                currentHP.Value -= damage;
            }

            StartCoroutine(TakeDamageVisualCue());

            // On Death
            if (IsDead)
            {
                if (myAudioSource != null)
                {
                    myAudioSource.PlayOneShot(myDeathSFX);
                }
                currentHP.Value = 0;
                OnDeathBaseHandler(); // Placed here to avoid manual storing in event
                if (onTakeDamageEvents != null)
                {
                    onTakeDamageEvents.Invoke();
                }
                if (onDeathEvents != null)
                {
                    onDeathEvents.Invoke();
                }
            }
            // On Simple Damage
            else
            {
                if (myAudioSource != null)
                {
                    myAudioSource.PlayOneShot(myHurtSFX[Random.Range(0, myHurtSFX.Length)]);
                }
                OnTakeDamageBaseHandler(); // Placed here to avoid manual storing in event
                if (onTakeDamageEvents != null)
                {
                    GameManager.instance.LastDamagePosition.Position = position;
                    onTakeDamageEvents.Invoke();
                }  
            }
        }
    }

    public void InstantDeath()
    {
        TakeDamage(maxHP.Value, Vector3.zero);
    }

    public void Poison()
    {
        Debug.Log(gameObject.name + " has be poisoned");
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            if (renderer)
            {
                renderer.color = Color.green;
            }
        }
    }

    public void Freeze()
    {
        Debug.Log(gameObject.name + " has be frozen");
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            if (renderer)
            {
                renderer.color = Color.blue;
            }
        }
    }

    // SECTION - Method - Utility Specific =========================================================
    private void OnDeathBaseHandler() 
    {
        // Animator
        if (anim != null && deathAnimStr != "")
        {
            anim.SetTrigger(deathAnimStr);
        }
    }

    private void OnTakeDamageBaseHandler()
    {
        // Extend default behaviours on take damage here
    }

    private IEnumerator TakeDamageVisualCue()
    {
        // Red
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            if (renderer)
            {
                renderer.color = Color.red;
            }
        }

        yield return new WaitForSeconds(onHitCueDuration);

        // Base Color
        foreach (SpriteRenderer renderer in spriteRenderer)
        {
            if (renderer)
            {
                renderer.color = Color.white;
            }   
        }
    }

    public void AE_ManageObjectAtEndDeathAnim() // Animator Event
    {
        if (isEnemy)
        {
            RoomEnemyManager myRoomEnemyManager = GetComponentInParent<RoomEnemyManager>();

            if (myRoomEnemyManager != null)
            {
                myRoomEnemyManager.CheckLivingEntities();
            } 
        }

        if (exitDeathDisablesSprite)
        {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        else if (exitDeathDestroys)
        {
            DestroyMe();
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
