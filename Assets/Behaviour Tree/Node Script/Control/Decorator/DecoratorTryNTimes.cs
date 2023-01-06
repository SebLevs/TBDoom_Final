using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Decorator/Try NN times", fileName = "DecoratorSO_Try NN times_NAME")]
public class DecoratorTryNTimes : Decorator
{
    [Min(0)] [SerializeField] private int tries = 2;

    protected override NodeState OnTick()
    {
        for (int i = 0; i < tries; i++)
        {
            m_state = children[0].Tick();
            if (m_state == NodeState.SUCCESS || m_state == NodeState.RUNNING) break;
        }
        return m_state;
    }
}
