using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Decorator/Inverter", fileName = "DecoratorSO_Inverter_NAME")]
public class DecoratorInverter : Decorator
{
    protected override NodeState OnTick()
    {
        m_state = children[0].Tick();

        if (m_state == NodeState.SUCCESS)
            m_state = NodeState.FAILURE;
        else if (m_state == NodeState.FAILURE)
            m_state = NodeState.SUCCESS;

        return m_state;
    }
}
