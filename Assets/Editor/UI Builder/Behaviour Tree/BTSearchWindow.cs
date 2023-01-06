using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using UnityEngine.UIElements;

public class BTSearchWindow : ScriptableObject, ISearchWindowProvider
{
    string[] _doNotCreateBaseTypeOf =
{
        "Decorator",
        "Execution",
        "Condition",
        "Action"
    };

    public BTGraphView _BTGraphView { get; set; }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            // Base
            new SearchTreeGroupEntry(new GUIContent("Create or load node"), 0), // window title
            new SearchTreeGroupEntry(new GUIContent("Node default values"), 1),
        };

        tree.Add(new SearchTreeGroupEntry(new GUIContent("Node Concrete implementation"), 1));

        // Control
        tree.Add(new SearchTreeGroupEntry(new GUIContent("Control"), 2));
        LoadAllNodeOfType(typeof(Sequence)).ForEach(x => tree.Add(x));// Sequence 
        LoadAllNodeOfType(typeof(Fallback)).ForEach(x => tree.Add(x));// Fallback
        LoadAllNodeOfType(typeof(ParallelAsynchronous)).ForEach(x => tree.Add(x));// Parallel ASYNC
        LoadAllNodeOfType(typeof(Decorator)).ForEach(x => tree.Add(x));// Decorator

        // Execution
        tree.Add(new SearchTreeGroupEntry(new GUIContent("Execution"), 2));
        LoadAllNodeOfType(typeof(Condition)).ForEach(x => tree.Add(x));// Condition
        LoadAllNodeOfType(typeof(Action)).ForEach(x => tree.Add(x));// Action

        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        if (SearchTreeEntry.userData.GetType() == null)
        {
            return false;
        }

        Node _parentNode = (SearchTreeEntry.userData as Node).Copy();
        BTNodeViewNode UINode = _BTGraphView.InstantiateNode(_parentNode, _BTGraphView.MousePosition); //context.screenMousePosition);

        if (_parentNode is Control)
        {
            _BTGraphView.InstantiateBranch(UINode);
        }
        
        return true;
    }

    private List<SearchTreeEntry> LoadAllNodeOfType(System.Type type)
    {
        Node[] nodes = Resources.LoadAll<Node>("Behaviour Tree/Node/" + type.BaseType.ToString() + "/" + type.ToString());

        List<SearchTreeEntry> treeEntries = new List<SearchTreeEntry>();

        // Group Entry
        treeEntries.Add(new SearchTreeGroupEntry(new GUIContent(type.ToString()), 3));

        // Base node
        if (!_doNotCreateBaseTypeOf.Contains(type.ToString()))
        {
            SearchTreeEntry searchTreeEntry = new SearchTreeEntry(new GUIContent(type.ToString())) // Template node
            {
                userData = CreateInstance(type) as Node,
                level = 4
            };

            (searchTreeEntry.userData as Node).name = type.ToString();

            treeEntries.Add(searchTreeEntry);
        }

        // Other nodes
        foreach (Node node in nodes)
        {
            treeEntries.Add(new SearchTreeEntry(new GUIContent(node.name))
            {
                userData = node,
                level = 4
            });
        }

        return treeEntries;
    }
}
