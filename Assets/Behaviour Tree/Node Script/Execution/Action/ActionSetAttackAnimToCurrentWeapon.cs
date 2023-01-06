using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Set attack anim to current weapon", 
                fileName = "ActionSO_Set attack anim to current weapon")]
public class ActionSetAttackAnimToCurrentWeapon : Action
{
    protected override NodeState OnTick()
    {
        tree.Brain.SetPlayCurrentWeaponAnimation();

        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
