using UnityEngine;
using Pathfinding; // Aaron Granberg A*
using System.Collections;

public class BasicEnemyContext : MonoBehaviour
{
    // SECTION - Field ===================================================================
    #region REGION - HIDDEN - State
    private IEnemyState currState;
    private IEnemyState oldState;
    #endregion


    #region REGION - HIDDEN - Entity Specific
    private LivingEntityContext myLivingEntity;
    private Transform mySpriteTransform;
    #endregion


    #region REGION - HIDDEN - AStar Specific
    private NNConstraint constraint;
    private bool canUseBehaviour = true;
    private Transform myTemporaryTargetTransform;
    private AIPath myAIPath;                           // Movement, rotation, End Reached Distance, etc.
    private AIDestinationSetter myAIDestinationSetter; // Pathfinding Target
    private float maxSpeed;
    private const float defaultEndReachedDistance = 0.96f;
    #endregion


    #region REGION - HIDDEN - Animator Strings

    // Animator
    private Animator anim;

    // Parameters
    private readonly string animParam_ExitDeathAnim = "ExitDeathAnim";
    private readonly string animParam_maxSpeed = "maxSpeed";
    private readonly string animParam_animAngle = "animAngle";

    // Animation States
    private readonly string animState_Iddle = "Iddle";
    private readonly string animState_OnAwake = "OnAwake";
    private readonly string animState_OnMoveBlendTree = "BlendTree _ Movement";
    private readonly string animState_OnDeath = "OnDeath";

    private readonly string animState_01_Transition = "State_01_Transition";
    private readonly string animState_01_NoToken = "State_01_NoToken";
    private readonly string animState_01_Token = "State_01_Token";

    private readonly string animState_02_Transition = "State_02_Transition";
    private readonly string animState_02_NoToken = "State_02_NoToken";
    private readonly string animState_02_Token = "State_02_Token";
    #endregion


    [Header("    ======= On Start Specifications =======\n")]
    [SerializeField] private bool isBypassTokenSystem = false;
    [Tooltip("Constrain the search to only nodes with tag 0 (Basic Ground)\nThe 'tags' field is a bitmask")]
    [SerializeField] private int bitmaskConstraintTag = 0;
    [Space(10)]
    [SerializeField] private GameObject myTemporaryTargetPrefab;
    [Space(10)]
    [SerializeField] private BasicEnemy_States startingState = BasicEnemy_States.ONE;
    [SerializeField] private bool startAtMaxSpeed = true;

    [Header("    ======= Animator Specifications =======\n")]
    [Tooltip("Allows to keep last sprite for lingering dead enemies")]
    [SerializeField] private bool exitDeathAnim = true;

    [Header("    ======= Other Specifications =======\n")]
    [SerializeField] private BasicEnemy_Types type = BasicEnemy_Types.GROUNDED;
    [SerializeField] private bool isHoveringEnemy = false;

    [Space(10)]
    [Header("    ========== State One ==========\n")]
    [Header("Weapon ShootingRangeHandler")]
    [Tooltip("[Range] is used for [endReachedDistance]" +
             "[MainWeaponIsReloading] is used for time between two attacks")]
    [SerializeField] private WeaponManager weaponManager_1;

    [Header("Animator")]
    [Tooltip("If false, OnDeath animation will stay at last frame until object is destroyed")]

    [SerializeField] private bool to_2_OnAtkExit = false;
    [SerializeField] private bool noTokenHasAnim_1 = false;
    private AbstractBehaviour behaviour_NoToken_1;
    private AbstractBehaviour behaviour_Token_1;

    [Space(10)]
    [Header("    ========= State Two =========\n")]
    [Header("Weapon ShootingRangeHandler")]
    [SerializeField] private WeaponManager weaponManager_2;

    [Header("Animator")]
    [SerializeField] private bool to_1_OnAtkExit = false;
    [SerializeField] private bool noTokenHasAnim_2 = false;
    
    private AbstractBehaviour behaviour_NoToken_2;
    private AbstractBehaviour behaviour_Token_2;

