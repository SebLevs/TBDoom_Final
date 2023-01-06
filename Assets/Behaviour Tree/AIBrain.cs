using UnityEngine;
using Pathfinding; // Aron Granberg A*
using AnimationContainer; // Animation infos
using UnityEngine.Animations;

public class AIBrain : MonoBehaviour
{
    // SECTION - Field =========================================================
    #region Misc
    [SerializeField] private bool isDebugOn = false;
    [SerializeField] private bool isHoveringEnemy = false;
    [Space(10)]
    [SerializeField] private BasicEnemy_Types enemyType = BasicEnemy_Types.GROUNDED;
    private LivingEntityContext myLivingEntity;
    #endregion

    #region AI
    [Header("    ======= AI =======\n")]
    [SerializeField] private BehaviourTreeSO myTreeSO;
    [SerializeField] private AIBlackBoardSO blackboard;
    #endregion

    #region Pathfinding  
    private float defaultSpeed = 2.0f;
    private Transform myBackupTargetTransform;
    private AIPath myAIPath;                           // Movement, rotation, End Reached Distance, etc.
    private AIDestinationSetter myAIDestinationSetter; // Target
    #endregion

    #region Animator
    private Transform mySpriteTransform;
    private Animator myAnim;

    // TODO: May need refactoring bellow into a values

    // Default Parameters
    private readonly string animParam_ExitDeathAnim = "ExitDeathAnim";
    private readonly string animParam_maxSpeed = "maxSpeed";
    private readonly string animParam_animAngle = "animAngle";

    // Default States
    private readonly string animState_OnMoveBlendTree = "BlendTree_Movement";
    private readonly string animState_OnIddle = "OnIddle";
    private readonly string animState_OnDeath = "OnDeath";
    #endregion

    #region Audio
    private AudioSource myAudioSource;
    #endregion

    #region WeaponManager
    [Header("    ======= Key pairing =======\n")]
    [SerializeField] private Container animContainer;
    private WeaponManager currentWeaponManager;
    #endregion


    // SECTION - Property =========================================================
    #region Class
    public bool IsDebugOn { get => isDebugOn; }
    public BasicEnemy_Types EnemyType { get => enemyType; }
    public bool HasToken { get; set; }
    public AIBlackBoardSO BlackBoard { get => blackboard; }
    #endregion

    #region Blackboard shorthand
    public float DefaultEndReachedDistance => blackboard.DefaultEndReachedDistance.Value;
    public int BitmaskConstraintTag => blackboard.bitmaskConstraintTag;
    public NNConstraint Constraint => blackboard.Constraint;
    public Transform PlayerTransform
    {
        get => blackboard.PlayerTransform != null ?
             blackboard.PlayerTransform.transform :
             GameObject.Find("Player").transform;
    }
    public int CurrentLevel => blackboard.CurrentLevelSO;
    #endregion

    #region PathFinding
    public bool HasReachedEndOfPath => myAIPath.reachedEndOfPath;
    public bool HasPath => myAIPath.hasPath;

    public Transform Target { get => myAIDestinationSetter.target; }
    public Transform MyBackupTargetTransform { get => myBackupTargetTransform; }
    #endregion

    #region Weapon Manager
    public Container AnimContainer { get => animContainer; }
    public WeaponManager CurrentWeaponManager { get => currentWeaponManager; }
    #endregion


    // SECTION - Method =========================================================
    #region Unity
    private void OnDestroy()
    {
        if (myBackupTargetTransform)
            Destroy(myBackupTargetTransform.gameObject);
    }

    private void Awake()
    {
        // get components
        myLivingEntity = GetComponent<LivingEntityContext>();
        mySpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        myAnim = GetComponentInChildren<Animator>();
        myBackupTargetTransform = Instantiate(blackboard.MyBackupTarget, GameObject.Find("--------------------- DYNAMIC").transform).transform;
        myAudioSource = GetComponentInChildren<AudioSource>();

        SetPathfindingReferences();

        // Set default weapon manager
        if (animContainer.Length != 0)
            SetCurrentWeaponManager(animContainer.GetValuesAt(0).weaponManager);

        // set tree
        SetTreeBrain();

        myAIDestinationSetter.target = PlayerTransform;
    }

    private void FixedUpdate()
    {
        if (myTreeSO)
            myTreeSO.Tick();

        SetAnimatorParameters();
    }
    #endregion


    #region Handler_Pathfinding
    public bool TrySetTargetAsPlayer() // TODO: Refactor into a BT node
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        NNConstraint tempConstraint = Constraint;
        tempConstraint.walkable = false;
        tempConstraint.constrainWalkability = false;
        toNode = AstarPath.active.graphs[(int)enemyType].GetNearest(PlayerTransform.position, tempConstraint).node;
        fromNode = AstarPath.active.graphs[(int)enemyType].GetNearest(transform.position, Constraint).node;

