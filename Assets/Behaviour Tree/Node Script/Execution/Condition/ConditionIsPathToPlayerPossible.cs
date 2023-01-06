using UnityEngine;
using Pathfinding;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Is path to player possible",
                 fileName = "ConditionSO_Is path to player possible")]
public class ConditionIsPathToPlayerPossible : Condition
{
    protected override NodeState OnTick()
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        toNode = AstarPath.active.graphs[(int)tree.Brain.EnemyType].GetNearest(tree.Brain.PlayerTransform.position, tree.Brain.Constraint).node;
        fromNode = AstarPath.active.graphs[(int)tree.Brain.EnemyType].GetNearest(tree.Brain.transform.position, tree.Brain.Constraint).node;

        m_state = PathUtilities.IsPathPossible(fromNode, toNode) ? NodeState.SUCCESS : NodeState.FAILURE;

        return m_state;
    }
}
