using UnityEngine;

/// <summary>
/// OnTick() logic definition<br/>
/// ... AND operator<br/>
/// ... SUCCESS condition<br/>
/// ... ... All children SUCCESS
/// </summary>
[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Sequence", 
                fileName = "SequenceSO_NAME")]
public class Sequence : Control
{
    protected override NodeState OnTick()
    {
        foreach (Node node in children)
        {
            m_state = node.Tick();

            if (m_state == NodeState.FAILURE) break;
            if (m_state == NodeState.RUNNING) break;
        }

        return m_state;
    }
}
