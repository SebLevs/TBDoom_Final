using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // AssetDatabase
using UnityEngine.UIElements; // UxmlFactory class
using UnityEditor.Experimental.GraphView; // Graphview class
using System;
using System.Linq;

public class BTGraphView : GraphView
{
    #region Node Types
    private const string control = "Control";
    private const string execution = "Execution";
    private const string decorator = "Decorator";
    private const string parallel = "Parallel";
    private const string action = "Action";
    private const string condition = "Condition";
    #endregion

    // Allows to see this.class inside of UI Builder's library
    public new class UxmlFactory : UxmlFactory<BTGraphView, UxmlTraits> { }
    private const string styleSheetPath = "Assets/Editor/UI Builder/Behaviour tree/BTEditorWindow.uss";

    public BTNodeViewTree TreeNodeView { get; set; }

    public Action<BTNodeView> OnNodeSelected;
    private Vector3 _mousePosition;

    public BTSearchWindow _searchWindow;

    public Vector3 MousePosition { get => _mousePosition; }

    public BTGraphView()
    {
        InsertVisualElements();
        AddManipulators();

        InstantiateTreeNode();

        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(styleSheetPath);
        this.styleSheets.Add(styleSheet);

        graphViewChanged = OnGraphViewChanged;

        // Add a callback on Pointer down event
        RegisterCallback<PointerDownEvent>(OnPointerDownEvent, TrickleDown.TrickleDown);

        SetSearchWindow();
    }

    #region Setter_Constructor
    private void InsertVisualElements()
    {
        // background
        this.Insert(0, new GridBackground());

        // miniMap: Position is managed through WindowEditor
        InstantiateMiniMap();

        // toolbar
        //Insert(childCount, InstantiateToolbar()); // TODO: DEPRECATED: DELETE
    }

    private void AddManipulators()
    {
        this.AddManipulator(new ContentZoomer()); // scroll
        this.AddManipulator(new ContentDragger()); // Drag one visual element
        this.AddManipulator(new SelectionDragger()); // Drag a selection of visual elements
        this.AddManipulator(new RectangleSelector()); // Select a zone
    }

    private void InstantiateMiniMap()
    {
        MiniMap minimap = new MiniMap { anchored = true };
        minimap.name = "minimap";
        minimap.SetPosition(new Rect(30, 30, 200, 140));
        minimap.Q<Label>().style.unityTextAlign = TextAnchor.MiddleCenter;
        //minimap.Q<Label>().style.color = Color.white;
        Add(minimap);
    }

    private void SetSearchWindow()
    {
        _searchWindow = ScriptableObject.CreateInstance<BTSearchWindow>();
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        _searchWindow._BTGraphView = this;
    }
    #endregion

    #region Node
    private void InstantiateTreeNode()
    {
        Vector2 nodePos = new Vector2(100, 100); // TODO: Set propet center position on graphView
        TreeNodeView = new BTNodeViewTree(nodePos);
        TreeNodeView.OnNodeSelected = OnNodeSelected;

        AddElement(TreeNodeView);
    }

    public BTNodeViewNode InstantiateNode(Node node, Vector2 position)
    {
        string _cleanName = GetTypeStringUI(node);
        BTNodeViewNode _nodeView = new BTNodeViewNode(node, _cleanName, position);
        _nodeView.OnNodeSelected = OnNodeSelected;
        AddElement(_nodeView);

        return _nodeView;
    }

    public void InstantiateBranch(BTNodeViewNode currentView) // TODO: Finish fixing the instantiation position
    {
        Control _controlNode = currentView.node as Control;

        float x = -200 * (_controlNode.Children.Count - 1);

        for (int i = 0; i < _controlNode.Children.Count; i++)
        {
            // TODO: Find width and height of node (currentView.resolvedStyle.width?)
            Vector2 pos = currentView.Position + new Vector2(x, 250);
            x += 400;
            BTNodeViewNode childNodeView = InstantiateNode(_controlNode.Children[i], pos);
            Add(currentView.Output.ConnectTo(childNodeView.Input));

            // Bug here for child node view as being execution
            if (_controlNode.Children[i] is Control)
            {
                InstantiateBranch(childNodeView);
            }
        }
    }

