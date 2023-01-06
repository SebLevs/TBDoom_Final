using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Is Target Near", fileName = "ConditionSO_Is Target Near")]
public class ConditionIsTargetNear : Condition
{
    [SerializeField] private float distance = 0.96f;

    protected override NodeState OnTick()
    {
        float evalDistance = Vector3.Magnitude(tree.Brain.transform.position - tree.Brain.Target.position);

        m_state = evalDistance <= distance ? NodeState.SUCCESS : NodeState.FAILURE;

        //Debug.ClearDeveloperConsole();

        return m_state;
    }
}
