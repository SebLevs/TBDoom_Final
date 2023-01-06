using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView; // Port refs
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;
using Unity.VisualScripting;
using System.Linq;

public class BTNodeViewNode : BTNodeView
{
    public Node node;
    public List<BTNodeViewNode> m_children = new List<BTNodeViewNode>();

    public BTNodeViewNode(Vector2 position) :base(position) { }
    public BTNodeViewNode(System.Type nodeType, string cleanName, Vector2 position): base(position)
    {
        this.node = ScriptableObject.CreateInstance(nodeType) as Node;
        node.guid = Guid.NewGuid().ToString(); // (Type)AssetDatabase.GUIDToAssetPath(guidString)

        base.SetIcon();
        SetAssetNameReference("");
        if (!(node is Execution))
        {
            this.Q<VisualElement>("node-border").Insert(1, base.AssetName);
        }

        name = cleanName;
        node.name = node.GetType().ToString();
        title = cleanName;

        SetInputPort();
        SetOutputPort();

        m_assetName.RegisterValueChangedCallback(x => node.name = x.newValue.ToString()); // AssetName

        m_nodeInspectorView.UpdateSelection(this);
    }

    public BTNodeViewNode(Node node, string cleanName, Vector2 position) : base(position)
    {
        this.node = node;
        node.guid = Guid.NewGuid().ToString(); // (Type)AssetDatabase.GUIDToAssetPath(guidString)

        base.SetIcon();
        SetAssetNameReference("");
        //if (!(node is Execution))
        {
            this.Q<VisualElement>("node-border").Insert(1, base.AssetName);
        }

        name = cleanName;
        node.name = node.name;
        title = cleanName;

        SetInputPort();
        SetOutputPort();
        m_assetName.RegisterValueChangedCallback(x => node.name = x.newValue.ToString());

        m_assetName.SetValueWithoutNotify(node.name);

        m_nodeInspectorView.UpdateSelection(this);
    }

    #region Setter_Port
    /// <summary>
    /// Output port capacity is assigned based off of the node type
    /// </summary>
    public override void SetOutputPort()
    {
        //Port outputPort = null;
        if (node is Execution)
            return;

        if (node is Decorator)
            Output = GeneratePort(Direction.Output, Port.Capacity.Single);
        else if (node is Control)
            Output = GeneratePort(Direction.Output, Port.Capacity.Multi);

        outputContainer.Add(Output);
    }
    #endregion

    public void AddChild(Node node)
    {
        (node as Control).AddChild(node);
    }
}
