using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Move Towards Target", fileName = "ActionSO_Move Towards Target")]
public class ActionMoveTowardsTarget : Action
{
    protected override void OnEnter()
    {
        m_state = NodeState.RUNNING;
    }

    private bool startedRunning = false;
    protected override NodeState OnTick()
    {
        tree.Brain.transform.position = Vector3.MoveTowards(tree.Brain.transform.position, tree.Brain.Target.position, 2.0f * Time.deltaTime);
        Debug.Log("Node 2 -Action: Move toward target-: " + m_state);

        return m_state;
    }

    protected override void OnExit()
    {
    }
}
