using UnityEngine;
using UnityEditor.Experimental.GraphView; // Port refs
using System;
using UnityEngine.UIElements;

public class BTNodeViewTree : BTNodeView
{
    public BehaviourTreeSO Tree { get; set; }

    public BTNodeViewNode RootNodeViewNode { get; set; }

    public BTNodeViewTree(Vector2 position) :base(position)
    {
        //this.AddManipulator(new ClickSelector());
        //titleContainer.GetFirstOfType<Label>().visible = false;

        Tree = ScriptableObject.CreateInstance("BehaviourTreeSO") as BehaviourTreeSO;
        Tree.Guid = Guid.NewGuid().ToString(); // (Type)AssetDatabase.GUIDToAssetPath(guidString)
        name = "Tree";
        Tree.name = name;
        title = "Tree";

        base.SetIcon();
        SetAssetNameReference("");
        this.Q<VisualElement>("node-border").Insert(1, base.AssetName);

        SetOutputPort();

        // You shall not move!
        base.capabilities &= ~Capabilities.Movable;
        base.capabilities &= ~Capabilities.Deletable;
        AddToClassList("anchored");

        m_nodeInspectorView.UpdateSelection(this);

        m_assetName.RegisterValueChangedCallback(x => Tree.name = x.newValue.ToString()); // AssetName
    }

    #region Setter_Port
    /// <summary>
    /// Output port capacity is assigned based off of the node type
    /// </summary>
    public override void SetOutputPort()
    {
        Output = GeneratePort(Direction.Output, Port.Capacity.Single);

        outputContainer.Add(Output);
    }
    #endregion

    public void SetRootNode(Node node)
    {
        Tree.SetRoot(node);
    }
}
