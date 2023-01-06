using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Decorator/Always failure",
                fileName = "DecoratorSO_Always failure_NAME")]
public class DecoratorAlwaysFailure : Decorator
{
    protected override NodeState OnTick()
    {
        children[0].Tick();

        m_state = NodeState.FAILURE;
        return m_state;
    }
}
