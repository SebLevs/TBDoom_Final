using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Decorator/Always success", fileName = "DecoratorSO_Always success_NAME")]
public class DecoratorAlwaysSuccess : Decorator
{
    protected override NodeState OnTick()
    {
        children[0].Tick();

        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
