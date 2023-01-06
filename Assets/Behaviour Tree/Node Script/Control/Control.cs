using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Control : Node
{
    public List<Node> children = new();

    [HideInInspector] public List<Node> Children { get => children; }

    public void SetChildren(List<Node> children) => this.children = children;
    public void SetChild(Node child, int index) => this.children[index] = child;
    public void RemoveChild(Node child) => this.children.Remove(child);
    public void ClearChildren() => this.children.Clear();
    public void AddChild(Node child)
    {
        if (this is Decorator)
            if (children.Count > 0)
                children.RemoveAt(0);

        this.children.Add(child); 
    }

    public override void SetTree(BehaviourTreeSO tree)
    {
        foreach (Node child in children)
            child.SetTree(tree);

        this.tree = tree;
    }

    /// <summary>
    /// Do override in concrete implementations of this type to set variables values if there is any
    /// </summary>
    public override Node Copy()
    {
        Control node = Instantiate(this);
        node.name = this.name;

        for (int index = 0; index < children.Count; index++)
        {
            node.SetChild(Children[index].Copy(), index);
        }

        return node;
    }

    public override Node CreateAsset(string path)
    {
        Control clone = Instantiate(this);
        string trueBaseType = (GetType().ToString().Contains("Control")) ? "Control/" : "Control/" + GetType().ToString() + "/";

        //if (AssetDatabase.LoadAssetAtPath(path + name + ".asset", GetType()) == null) // Use if do not want to override
        {
            AssetDatabase.CreateAsset(clone, path + trueBaseType + name + ".asset");
        }


        Control node = AssetDatabase.LoadAssetAtPath(path + trueBaseType + name + ".asset", GetType()) as Control;//(path + name + ".asset", GetType()) as Control;

        for (int index = 0; index < node.children.Count; index++)
        {
            node.SetChild(Children[index].CreateAsset(path), index);
        }

        EditorUtility.SetDirty(node);
        return node;
    }

    public override void Kill()
    {
        foreach (Node child in children)
            child.Kill();

        Destroy(this);
    }
}
