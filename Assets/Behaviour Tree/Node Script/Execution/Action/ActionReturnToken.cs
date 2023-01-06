using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Return token", fileName = "ActionSO_Return Token")]
public class ActionReturnToken : Action
{
    protected override NodeState OnTick()
    {
        m_state = AIManager.instance.ReturnToken(tree.Brain) ? NodeState.SUCCESS : NodeState.FAILURE;
        Debug.Log($"Did I ({tree.Brain.gameObject.name}) returned token? {tree.Brain.HasToken} : State {m_state}");
        return m_state;
    }
}
