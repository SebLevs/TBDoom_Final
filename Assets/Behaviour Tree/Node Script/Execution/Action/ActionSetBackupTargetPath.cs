using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Set backup target path (one per enemy and action)", 
                fileName = "ActionSO_Set backup target path_NAME")]
public class ActionSetBackupTargetPath : Action
{
    [SerializeField] private AIPossiblePositionsSO myDesiredPositions;

    [Header("Next position modifier")]   
    [Tooltip("Random.insideUnitSphere * radiusModifier + 0.96f\nNote: insideUnitSphere is normalised")]
    [Min(0.0f)][SerializeField] private float radiusModifier = 2.56f;
                     private const float minimumDistance = 0.96f;

    protected override NodeState OnTick()
    {
        // Pick a random position near either a random node from desired positions or near current target
        if (myDesiredPositions)
            if (TrySetPath(GetRandomPointNear(myDesiredPositions.GetRandomNode())))
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
       
        m_state = TrySetPath(GetRandomPointNear(tree.Brain.transform.position)) ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }

    /// <summary>
    /// Get a random position around [fromPosition] on a 2D plane (y = 0)
    /// </summary>
    private Vector3 GetRandomPointNear(Vector3 fromPosition)
    {
        float x = minimumDistance * ((UnityEngine.Random.Range(-1, 1) < 0) ? -1 : 1);
        float z = minimumDistance * ((UnityEngine.Random.Range(-1, 1) < 0) ? -1 : 1);
        Vector3 addedDistance = new(x, 0.0f, z);

        Vector3 position = Random.insideUnitSphere * radiusModifier + addedDistance;
        position.y = 0;
        position += fromPosition; // Add a minimum range

        return position;
    }

    private bool TrySetPath(Vector3 towardsPosition)
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        toNode = AstarPath.active.graphs[(int)tree.Brain.EnemyType].GetNearest(towardsPosition, tree.Brain.Constraint).node;
        fromNode = AstarPath.active.graphs[(int)tree.Brain.EnemyType].GetNearest(tree.Brain.transform.position, tree.Brain.Constraint).node;

        if (PathUtilities.IsPathPossible(fromNode, toNode))
        {
            tree.Brain.SetMyBackupPosition((Vector3)toNode.position);
            return true;
        }
        return false;
    }
}