    [Space(10)]
    [SerializeField] private bool hasToken = false;


    // SECTION - Property ===================================================================
    #region REGION - PROPERTY
    // State
    public IEnemyState CurrState { get => currState; set => currState = value; }

    public bool IsBypassTokenSystem { get => isBypassTokenSystem; }

    public NNConstraint Constraint { get => constraint; }

    // General
    public LivingEntityContext MyLivingEntity { get => myLivingEntity; set => myLivingEntity = value; }
    public Animator Anim { get => anim; set => anim = value; }

    // AI
    public AIPath MyAIPath { get => myAIPath; set => myAIPath = value; }
    public Transform MyTemporaryTargetTransform { get => myTemporaryTargetTransform; }

    // State One
    public WeaponManager WeaponManager_1 { get => weaponManager_1; set => weaponManager_1 = value; }
    public bool To_2_OnAtkExit { get => to_2_OnAtkExit; }
    public bool NoTokenHasAnim_1 { get => noTokenHasAnim_1; }
    public AbstractBehaviour Behaviour_NoToken_1 { get => behaviour_NoToken_1; }
    public AbstractBehaviour Behaviour_Token_1 { get => behaviour_Token_1; }

    // State Two
    public WeaponManager WeaponManager_2 { get => weaponManager_2; set => weaponManager_2 = value; }
    public bool To_1_OnAtkExit { get => to_1_OnAtkExit; }
    public bool NoTokenHasAnim_2 { get => noTokenHasAnim_2; }
    public AbstractBehaviour Behaviour_NoToken_2 { get => behaviour_NoToken_2; }
    public AbstractBehaviour Behaviour_Token_2 { get => behaviour_Token_2; }

    public bool HasToken { get => hasToken; set => hasToken = value; }
    public bool CanUseBehaviour { get => canUseBehaviour; set => canUseBehaviour = value; }
    public BasicEnemy_States GetCurrentStateHasEnum => (currState is BasicEnemyState_One) ? BasicEnemy_States.ONE : BasicEnemy_States.TWO;

    public BasicEnemy_Types Type { get => type; }

    // Variables from fields
    public bool HasReachedEndOfPath => myAIPath.reachedEndOfPath;

    public bool HasPath => myAIPath.hasPath;

    public Transform GetTargetTransform { get => myAIDestinationSetter.target; }
    #endregion


    // SECTION - Method - Unity Specific ===================================================================
    #region REGION - Methods - Unity Specific
    private void OnDestroy()
    {
        if (MyTemporaryTargetTransform)
            Destroy(myTemporaryTargetTransform.gameObject);
    }

    private void Start()
    {
        // Get Set Components & Variables
        GetSetHiddensHandler();

        // Set State Machine
        FirstStateHandler();
    }

    public bool isDebugOn = false;
    private void FixedUpdate()
    {
        if (oldState != currState)
        {
            oldState = currState;
            OnStateEnter();
        }

        OnStateUpdate();
        OnStateExit();

        if (isDebugOn)
            Debug.Log($"Enemy hasreachedendofpath: {HasReachedEndOfPath} | HasPath {HasPath}");
    }
    #endregion


    // SECTION - Method - State Specific ===================================================================
    #region REGION - Methods - State Specific
    public void OnStateEnter()
    {
        currState.OnStateEnter(this);
    }

    public void OnStateUpdate()
    {
        currState.OnStateUpdate(this);
    }

    public void OnStateExit()
    {
        currState = currState.OnStateExit(this);
    }
    #endregion


    // SECTION - Method - Utility ===================================================================
    #region REGION - On Start Handlers
    private void FirstStateHandler()
    {
        switch(startingState)
        {
            case BasicEnemy_States.ONE:
                SetFiniteStateMachine(BasicEnemy_States.ONE);
                break;
            case BasicEnemy_States.TWO:
                SetFiniteStateMachine(BasicEnemy_States.TWO);
                break;
            default: Debug.Log($"An error as occured at [FirstStateHandler()] of [EnemyContext.cs] from enemy: {gameObject.name}"); break;
        }

        oldState = currState;

        // Instantiate WeaponSOs && Set endReachedDistance
        FirstSetMainWeaponAndAIDistance(currState);     
    }

