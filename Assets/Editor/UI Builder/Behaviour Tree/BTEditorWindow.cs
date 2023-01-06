using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using BehaviourTree.Utility.NodeViewSave;

public class BTEditorWindow : EditorWindow
{
    #region Constant variable
    private readonly Vector2 minWindowSize = new Vector2(200.0f, 200.0f);
    private readonly Vector2 miniMapSize = new Vector2(200.0f, 150.0f);

    private const string m_UxmlPath = "Assets/Editor/UI Builder/Behaviour tree/BTEditorWindow.uxml";
    private const string m_UssPath = "Assets/Editor/UI Builder/Behaviour tree/BTEditorWindow.uss";

    private const string ButtonSaveName = "ButtonSave";
    private const string ButtonLoadName = "ButtonLoad";
    #endregion

    private BTGraphView graphView;
    private BTInspectorView inspectorView;

    private BT_UISaveLoad m_saveLoadHandler;
    private Button buttonSave;

    [MenuItem("Behaviour Tree/Editor")]
    public static void OpenWindow()
    {
        GetWindow<BTEditorWindow>(title: "Behaviour tree editor");
    }

    public void OnGUI()
    {
        // Keep MiniMap in view
        SetMiniMapPositionRelative();
    }

    public void CreateGUI()
    {
        minSize = minWindowSize;

        // clone UXML from graphView for Editor Window
        // Query only after this point
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(m_UxmlPath);
        visualTree.CloneTree(rootVisualElement);

        // clone USS from Graphview for Editor Window
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(m_UssPath);
        rootVisualElement.styleSheets.Add(styleSheet);

        // set view references
        graphView = rootVisualElement.Q<BTGraphView>();
        inspectorView = rootVisualElement.Q<BTInspectorView>();
        graphView.OnNodeSelected = OnNodeSelectionChanged;

        // get button
        m_saveLoadHandler = new BT_UISaveLoad(graphView);
        buttonSave = rootVisualElement.Q<Button>(ButtonSaveName);
        buttonSave.clicked += () => m_saveLoadHandler.SaveFromRoot(graphView.TreeNodeView);

        SetMiniMapPositionRelative();

        //SetTreeNodeView(); // TODO: Make it so BTNodeViewTree move to the middle of the screen on create gui
    }

    private void SetMiniMapPositionRelative() // TODO: Make it so the minimap size is relative to the window size
    {
        Vector2 coords = Vector2.zero;
        // Keep minimap on the right side of the screen
        if (graphView != null)
        {
            coords = graphView.WorldToLocal(new Vector2(position.width - 210, 80));
        }

        MiniMap minimap = rootVisualElement.Q<MiniMap>();
        if(minimap != null)
        {
            minimap.SetPosition(new Rect(coords.x, coords.y, miniMapSize.x, miniMapSize.y));
        }
    }

    private void OnNodeSelectionChanged(BTNodeView nodeView)
    {
        inspectorView.UpdateSelection(nodeView);
        Debug.Log($"OnNodeSelectionChanged() for : {nodeView.name}");
    }
}