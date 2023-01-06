using UnityEngine;
using System;
using Unity.VisualScripting;

[CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Tree", fileName = "TreeSO_NAME")]
public class BehaviourTreeSO : ScriptableObject
{
    [HideInInspector] public string Guid { get; set; }
    [SerializeField] protected Node root = null;

    [SerializeField] public AIBrain Brain { get; set; }
    public bool IsDebugOn { get; set; }
    public Node Root { get => root; }

    public void Tick()
    {
        root.Tick();
    }

    public void SetRoot(Node root) => this.root = root;

    /// <summary>
    /// Recursively set every node's [BehaviourTreeSO.cs] reference to this.class
    /// </summary>
    public void OnAwakeSetter()
    {
        root = root.Copy(); // Copy
        root.SetTree(this);
    }

    public void Kill()
    {
        root.Kill();
        Destroy(this);
    }
}