    private void FirstSetMainWeaponAndAIDistance(IEnemyState myState)
    {
        // Weapons
        WeaponSO myWeaponSO = null;

        if (myState is BasicEnemyState_One)
        {
            if (weaponManager_1 != null)
            {
                // Clone WeaponSO and set it up as main weapon
                myWeaponSO = Instantiate(weaponManager_1.Weapon);
                weaponManager_1.Weapon = myWeaponSO;
                return;
            }
        }
        else if (myState is BasicEnemyState_Two)
        {
            if (weaponManager_2 != null)
            {
                // Clone WeaponSO and set it up as main weapon
                myWeaponSO = Instantiate(weaponManager_2.Weapon);
                weaponManager_2.Weapon = myWeaponSO;
                return;
            }
        }

        if (myWeaponSO != null)
            SetEndReachedDistance(myWeaponSO.Range);
        else
            SetEndReachedDistance(defaultEndReachedDistance);
    }

    private void GetSetHiddensHandler()
    {
        // AI ========================================
        // Get Components
        MyAIPath = GetComponentInChildren<AIPath>();
        myAIDestinationSetter = GetComponentInChildren<AIDestinationSetter>();
        // Setter
        SetDefaultNNConstraint();

        // Set Variables
        myTemporaryTargetTransform = Instantiate(myTemporaryTargetPrefab, GameObject.Find("--------------------- DYNAMIC").transform).transform;
        maxSpeed = myAIPath.maxSpeed;


        if (myAIDestinationSetter.target == null)
            SetTargetAsPlayer();
            //myAIDestinationSetter.target = GameManager.instance.PlayerTransformRef;

        if (!startAtMaxSpeed)
            SetSpeed(0.0f);

        // Weapons ========================================
        // Instantiate SO as unique && set infinite clip
        SetNewWeaponSO(BasicEnemy_States.ONE);
        SetNewWeaponSO(BasicEnemy_States.TWO);


        // Miscellaneous ========================================
        myLivingEntity = GetComponentInChildren<LivingEntityContext>();
        mySpriteTransform = GetComponentInChildren<SpriteRenderer>().transform;
        anim = GetComponentInChildren<Animator>();
        anim.SetBool(animParam_ExitDeathAnim, exitDeathAnim);

        behaviour_NoToken_1 = transform.GetChild(1).transform.GetChild(0).GetComponentInChildren<AbstractBehaviour>();
        behaviour_Token_1 = transform.GetChild(1).transform.GetChild(1).GetComponentInChildren<AbstractBehaviour>();

        behaviour_NoToken_2 = transform.GetChild(2).transform.GetChild(0).GetComponentInChildren<AbstractBehaviour>();
        behaviour_Token_2 = transform.GetChild(2).transform.GetChild(1).GetComponentInChildren<AbstractBehaviour>();
    }

    private void SetDefaultNNConstraint()
    {
        // Set Nearest NodeSO Constraint
        constraint = NNConstraint.None;

        // Constrain the search to walkable nodes only
        constraint.constrainWalkability = true;
        constraint.walkable = true;

        // Constrain the search to only nodes with tag 0 (Basic Ground)
        // The 'tags' field is a bitmask
        constraint.constrainTags = true;
        constraint.tags = (1 << bitmaskConstraintTag);
    }


    #endregion

    #region REGION - Default Behaviours
    public void OnDefaultAttackBehaviour()
    {
        SetSpeed(0.0f);
    }

    private int lastIndex = 0;
    public void OnDefault_NoToken_Behaviour()
    {
        anim.SetFloat(animParam_maxSpeed, myAIPath.maxSpeed);
        float angle = 0.0f;
        if (PlayerContext.instance.transform)
        {
            angle = StaticEnemyAnimHandler.GetAngle(PlayerContext.instance.transform, transform);
        }
        anim.SetFloat(animParam_animAngle, StaticEnemyAnimHandler.GetIndex(angle, lastIndex));
        StaticEnemyAnimHandler.SetSpriteFlip(mySpriteTransform, angle);
    }

