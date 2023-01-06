using UnityEditor;
using UnityEngine;

namespace BehaviourTree.Utility.NodeViewSave
{
    public class BT_UISaveLoad
    {
        // TODO: Refactor into a popup window to select folder and return path into a single string
        private const string m_nodeFolderPath = "Assets/Resources/Behaviour Tree/Node/";

        private BTGraphView m_graphView;

        public BT_UISaveLoad(BTGraphView graphView)
        {
            m_graphView = graphView;
        }

        public void SaveFromRoot(BTNodeViewTree nodeViewTree)
        {
            if (nodeViewTree.Tree.Root == null) 
            {
                EditorUtility.DisplayDialog("Tree root null", 
                    "A root is required to save from the tree\n" +
                    "Solutions:\n" +
                    "1 - Connect a node to the tree\n" +
                    "2 - Save a branch by right clicking on a node and selecting \"Save branch\"", 
                    "OK");
                return; 
            }

            if (!AssetDatabase.IsValidFolder(m_nodeFolderPath))
            {
                EditorUtility.DisplayDialog("Path does not exist",
                    "The [m_nodeFolderPath] path of the script BT_UISaveLoad.cs does not exist\n" +
                    $"{m_nodeFolderPath}",
                    "OK");
                return;
                //AssetDatabase.CreateFolder(path, name of folder);
            }

            Node rootCopy = nodeViewTree.RootNodeViewNode.node.CreateAsset(m_nodeFolderPath);
            BehaviourTreeSO tree = ScriptableObject.CreateInstance<BehaviourTreeSO>();
            tree.SetRoot(rootCopy);
            tree.name = m_graphView.TreeNodeView.Tree.name;
            AssetDatabase.CreateAsset(tree, m_nodeFolderPath + "Tree/" + tree.name + ".asset");

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
