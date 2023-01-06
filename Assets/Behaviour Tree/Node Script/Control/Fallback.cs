using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// OnTick() logic definition<br/>
/// ... OR operator<br/>
/// ... SUCCESS condition<br/>
/// ... ... At least one child SUCCESS
/// </summary>
[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Fallback", 
                fileName = "FallbackSO_NAME")]
public class Fallback : Control
{
    protected override NodeState OnTick()
    {
        foreach (Node node in children)
        {
            m_state = node.Tick();

            if (m_state == NodeState.SUCCESS) break;
            if (m_state == NodeState.RUNNING) break;
        }

        return m_state;
    }
}