    public void OnDefault_Token_Behaviour()
    {
        if (!isBypassTokenSystem)
        {
            if (hasToken && !HasPath)
            {
                hasToken = AIManager.instance.MyTokenHandlerSO.ReturnToken(HasToken);
                SetTarget();
            }
            else if (!hasToken && IsTargetNear() && canUseBehaviour) // Might need tweak (delete canUseBehaviour check) to allow enemy to target player mid no-token behaviour
                TokenHandler();
        }
    }

    public void SetTargetAsPlayer()
    {
        //if (!myAIDestinationSetter.target == GameManager.instance.PlayerTransformRef.transform)
        //myAIDestinationSetter.target = GameManager.instance.PlayerTransformRef.transform;

        myAIDestinationSetter.target = GameObject.Find("Player").transform;
    }

    public void SetTarget(Transform newTarget = null)
    {
        if (newTarget == null)
        {
            myAIDestinationSetter.target = myTemporaryTargetTransform;
            return;
        }

        myAIDestinationSetter.target = newTarget;
    }

    public void SetTarget(Vector3 newPosition)
    {
        myAIDestinationSetter.target.position = newPosition;
    }

    public void SetTargetNull()
    {
        myAIDestinationSetter.target = null;
    }

    public bool TokenHandler()
    {
        if (!isBypassTokenSystem)
        {
            if (!hasToken && TrySetTargetAsPlayer())
            {
                hasToken = AIManager.instance.MyTokenHandlerSO.TryGetToken(hasToken);

                if (hasToken)
                    SetEndReachedDistance_ToCurrState();
            }
            else if (hasToken)
            {
                hasToken = AIManager.instance.MyTokenHandlerSO.ReturnToken(hasToken);
                SetTargetAsPlayer();
            }
        }

        return hasToken;
    }

    public void TryGetTokenDelegation()
    {
        if (!isBypassTokenSystem)
        {
            if (!hasToken && TrySetTargetAsPlayer())
                hasToken = AIManager.instance.MyTokenHandlerSO.TryGetToken(hasToken);

            if (hasToken)
                SetEndReachedDistance_ToCurrState();
        }
    }

    public void ReturnTokenDelegation()
    {
        if (!isBypassTokenSystem)
        {
            hasToken = AIManager.instance.MyTokenHandlerSO.ReturnToken(hasToken);
            SetEndReachedDistance();
        }
    }

    private bool TrySetTargetAsPlayer()
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        NNConstraint tempConstraint = constraint;
        tempConstraint.walkable = false;
        tempConstraint.constrainWalkability = false;
        toNode = AstarPath.active.graphs[(int)type].GetNearest(GameObject.Find("Player").transform.position, tempConstraint).node;
        fromNode = AstarPath.active.graphs[(int)type].GetNearest(transform.position, constraint).node;

