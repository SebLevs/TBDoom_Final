using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Blackboard", fileName = "AIBlackboard")]
public class AIBlackBoardSO : ScriptableObject
{
    // SECTION - Field =========================================================
    [SerializeField] private IntVariable currentLevelSO;
    [SerializeField] private FloatVariable defaultEndReachedDistance;

    /// <summary>
    /// Constrain the search to only nodes with tag 0 (Basic Ground)<br/> 
    /// The 'tags' field is a bitmask
    /// </summary>
    public readonly int bitmaskConstraintTag = 0;
    private NNConstraint constraint;

    [Space(10)]
    [SerializeField] private TransformSO playerTransformSO;
    [SerializeField] private GameObject myBackupTarget;


    // SECTION - Property =========================================================
    #region Property
    public int CurrentLevelSO { get => currentLevelSO.Value; }
    public FloatVariable DefaultEndReachedDistance { get => defaultEndReachedDistance; }

    public NNConstraint Constraint { get => constraint; }

    public Transform PlayerTransform { get => playerTransformSO.Transform; }
    public GameObject MyBackupTarget { get => myBackupTarget; }
    #endregion


    // SECTION - Method =========================================================
    #region Unity
    private void OnEnable()
    {
        if (constraint == null)
            SetDefaultNNConstraint();
    }
    #endregion

    #region Setter
    public void SetDefaultNNConstraint()
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
}
