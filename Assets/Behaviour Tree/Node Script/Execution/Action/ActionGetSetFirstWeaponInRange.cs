using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/GetSet first weapon in range", 
                fileName = "ActionSO_GetSet first weapon in range")]
public class ActionGetSetFirstWeaponInRange : Action
{
    protected override NodeState OnTick()
    {
        WeaponManager validWeaponManager = tree.Brain.GetFirstWeaponManagerInRange();

        // Quit argument
        if (validWeaponManager == null)
        {
            m_state = NodeState.FAILURE;
            return m_state;
        }

        tree.Brain.SetCurrentWeaponManager(validWeaponManager);
        tree.Brain.SetEndReachedDistance(tree.Brain.CurrentWeaponManager.Weapon.Range);
        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