        if (PathUtilities.IsPathPossible(fromNode, toNode))
        {
            SetTargetAsPlayer();
            return true;
        }
        return false;
    }
    #endregion

    #region REGION - Utility
    // Miscellaneous
    public void SetRigidBodyConstraint_Y()
    {
        if (isHoveringEnemy)
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
    }

    public bool IsTargetNear()
    {

        if (GetCurrentWeaponManager() == null)
        {
            Collider[] hit;
            hit = StaticRayCaster.IsOverlapSphereTouching(transform, MyAIPath.endReachedDistance, myAIDestinationSetter.target.gameObject.layer, true);

            return hit == null;
            //return !(hit[0].transform == null);
            // return true;
        }

        if (GetCurrentWeaponManager().TracksPlayer)
            return GetCurrentWeaponManager().IsTargetInFront();

        return GetCurrentWeaponManager().IsTargetAround();
    }

    // Target
    public void SetMyTemporaryTargetAs(Transform setAs)
    {
        myTemporaryTargetTransform.position = setAs.position;
    }

    public void SetMyTemporaryTargetAs(Vector3 setAs)
    {
        myTemporaryTargetTransform.position = setAs;
    }


    // Weapon
    public bool TryFireMainWeapon()
    {
        if (currState is BasicEnemyState_One)
            if (weaponManager_1 != null)
                return weaponManager_1.TriggerWeapon();
        else if (currState is BasicEnemyState_Two)
        {
                Debug.Log("FIRE MAIN WEAPON STATE TWO");
            if (weaponManager_2 != null)
                return weaponManager_2.TriggerWeapon();
        }


        return false; // true == prevent using main weapon when checking !IsMainWeaponReloading()
    }

    public bool TryFireMainWeapon(BasicEnemy_States stateSpecificCheck)
    {
        if (stateSpecificCheck == BasicEnemy_States.ONE)
            if (weaponManager_1 != null)
                return weaponManager_1.TriggerWeapon();
        else if (stateSpecificCheck == BasicEnemy_States.TWO)
            if (weaponManager_2 != null)
                return weaponManager_2.TriggerWeapon();

        return false; // true == prevent using main weapon when checking !IsMainWeaponReloading()
    }

    public bool IsCurrentWeaponManagerNull()
    {
        if (currState is BasicEnemyState_One && weaponManager_1 == null)
            return true;
        else if (currState is BasicEnemyState_Two && WeaponManager_2 == null)
            return true;

        return false;
    }

    public WeaponManager GetCurrentWeaponManager()
    {
        if (currState is BasicEnemyState_One && weaponManager_1 != null)
            return weaponManager_1;
        else if (currState is BasicEnemyState_Two && WeaponManager_2 != null)
            return WeaponManager_2;

        return null;
    }

    public WeaponManager GetSpecificWeaponManager(BasicEnemy_States specificState)
    {
        if (specificState == BasicEnemy_States.ONE)
            return weaponManager_2;
        else if (specificState == BasicEnemy_States.TWO)
            return WeaponManager_1;

        return null;
    }

    public void SetNewWeaponSO(BasicEnemy_States atState, WeaponSO myDesiredWeaponSO = null)
    {
        WeaponManager myWeaponManager = GetSpecificWeaponManager(atState);

        if (myWeaponManager != null)
        {
            if (myWeaponManager.Weapon == null)
                return;

            WeaponSO myNewWeaponSO;

            // Check for specific or current WeaponSO instantiate
            if (myDesiredWeaponSO == null)
                myNewWeaponSO = Instantiate(myWeaponManager.Weapon);
            else
                myNewWeaponSO = Instantiate(myDesiredWeaponSO);

            myNewWeaponSO.InfiniteAmmo = true; // Just in case

            myWeaponManager.Weapon = myNewWeaponSO;
        }
    }

    public bool IsWeaponReloading()
    {
        WeaponManager myWeaponManager = GetCurrentWeaponManager();

        if (myWeaponManager == null)
            return false;

        if (myWeaponManager.Weapon == null)
            return false;

        // Manage token in case of reload
        if (!isBypassTokenSystem && myWeaponManager.WeaponIsReloading && hasToken)
        {
            AIManager.instance.MyTokenHandlerSO.ReturnToken(hasToken);
            hasToken = false;
        }

        return myWeaponManager.WeaponIsReloading || myWeaponManager.Weapon.CurrentClip == 0;
    }

    public void DestroyAllWeaponSO()
    {
        WeaponManager myWeaponManager = GetSpecificWeaponManager(BasicEnemy_States.ONE);

        if (myWeaponManager != null)
            Destroy(myWeaponManager.Weapon);

        myWeaponManager = GetSpecificWeaponManager(BasicEnemy_States.TWO);

        if (myWeaponManager != null)
            Destroy(myWeaponManager.Weapon);
    }

    public AbstractBehaviour GetCurrentBehaviour()
    {
        // Reminder
        //      - Will return null if behaviour is null | Proceed accordingly

        if (currState is BasicEnemyState_One)
        {
            if (HasToken)
                return behaviour_Token_1;
            else
                return behaviour_NoToken_1;
        }
        else if (currState is BasicEnemyState_Two)
        {
            if (HasToken)
                return behaviour_Token_2;
            else
                return behaviour_NoToken_2;
        }

        return null;
    }

    // PathFinding
    public void SetSpeedAsDefault() // Note : Also used as animator event
    {
        MyAIPath.maxSpeed = maxSpeed;
    }

    public void SetSpeed(float newSpeed)
    {
        MyAIPath.maxSpeed = newSpeed;
    }

    public void SetEndReachedDistance(float newEndReachedDistance = defaultEndReachedDistance)
    {
        myAIPath.endReachedDistance = newEndReachedDistance;
    }

    public void SetEndReachedDistance_ToCurrState()
    {
        if (currState is BasicEnemyState_One)
        {
            if (weaponManager_1 != null)
            {
                SetEndReachedDistance(weaponManager_1.Weapon.Range);
                return;
            }
        }
        else if (currState is BasicEnemyState_Two)
        {
            if (weaponManager_2 != null)
            {
                SetEndReachedDistance(weaponManager_2.Weapon.Range);
                return;
            }
        }

        SetEndReachedDistance();
    }

    // State 
    public void SetFiniteStateMachine(BasicEnemy_States transitionTo)
    {
        switch (transitionTo)
        {
            case BasicEnemy_States.ONE:
                if (!(currState is BasicEnemyState_One))
                    currState = new BasicEnemyState_One();
                break;
            case BasicEnemy_States.TWO:
                if (!(currState is BasicEnemyState_Two))
                    currState = new BasicEnemyState_Two();
                break;
        }
    }

    public void ToggleState()
    {
        if (currState is BasicEnemyState_One)
            SetFiniteStateMachine(BasicEnemy_States.TWO);
        else
            SetFiniteStateMachine(BasicEnemy_States.ONE);
    }

    // Animator
    public void SetAwakingTransitionOnHit(bool shouldToggleState)
    {
        if (IsInAnimationState(BasicEnemy_AnimationStates.IDDLE))
        {
            SetTransitionAnim();

            if (shouldToggleState)
            {
                if (currState is BasicEnemyState_One)
                    currState = new BasicEnemyState_Two();
                else if (currState is BasicEnemyState_Two)
                    currState = new BasicEnemyState_One();
            }
        }
    }

    public void SetTransitionAnim()
    {
        if (currState is BasicEnemyState_One)
            anim.SetTrigger(animState_01_Transition);
        else if (currState is BasicEnemyState_Two)
            anim.SetTrigger(animState_02_Transition);
    }

    public void SetAnimTrigger(BasicEnemy_AnimTriggers trigger)
    {
        switch (trigger)
        {
            case BasicEnemy_AnimTriggers.DEATH:                 // MISC
                anim.SetTrigger(animState_OnDeath);
                break;

            case BasicEnemy_AnimTriggers.EXITDEATH:
                anim.SetBool(animParam_ExitDeathAnim, true);
                break;


            case BasicEnemy_AnimTriggers.STATE_01_TRANSITION:   // STATE ONE
                anim.SetTrigger(animState_01_Transition);
                break;

            case BasicEnemy_AnimTriggers.STATE_01_NOTOKEN:
                anim.SetTrigger(animState_01_NoToken);
                break;

            case BasicEnemy_AnimTriggers.STATE_01_TOKEN:
                anim.SetTrigger(animState_01_Token);
                break;


            case BasicEnemy_AnimTriggers.STATE_02_TRANSITION:   // STATE TWO
                anim.SetTrigger(animState_02_Transition);
                break;

            case BasicEnemy_AnimTriggers.STATE_02_NOTOKEN:
                anim.SetTrigger(animState_02_NoToken);
                break;

            case BasicEnemy_AnimTriggers.STATE_02_TOKEN:
                anim.SetTrigger(animState_02_Token);
                break;

            default: Debug.Log($"An error as occured at [SetAnimTrigger()] of [EnemyContext.cs] from enemy: {gameObject.name}"); break;
        }
    }

    public bool IsInAnimationState(BasicEnemy_AnimationStates checkAnimation)
    {
        switch (checkAnimation)
        {
            case BasicEnemy_AnimationStates.IDDLE:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_Iddle);

            case BasicEnemy_AnimationStates.ONAWAKE:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnAwake);

            case BasicEnemy_AnimationStates.MOVEMENT:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnMoveBlendTree);

            case BasicEnemy_AnimationStates.DEAD:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnDeath);



            case BasicEnemy_AnimationStates.STATE_01_TRANSITION:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_01_Transition);

            case BasicEnemy_AnimationStates.STATE_01_NOTOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_01_NoToken);

            case BasicEnemy_AnimationStates.STATE_01_TOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_01_Token);


            case BasicEnemy_AnimationStates.STATE_02_TRANSITION:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_02_Transition);

            case BasicEnemy_AnimationStates.STATE_02_NOTOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_02_NoToken);

            case BasicEnemy_AnimationStates.STATE_02_TOKEN:
                return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_02_Token);


            default: Debug.Log($"An error as occured at [IsInAnimationState()] of [EnemyContext.cs] from enemy: {gameObject.name}"); break;
        }

        return false;
    }

    public bool IsIddleOrMoving()
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(animState_Iddle) ||
                anim.GetCurrentAnimatorStateInfo(0).IsName(animState_OnMoveBlendTree);
    }

    public float GetCurrentAnimStateLength()
    {
        return anim.GetCurrentAnimatorStateInfo(0).length;
    }

    private void AE_ExecuteCurrentBehaviour(AdditionalWeaponEvent desiredEvent = AdditionalWeaponEvent.NONE) 
    {
        AbstractBehaviour myCurrentBehaviour = GetCurrentBehaviour();

        if (hasToken && !isBypassTokenSystem) // TODO: !isBypass... should be removed and an alternative should be found
            TryFireMainWeapon();

        if (!myCurrentBehaviour)
            return;

        myCurrentBehaviour.Execute();

        // Prevents the call of [WaitUntilBehaviourThenCallWeaponEvent()] if an enemy is dead during any behaviour
        if (currState is EnemyStateDead)
            return;

        StartCoroutine(WaitUntilBehaviourThenCallWeaponEvent(desiredEvent));
    }

    private IEnumerator WaitUntilBehaviourThenCallWeaponEvent(AdditionalWeaponEvent desiredEvent = AdditionalWeaponEvent.NONE)
    {
        if (currState is EnemyStateDead)
            yield break;

        yield return new WaitUntil(() => CanUseBehaviour);

        WeaponManager myWeaponManager = GetCurrentWeaponManager();

        if (myWeaponManager)
            switch (desiredEvent)
            {
                case AdditionalWeaponEvent.NONE:
                    break;

                case AdditionalWeaponEvent.ALL:
                    myWeaponManager.WeaponHasChanged.Invoke();
                    myWeaponManager.WeaponFinishedReloading.Invoke();
                    myWeaponManager.WeaponHasShot.Invoke();
                    myWeaponManager.WeaponStartedReloading.Invoke();
                    break;

                case AdditionalWeaponEvent.HASCHANGED:
                    myWeaponManager.WeaponHasChanged.Invoke();
                    break;

                case AdditionalWeaponEvent.FINISHEDRELOADING:
                    myWeaponManager.WeaponFinishedReloading.Invoke();
                    break;

                case AdditionalWeaponEvent.HASSHOT:
                    myWeaponManager.WeaponHasShot.Invoke();
                    break;

                case AdditionalWeaponEvent.STARTEDRELOADING:
                    myWeaponManager.WeaponStartedReloading.Invoke();
                    break;

                default: Debug.Log("AN ERROR HAS OCCURED"); break;
            }
    }


    private void AE_FreezeRigidBodyDisableCollider()
    {
        Rigidbody myRigidBody = GetComponent<Rigidbody>();
        myRigidBody.constraints = RigidbodyConstraints.FreezePosition; 
        GetComponent<Collider>().enabled = false;
    }
    #endregion
}