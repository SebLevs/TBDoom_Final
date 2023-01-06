using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Set target as player", fileName = "ActionSO_Set target as player")]
public class ActionSetTargetAsPlayer : Action
{
    protected override NodeState OnTick()
    {
        tree.Brain.SetTargetAsPlayer();
        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
