using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Execution/Action/Wait NN seconds",
                 fileName = "Wait NN seconds")]
public class ActionWait : Action
{
    [SerializeField] private float _waitTime = 1.0f;
    [SerializeField] private float m_currentTime = 0.0f;

    protected override NodeState OnTick()
    {
        m_currentTime += Time.deltaTime;
        Debug.Log($"Time is: {m_currentTime} / {_waitTime}");
        m_state = (m_currentTime >= _waitTime) ? NodeState.SUCCESS : NodeState.FAILURE;
        return m_state;
    }

    protected override void OnExit()
    {
        if (m_state == NodeState.SUCCESS)
        {
            m_currentTime = 0.0f;
        }
    }
}
