using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Set target as backup", fileName = "ActionSO_Set target as backup")]
public class ActionSetTargetAsBackup : Action
{
    protected override NodeState OnTick()
    {
        tree.Brain.SetTargetAsBackup();
        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
