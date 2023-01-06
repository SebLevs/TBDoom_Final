using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class PlayerContext : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static PlayerContext instance = null;

    private IPlayerState currState;
    private IPlayerState oldState;

    [SerializeField] private CinemachineVirtualCamera m_playerCamera;

    [Header("Living Entity")]
    [SerializeField] private LivingEntityContext livingEntityContext;

    [Header("Raycast")]
    [SerializeField] private float distanceGround = 0.55f;
    [SerializeField] private float distanceInteractible = 0.75f;
    private bool isDebugOn = false;

    [Header("Weapons")]
    // CURRENT WEAPON & MORE GO HERE
    [SerializeField] private WeaponsInventorySO weapons;
    [SerializeField] private WeaponManager meleeWeapon;
    [SerializeField] private WeaponManager mainWeapon;
    [SerializeField] private WeaponManager secondaryWeapon;
    [SerializeField] private TransformSO playerTransform;
    [SerializeField] private PositionRotationSO lastSpawnPositionRotation;

    [Header("Rigidbody & Colliders")]
    [SerializeField] private Rigidbody rb;

    [Header("Scriptables")]
    [SerializeField] private PlayerInputSO input;

    [Header("Animator")]
    [SerializeField] private Animator anim;

    [Header("Canvases")]
    [SerializeField] private InteractCanvasHandler interactCanvasHandler;

    [Header("Events")]
    [SerializeField] private UnityEvent mainWeaponHasChanged;

    [SerializeField] private AIPossiblePositionsSO myBackSO;
    [SerializeField] private AIPossiblePositionsSO myCloseSO;
    [SerializeField] private AIPossiblePositionsSO myMidSO;


    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    public LivingEntityContext LivingEntityContext { get => livingEntityContext; set => livingEntityContext = value; }

    public float DistanceGround { get => distanceGround; }
    public float DistanceInteractible { get => distanceInteractible; }
    public bool IsDebugOn { get => isDebugOn; set => isDebugOn = value; }

    public WeaponsInventorySO Weapons { get => weapons; set => weapons = value; }
    public WeaponManager MeleeWeapon { get => meleeWeapon; set => meleeWeapon = value; }
    public WeaponManager MainWeapon { get => mainWeapon; set => mainWeapon = value; }
    public WeaponManager SecondaryWeapon { get => secondaryWeapon; set => secondaryWeapon = value; }
    public TransformSO PlayerTransform { get => playerTransform; set => playerTransform = value; }
    public PositionRotationSO LastSpawnPositionRotation { get => lastSpawnPositionRotation; set => lastSpawnPositionRotation = value; }

    public Rigidbody Rb { get => rb; set => rb = value; }

    public PlayerInputSO Input { get => input; set => input = value; }

    public Animator Anim { get => anim; set => anim = value; }
    
    public UnityEvent MainWeaponHasChanged { get => mainWeaponHasChanged; set => mainWeaponHasChanged = value; }

    public InteractCanvasHandler InteractCanvasHandler { get => interactCanvasHandler; set => interactCanvasHandler = value; }
    public CinemachineVirtualCamera Camera { get => m_playerCamera; set => m_playerCamera = value; }
    public IPlayerState CurrState { get => currState; set => currState = value; }
    #endregion

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }
        
        DontDestroyOnLoad(gameObject);

        GetComponentInChildren<Camera>().backgroundColor = new Color(50, 40, 35);
    }

    // SECTION - Method - Unity ===================================================================
    private void Start()
    {
        currState = new PlayerStateGrounded();
        oldState = currState;

        weapons.SetDefaultWeapons();

        meleeWeapon.Weapon = weapons.EquippedMeleeWeapon;
        mainWeapon.Weapon = weapons.EquippedMainWeapon;
        secondaryWeapon.Weapon = weapons.EquippedSecondaryWeapon;
    }

    public void Respawn()
    {
        transform.position = lastSpawnPositionRotation.Position;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        if (oldState != currState)
        {
            oldState = currState;
            OnStateEnter();
        }

        if (playerTransform.Transform == null)
            playerTransform.Transform = transform;

        OnStateUpdate();
        OnStateExit();

        //if (myBackSO.PossiblePositions[0] == null)
        //{
        //SetNodesReference(myBackSO, 0);
        // SetNodesReference(myBackSO, 1);
        //SetNodesReference(myBackSO, 2);
        // }
    }

    private void SetNodesReference(AIPossiblePositionsSO myNodeRange, int childIndex = 0)
    {
        myNodeRange.PossiblePositions.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            myBackSO.PossiblePositions.Add(transform.GetChild(9).transform.GetChild(childIndex).transform.GetChild(i));
        }
    }


    // SECTION - Method - State Machine ===================================================================
    public void OnStateEnter()
    {
        currState.OnStateEnter(this);
    }

    public void OnStateUpdate()
    {
        // Added to have player position known to everything
        currState.OnStateUpdate(this);
    }

    public void OnStateExit()
    {
        currState = currState.OnStateExit(this);
    }


    // SECTION - Method - Utility ===================================================================
    // NOTE : Decouple in its own script?
    public RaycastHit TryRayCastGround() // Only purpose is to aleviate eye bleeding
    {
        return StaticRayCaster.IsLineCastTouching(transform.position, -transform.up, DistanceGround, GameManager.instance.groundMask, IsDebugOn);
    }

    public RaycastHit TryRayCastRespawn() // Only purpose is to aleviate eye bleeding
    {
        return StaticRayCaster.IsLineCastTouching(transform.position, -transform.up, DistanceGround, GameManager.instance.respawnMask, IsDebugOn);
    }

    public RaycastHit TryRayCastInteractable() // Only purpose is to aleviate eye bleeding
    {
        Vector3 trueY = new Vector3(transform.position.x, transform.position.y * 0.5f, transform.position.z);
        return StaticRayCaster.IsLineCastTouching(trueY, transform.forward, distanceInteractible, GameManager.instance.interactableMask, isDebugOn);
    }

    #region REGION - Movement
    public void OnDefaultMovement(float stateDependantModifier = 1.0f)
    {
        float moveX = input.DirX * input.MoveFactor.Value;
        float moveZ = input.DirZ * input.MoveFactor.Value;

        Vector3 movement = (transform.right * moveX +
                            transform.up * rb.velocity.y +
                            transform.forward * moveZ) *
                            stateDependantModifier;

        rb.velocity = movement;
    }

    public void OnDefaultLook()
    {
        float lookY = input.LookY * input.MouseSensitivity.Value;

        Vector3 rotationValues = Vector3.up * lookY;

        transform.Rotate(rotationValues);
    }
    #endregion

    #region REGION - Weapon
    public void OnDefaultFireWeaponMelee()
    {
        if (input.FireMeleeWeapon)
        {
            if (!meleeWeapon.Weapon.CanFireContinuously)
            {
                input.FireMeleeWeapon = false;
            }

            meleeWeapon.TriggerWeapon();
        }
    }

    public void OnDefaultFireWeaponMain()
    {
        if (input.FireMainWeapon)
        {
            input.FireMainWeapon = mainWeapon.Weapon.ShootingStrategy.Press(mainWeapon);
            //if (mainWeapon.Weapon.ChargeTime == 0)
            //{
            //    if (!mainWeapon.Weapon.CanFireContinuously || mainWeapon.Weapon.CurrentClip == 0)
            //    {
            //        input.FireMainWeapon = false;
            //    }

            //    mainWeapon.TriggerWeapon();
            //}
            //else
            //{
            //    mainWeapon.ChargeWeapon();
            //}
        }
        else
        {
            mainWeapon.Weapon.ShootingStrategy.Release(mainWeapon);
            //if (mainWeapon.Weapon.ChargeTime != 0 && mainWeapon.Weapon.CurrentChargeTime != 0)
            //{
            //    if (mainWeapon.ChargeWeapon())
            //    {
            //        mainWeapon.TriggerWeapon();
            //    }
            //    mainWeapon.Weapon.CurrentChargeTime = 0;
            //}
        }
    }

    public void OnDefaultFireWeaponSecondary()
    {
        if (input.FireSecondaryWeapon)
        {
            input.FireSecondaryWeapon = false;

            secondaryWeapon.TriggerWeapon();
        }
    }

    public void OnDefaultWeaponChange()
    {
        if (input.WeaponOne)            // WEAPON ONE
        {
            input.WeaponOne = false;

            // EVENT GO HERE
            if (weapons.CarriedMainWeapons.Count > 1)
            {
                mainWeapon.ResetReload();
            }

            weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[0];
            mainWeapon.UpdateWeapon(); // weapons.EquippedMainWeapon
            // weaponHolder.MainWeapon = weapons.EquippedMainWeapon;
            StaticDebugger.SimpleDebugger(isDebugOn, $"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
            mainWeaponHasChanged.Invoke();
        }
        else if (input.WeaponTwo)       // WEAPON TWO
        {
            input.WeaponTwo = false;

            if (weapons.CarriedMainWeapons.Count > 1)
            {
                mainWeapon.ResetReload();
            }

            // EVENT GO HERE
            if (weapons.CarriedMainWeapons.Count > 1)
            {
                weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[1];
                mainWeapon.UpdateWeapon(); // weapons.EquippedMainWeapon
                // weaponHolder.MainWeapon = weapons.EquippedMainWeapon;
                StaticDebugger.SimpleDebugger(IsDebugOn, $"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
                mainWeaponHasChanged.Invoke();
            }
        }
        else if (input.WeaponThree)       // WEAPON TWO
        {
            input.WeaponThree = false;

            if (weapons.CarriedMainWeapons.Count > 1)
            {
                mainWeapon.ResetReload();
            }

            // EVENT GO HERE
            if (weapons.CarriedMainWeapons.Count > 2)
            {
                weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[2];
                mainWeapon.UpdateWeapon(); // weapons.EquippedMainWeapon
                // weaponHolder.MainWeapon = weapons.EquippedMainWeapon;
                StaticDebugger.SimpleDebugger(IsDebugOn, $"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
                mainWeaponHasChanged.Invoke();
            }
        }
        else if (input.WeaponScrollBackward)       // WEAPON SCROLL <=
        {
            input.WeaponScrollBackward = false;

            if (weapons.CarriedMainWeapons.Count > 1)
            {
                mainWeapon.ResetReload();
            }

            // EVENT GO HERE
            if (weapons.CarriedMainWeapons.Count > 1)
            {
                var index = weapons.CarriedMainWeapons.IndexOf(weapons.EquippedMainWeapon) + 1;
                if (index > weapons.CarriedMainWeapons.Count - 1)
                    index = 0;
                weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[index];
                mainWeapon.UpdateWeapon(); // weapons.EquippedMainWeapon
                // weaponHolder.MainWeapon = weapons.EquippedMainWeapon;
                StaticDebugger.SimpleDebugger(IsDebugOn, $"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
                mainWeaponHasChanged.Invoke();
            }
        }
        else if (input.WeaponScrollForward)       // WEAPON SCROLL =>
        {
            input.WeaponScrollForward = false;

            if (weapons.CarriedMainWeapons.Count > 1)
            {
                mainWeapon.ResetReload();
            }

            // EVENT GO HERE
            if (weapons.CarriedMainWeapons.Count > 1)
            {
                var index = weapons.CarriedMainWeapons.IndexOf(weapons.EquippedMainWeapon) - 1;
                if (index < 0)
                    index = weapons.CarriedMainWeapons.Count - 1;
                weapons.EquippedMainWeapon = weapons.CarriedMainWeapons[index];
                mainWeapon.UpdateWeapon(); // weapons.EquippedMainWeapon 
                // weaponHolder.MainWeapon = weapons.EquippedMainWeapon;
                StaticDebugger.SimpleDebugger(IsDebugOn, $"MAIN WEAPON CHANGED TO ... {weapons.EquippedMainWeapon.WeaponName}");
                mainWeaponHasChanged.Invoke();
            }
        }
    }

    public void OnWeaponReload()
    {
        if (input.Reload)
        {
            input.Reload = false;

            // EVENT GO HERE
            mainWeapon.ReloadWeapon();
        }
    }
    #endregion

    #region REGION - Misc
    public void OnDefaultInteract(RaycastHit hit)
    {
        if (input.Interact)
        {
            input.Interact = false;

            if (hit.transform != null)
            {
                Interactable interactable = hit.transform.GetComponentInChildren<Interactable>();

                if (interactable)
                {
                    // Canvas visual cue
                    interactCanvasHandler.SetVisualCue(interactable.IsInteractable);

                    // Event launch
                    interactable.OnInteraction();
                }
                else
                    StaticDebugger.SimpleDebugger(IsDebugOn, "Interactable was null on interaction");
            }
        }
    }

    public void OnDefaultShowMap()
    {
        if (input.ShowMap)
        {
            input.ShowMap = false;

            // EVENT GO HERE
            Minimap minimap = FindObjectOfType<Minimap>();
            if (minimap) minimap.ToggleMap();
        }
    }
    #endregion
}