    /// <summary>
    /// Returns formated child type with space in between each word which starts with an upper character<br/><br/>
    /// BaseType<br/>
    /// CurrentType
    /// <br/><br/>
    /// -Exemple-<br/>
    /// Decorator<br/>
    /// Try N Time
    /// </summary>
    public string GetTypeStringUI(Node node)
    {
        string _cleanName = "";

        int lastIndex = 0;
        List<string> words = new List<string>();

        for (int i = 1; i < node.GetType().ToString().Length; i++)
        {
            if (char.IsUpper(node.GetType().ToString()[i]) || i == node.GetType().ToString().Length - 1)
            {
                int trueI = i == node.GetType().ToString().Length - 1 ? i + 1 : i;
                words.Add(node.GetType().ToString().Substring(lastIndex, trueI - lastIndex));
                lastIndex = i;
            }
        }

        foreach (string word in words)
        {
            _cleanName += (word + " ");
            if (word == node.GetType().BaseType.Name)
            {
                _cleanName += "\n";
            }
        }

        return _cleanName;
    }

    private void OnPointerDownEvent(PointerDownEvent evt)
    {
        _mousePosition = evt.localPosition;
    }

    // Do not delete
    // Must be overriden so that the edges connect from one child to another
    // TODO: May be a bug from Unity: Look for deletion when official system is updated
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction
                                    && endPort.node != startPort.node).ToList();
    }
    #endregion

    #region GraphView events
    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        // On Delete
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(graphElement =>
            {
                if (graphElement is Edge)
                {
                    BTNodeViewNode inputNV = (graphElement as Edge).input.GetFirstOfType<BTNodeViewNode>(); // the deleted child
                    if (inputNV != null)
                    {
                        BTNodeView outputNV = (graphElement as Edge).output.GetFirstOfType<BTNodeView>();

                        if (outputNV is BTNodeViewNode)
                        {
                            ((outputNV as BTNodeViewNode).node as Control).RemoveChild(inputNV.node);
                            (outputNV as BTNodeViewNode).m_children.Remove(inputNV);
                        }
                        else if (outputNV is BTNodeViewTree)
                        {
                            (outputNV as BTNodeViewTree).Tree.SetRoot(null);
                            (outputNV as BTNodeViewTree).RootNodeViewNode = null;
                        }
                    }
                }
            });
        }

        // On Connect
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (Edge edge in graphViewChange.edgesToCreate)
            {
                BTNodeView nodeView = edge.output.node.GetFirstOfType<BTNodeView>();
                if (nodeView is BTNodeViewNode)
                {
                    ((nodeView as BTNodeViewNode).node as Control).AddChild(edge.input.node.GetFirstOfType<BTNodeViewNode>().node);
                    (nodeView as BTNodeViewNode).m_children.Add(edge.input.node.GetFirstOfType<BTNodeViewNode>());
                }
                else if (nodeView is BTNodeViewTree)
                {
                    (nodeView as BTNodeViewTree).Tree.SetRoot(edge.input.node.GetFirstOfType<BTNodeViewNode>().node);
                    (nodeView as BTNodeViewTree).RootNodeViewNode = edge.input.node.GetFirstOfType<BTNodeViewNode>();
                }
            }
        }
        else
        {
            Debug.Log("Edge to connect was null");
        }

        return graphViewChange;
    }
    #endregion

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        evt.menu.AppendAction(actionName: "Focus Tree node", (foo) => FocusTreeNode());
        evt.menu.AppendSeparator();
        base.BuildContextualMenu(evt);
    }

    private void FocusTreeNode()
    {
        viewport.transform.position = TreeNodeView.transform.position;
    }
}
