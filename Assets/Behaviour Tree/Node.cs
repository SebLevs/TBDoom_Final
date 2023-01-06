using UnityEditor;
using UnityEngine;

public enum NodeState { SUCCESS, FAILURE, RUNNING }

public class Node : ScriptableObject
{
    // Allows to access an asset through guid instead of path:
    // (Type)AssetDatabase.GUIDToAssetPath(guidString)
    public string guid { get; set; } // Guid.NewGuid().ToString();
    protected NodeState m_state = NodeState.FAILURE;

    protected BehaviourTreeSO tree = null;

    /// <summary>
    /// !!! DO NOT OVERRIDE - override OnTick() instead!!! <br/>
    /// OnEnter() // !RUNNING <br/>
    /// OnTick() // Returns updated m_state <br/>
    /// OnExit() // SUCCESS || FAILURE <br/>
    /// </summary>
    public NodeState Tick()
    {
        if (m_state != NodeState.RUNNING)
            OnEnter();

        m_state = OnTick();

        if (m_state == NodeState.SUCCESS || m_state == NodeState.FAILURE)
            OnExit();

        DebugLog();

        return m_state;
    }

    protected virtual void OnEnter() { }
    /// <summary>
    /// Update & Returns node NodeState after OnTick() has executed
    /// </summary>
    protected virtual NodeState OnTick() => m_state;
    /// <summary>
    /// !!! SHOULD BE USED FOR NODE CLEANUP ONLY !!!<br/><br/>
    /// Will execute uppon SUCCES || FAILURE<br/>
    /// May require [override] regarding specific action for specific m_state
    /// </summary>
    protected virtual void OnExit() { }

    public virtual void SetTree(BehaviourTreeSO tree) => this.tree = tree;

    public virtual Node Copy()
    {
        Node node = Instantiate(this);
        node.name = this.name;
        return node;
    }

    public virtual Node CreateAsset(string path)
    {
        Node clone = Instantiate(this);

        //if (AssetDatabase.LoadAssetAtPath(path + name + ".asset", GetType()) == null) // Use if do not want to override
        {
            //AssetDatabase.CreateAsset(clone, path + name + ".asset");

            AssetDatabase.CreateAsset(clone, path + GetType().BaseType.BaseType + "/" + GetType().BaseType + "/" + name + ".asset");
        }

        //Node node = AssetDatabase.LoadAssetAtPath(path + name + ".asset", GetType()) as Node;
        Node node = AssetDatabase.LoadAssetAtPath(path + GetType().BaseType.BaseType + "/" + GetType().BaseType + "/" + name + ".asset", GetType()) as Node;
        EditorUtility.SetDirty(node);
        return node;
    }

    public virtual void Kill()
    {
        Destroy(this);
    }

    public void DebugLog()
    {
        StaticDebugger.SimpleDebugger(
            tree.Brain.IsDebugOn,
            $"{StaticDebugger.Color(tree.Brain.gameObject.name, cType.OBJECT)} exited {StaticDebugger.Color(name, cType.SCRIPTABLE_OBJECT)}" +
            $" with {StaticDebugger.Color($"{m_state}", cType.ENUM)}"
            );
    }
}
