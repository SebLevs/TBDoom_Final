using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Set endReachedDistance as current weapon", 
                 fileName = "ActionSO_Set endReachedDistance as current weapon")]
public class ActionSetEndReachedDistanceAsCurrentWeapon : Action
{
    protected override NodeState OnTick()
    {
        tree.Brain.SetEndReachedDistance(tree.Brain.CurrentWeaponManager.Weapon.Range);
        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
