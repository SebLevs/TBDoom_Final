using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Try get token", fileName = "ActionSO_Try get token")]
public class ActionTryGetToken : Action
{
    protected override NodeState OnTick()
    {
        tree.Brain.HasToken = AIManager.instance.MyTokenHandlerSO.TryGetToken(tree.Brain.HasToken);
        m_state = tree.Brain.HasToken ? NodeState.SUCCESS: NodeState.FAILURE;
        return m_state;
    }
}
