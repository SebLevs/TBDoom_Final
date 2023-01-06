using UnityEngine;
using System;
using UnityEditor.Experimental.GraphView; // Port refs
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;

public abstract class BTNodeView : UnityEditor.Experimental.GraphView.Node
{
    #region TODO: Move inside of BlackBoard
    private Color bgColorGrey = new Color(0.7686f, 0.7686f, 0.7686f, 1.0f); // TODO: Should be inside of blackboard
    private Color bgColorBlack = new Color(0.1764f, 0.1764f, 0.1764f, 1.0f); // TODO: Should be inside of blackboard

    private static string iconFolder = "Behaviour tree/";
    private const float iconWidthHeight = 32;
    private Dictionary<System.Type, string> dictionaryIcon = new Dictionary<Type, string>
    {
        {typeof(BTNodeViewTree), iconFolder + "Icon_Tree"},

        {typeof(Sequence), iconFolder + "Icon_Sequence"},
        {typeof(Fallback), iconFolder + "Icon_Fallback"},
        {typeof(Decorator), iconFolder + "Icon_Decorator"},
        {typeof(ParallelAsynchronous), iconFolder + "Icon_Parallel"},

        {typeof(Condition), iconFolder + "Icon_Condition"},
        {typeof(Action), iconFolder + "Icon_Action"},
    };
    #endregion
    public Port Input { get; set; }
    public Port Output { get; set; }

    public Vector2 Position { get; set; }

    protected TextField m_assetName;

    public Action<BTNodeView> OnNodeSelected;
    public BTInspectorView m_nodeInspectorView;

    public TextField AssetName { get => m_assetName; }

    public BTNodeView(Vector2 position)
    {
        Position = position;
        
        SetPosition(new Rect(Position.x, Position.y, 100, 100));

        // Title
        this.Q<Label>("title-label").style.unityTextAlign = TextAnchor.MiddleCenter;

        // Custom asset name reference field
        m_assetName = new TextField();
        m_assetName.style.alignSelf = Align.Center;
        m_assetName.style.justifyContent = Justify.Center;

        // Inspector view
        m_nodeInspectorView = new BTInspectorView();
        m_nodeInspectorView.style.backgroundColor = bgColorBlack;
        topContainer.Insert(1, m_nodeInspectorView);
        //extensionContainer.Add(m_nodeInspectorView); // TODO: Location for colapsable area
        //m_nodeInspectorView.UpdateSelection(this);
    }

    // called automatically by system
    public override void OnSelected() 
    {
        base.OnSelected();
        if (OnNodeSelected != null)
            OnNodeSelected.Invoke(this);
    }

    #region Setter_Port
    /// <summary>
    /// Input port capacity -default- is one <br/>
    /// May be set manually, if need be
    /// </summary>
    public virtual void SetInputPort(Port.Capacity capacity = Port.Capacity.Single)
    {
        Input = GeneratePort(Direction.Input, capacity);
        inputContainer.Add(Input);
    }

    public abstract void SetOutputPort();

    protected Port GeneratePort(Direction portDirection, Port.Capacity capacity)
    {
        Port port = InstantiatePort(Orientation.Vertical, portDirection, capacity, typeof(bool));
        port.portName = ""; // default name printed == "Single"
        return port;
    }
    #endregion

    #region Setter_VisualElement
    protected void SetIcon()
    {
        //Texture2D m_icon = Resources.Load<Texture2D>(iconFolder + "Icon_Fallback");//GetIconTexture(); // TODO: String should be inside of blackboard
        System.Type m_iconType = GetIconType();

        Texture2D m_icon = Resources.Load<Texture2D>(dictionaryIcon[m_iconType]);//GetIconTexture(); // TODO: String should be inside of blackboard
        VisualElement veIcon = new VisualElement();
        veIcon.style.backgroundColor = bgColorGrey;
        veIcon.style.backgroundImage = m_icon;
        veIcon.style.width = iconWidthHeight;
        veIcon.style.height = iconWidthHeight;
        titleContainer.Insert(0, veIcon);
    }

    private System.Type GetIconType()
    {
        System.Type m_iconType;
        if (this is BTNodeViewNode)
        {
            m_iconType = (this as BTNodeViewNode).node.GetType();

            if ((this as BTNodeViewNode).node is Decorator)
                m_iconType = typeof(Decorator);
            else if ((this as BTNodeViewNode).node is Execution)
                m_iconType = (this as BTNodeViewNode).node.GetType().BaseType;
        }
        else
            m_iconType = GetType();

        return m_iconType;
    }

    public void SetAssetNameReference(string value)
    {
        string name = (value.Length > 2)? value: GetNodeTypeToString() + "_nodeName";
        m_assetName.SetValueWithoutNotify(name);
    }
    #endregion

    #region Getter
    protected Texture2D GetIconTexture2D()
    {
        return Resources.Load<Texture2D>(iconFolder + GetNodeTypeToString());
    }

    private string GetNodeTypeToString()
    {
        if (this is BTNodeViewNode)
        {
            return (this as BTNodeViewNode).node.GetType().ToString();
        }

        return (this as BTNodeViewTree).Tree.GetType().ToString();
    }
    #endregion

    #region Contextual menu
    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.AppendAction(actionName: "Select all children", action: (foo) => SelectAllChildren());
        evt.menu.AppendAction(actionName: "Delete graph children", action: (foo) => DeleteGraphChildren());
        evt.menu.AppendAction(actionName: "Save branch", action: (foo) => SaveBranch()); // TODO: May need to make it into action and assign through instantiation

        evt.menu.AppendSeparator();
    }

    protected void SelectAllChildren()
    {

    }

    protected void DeleteGraphChildren()
    {
        // TODO: Implement recursive child deletion of BTNodeViewNode
    }

    protected void SaveBranch()
    {

    }
    #endregion
}
