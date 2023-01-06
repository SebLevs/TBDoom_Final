using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Is iddle or moving",
                 fileName = "Is iddle or moving")]
public class ConditionIsIddleOrMoving : Condition
{
    protected override NodeState OnTick()
    {
        bool evaluation = tree.Brain.GetCurrentAnimationClip().name == "Iddle" || tree.Brain.GetCurrentAnimationClip().name == "Movement";

        m_state = (evaluation)? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }
}
