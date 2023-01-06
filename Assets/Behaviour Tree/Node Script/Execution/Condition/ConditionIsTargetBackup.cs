using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Is target backup", fileName = "ConditionSO_Is target backup")]
public class ConditionIsTargetBackup : Condition
{
    protected override NodeState OnTick()
    {
        m_state = tree.Brain.Target == tree.Brain.MyBackupTargetTransform ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }
}
