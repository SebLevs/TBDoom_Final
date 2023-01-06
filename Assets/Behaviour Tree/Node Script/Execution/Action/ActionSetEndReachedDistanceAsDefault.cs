using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Set endReachedDistance as default",
                 fileName = "ActionSO_Set endReachedDistance as default")]
public class ActionSetEndReachedDistanceAsDefault : Action
{
    protected override NodeState OnTick()
    {
        tree.Brain.SetEndReachedDistance(tree.Brain.DefaultEndReachedDistance);
        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
