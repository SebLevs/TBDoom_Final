using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Has reached end of path", 
                 fileName = "ConditionSO_Has reached end of path")]
public class ConditionHasReachedEndOfPath : Condition
{
    protected override NodeState OnTick()
    {
        // TODO: If bugs are encountered regarding SUCCES when !HasPath, add check with tree.Brain.HasPath
        m_state = tree.Brain.HasReachedEndOfPath ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }
}
