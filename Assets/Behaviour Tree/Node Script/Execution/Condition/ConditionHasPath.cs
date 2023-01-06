using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Has path", fileName = "ConditionSO_Has path")]
public class ConditionHasPath : Condition
{
    protected override NodeState OnTick()
    {
        m_state = tree.Brain.HasPath ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }
}