        if (PathUtilities.IsPathPossible(fromNode, toNode))
        {
            SetTargetAsPlayer();
            return true;
        }
        return false;
    }
    #endregion

    #region Handler_Behaviour Tree
    public void Kill()
    {
        myTreeSO.Kill();
        this.enabled = false;
    }
    #endregion


    #region Getter_Pathfinding
    public bool IsNearTarget(WeaponManager weaponManager)
    {
        float evalDistance = Vector3.Magnitude(transform.position - Target.position);
        return evalDistance <= weaponManager.Weapon.Range * 0.95f;
    }
    #endregion

    #region Getter_Animator
    public AnimationClip GetCurrentAnimationClip()
    {
        return myAnim.GetCurrentAnimatorClipInfo(0)[0].clip;
    }
    #endregion

    #region Getter_Container
    /// <summary>
    /// Returns first instance found<br/>
    /// WeaponManager: If any weapon is in range to target<br/>
    /// null: If no weapon is in range to target
    /// </summary>
    public WeaponManager GetFirstWeaponManagerInRange()
    {
        for (int i = 0; i < animContainer.Length; i++)
        {
            if (!animContainer.GetValuesAt(i).weaponManager.WeaponIsReloading &&
                IsNearTarget(animContainer.GetValuesAt(i).weaponManager))
            { return animContainer.GetValuesAt(i).weaponManager; }
        }

        return null;
    }
    #endregion


    #region Setter_PathFinding
    private void SetPathfindingReferences()
    {
        myAIPath = GetComponentInChildren<AIPath>();
        myAIDestinationSetter = GetComponentInChildren<AIDestinationSetter>();
        defaultSpeed = myAIPath.maxSpeed != 0 ? myAIPath.maxSpeed : defaultSpeed;
    }

    public void SetTargetAsPlayer() { myAIDestinationSetter.target = PlayerTransform; }
    public void SetTargetAsBackup() { myAIDestinationSetter.target = myBackupTargetTransform; }
    public void SetTarget(Transform newTarget) { myAIDestinationSetter.target = newTarget; }

    public void SetMyBackupPosition(Transform transform)
    { myBackupTargetTransform.position = transform.position; }
    public void SetMyBackupPosition(Vector3 position)
    { myBackupTargetTransform.position = position; }

    public void SetEndReachedDistance(float newEndReachedDistance) { myAIPath.endReachedDistance = newEndReachedDistance; }

    public void SetSpeedAsDefault() { myAIPath.maxSpeed = defaultSpeed; }
    public void SetSpeed(float newSpeed) { myAIPath.maxSpeed = newSpeed; }
    #endregion

    #region Setter_Behaviour Tree
    private void SetTreeBrain()
    {
        if (myTreeSO == null)
        {
            StaticDebugger.SimpleDebugger(isDebugOn, $"{gameObject.name} at {this.name} does not have a {myTreeSO.name}");
            return;
        }

        myTreeSO = Instantiate(myTreeSO); // copy
        myTreeSO.Brain = this;

        myTreeSO.OnAwakeSetter();
    }
    #endregion

    #region Setter_Animator

    private int lastIndex = 0;
    /// <summary>
    /// Flip sprite and set animator's parameters<br/><br/>
    /// 
    /// Parameter:<br/>
    /// maxSpeed<br/>
    /// angle
    /// </summary>
    private void SetAnimatorParameters()
    {
        myAnim.SetFloat(animParam_maxSpeed, myAIPath.maxSpeed);

        float angle = StaticEnemyAnimHandler.GetAngle(PlayerContext.instance.transform, transform);
        myAnim.SetFloat(animParam_animAngle, StaticEnemyAnimHandler.GetIndex(angle, lastIndex));
        StaticEnemyAnimHandler.SetSpriteFlip(mySpriteTransform, angle);
    }
    #endregion

    #region Setter_Current Weapon
    public void SetCurrentWeaponManager(WeaponManager weaponManager) { currentWeaponManager = weaponManager; }

    public void SetPlayCurrentWeaponAnimation()
    {
        string animatorState = animContainer.GetValuesAt(currentWeaponManager).animClip.name;
        myAnim.Play(animatorState);
    }
    #endregion


    #region Animator Event
    public void AE_PlayOneShot(AudioClip clip)
    {
        myAudioSource.clip = clip;
        myAudioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Recursively Copy() the node and its children<br/>
    /// Then Tick()<br/>
    /// Then recursively Kill() the node and its children<br/>
    /// </summary>
    public void AE_TickSpecificNode(Node node) // TODO: MAY NEED DELETION IF NOT USEFULL
    {
        Node copy = node.Copy(); // Copy
        copy.SetTree(myTreeSO);
        copy.Tick();
        copy.Kill();
    }

    private void AE_TriggerCurrentWeapon()
    {
        currentWeaponManager.TriggerWeapon();
    }

    private void AE_OnDeathFreeze()
    {
        Rigidbody myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.constraints = RigidbodyConstraints.FreezePosition;
        GetComponent<Collider>().enabled = false;
        //AIManager.instance.ReturnToken(this);
    }
    #endregion
}
