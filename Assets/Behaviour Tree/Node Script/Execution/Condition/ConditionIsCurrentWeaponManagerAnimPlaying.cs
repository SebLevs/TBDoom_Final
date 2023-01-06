using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Condition/Is current weapon manager anim playing", 
                 fileName = "ConditionSO_Is current weapon manager anim playing")]
public class ConditionIsCurrentWeaponManagerAnimPlaying : Condition
{
    protected override NodeState OnTick()
    {
       bool evaluation = tree.Brain.GetCurrentAnimationClip().name ==
                         tree.Brain.AnimContainer.GetValuesAt(tree.Brain.CurrentWeaponManager).animClip.name;

        m_state = evaluation ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }
}
