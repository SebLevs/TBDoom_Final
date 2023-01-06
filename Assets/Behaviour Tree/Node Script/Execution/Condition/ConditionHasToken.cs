using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Has token", fileName = "ConditionSO_Has token")]
public class ConditionHasToken : Condition
{
    protected override NodeState OnTick()
    {
        m_state = tree.Brain.HasToken ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }
}
