using UnityEditor;
using UnityEngine;

/// <summary>
/// OnTick() logic definition<br/>
/// ... Conditional OR<br/>
/// ... SUCCESS condition:<br/>
/// ... ... N children return SUCCESS<br/><br/>
/// ... FAILURE condition:<br/>
/// ... ... N children return FAILURE<br/><br/>
/// ... RUNNING condition:<br/>
/// ... ... first RUNNING child
/// </summary>
[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/ParallelAsynchronous", 
                fileName = "ParallelAsynchronousSO_NAME")]
public class ParallelAsynchronous : Control
{
    [Header("Return SUCCESS when")]
    [Min(0)] [SerializeField] private int requiredSuccesses = 1;
    private int successes;

    //[Header("Return FAILURE when")]
    [Min(0)] [SerializeField] private int requiredFailure = 1; 
    private int failures;

    private void OnEnable()
    {
        if (requiredFailure > children.Count)
            requiredFailure = children.Count;
        if (requiredSuccesses > children.Count)
            requiredSuccesses = children.Count;
    }

    protected override void OnEnter()
    {
        successes = 0;
        failures = 0;
    }

    protected override NodeState OnTick()
    {
        foreach (Node node in children)
        {
            m_state = node.Tick();

            if (m_state == NodeState.SUCCESS) successes++;
            else if (m_state == NodeState.FAILURE) failures++;

            if (m_state == NodeState.RUNNING) break;

            if (successes == requiredSuccesses)
            {
                m_state = NodeState.SUCCESS;
                break;
            }
                
            if (failures == requiredFailure)
            {
                m_state = NodeState.FAILURE;
                break;
            }

        }

        return m_state;
    }

    public override Node Copy()
    {
        Node node = base.Copy();
        (node as ParallelAsynchronous).requiredSuccesses = requiredSuccesses;
        (node as ParallelAsynchronous).requiredFailure = requiredFailure;

        return node;
    }
}
