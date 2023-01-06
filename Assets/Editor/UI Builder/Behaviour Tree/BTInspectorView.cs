using UnityEditor; // Editor instance
using UnityEngine.UIElements; // TwoPaneSplitView class

public class BTInspectorView : VisualElement
{
    // Allows to see this.class inside of UI Builder's library
    public new class UxmlFactory : UxmlFactory<BTInspectorView, UxmlTraits> { }

    // Allows to print a class' data
    private Editor editor;

    public BTInspectorView() { }

    internal void UpdateSelection(BTNodeView nodeView)
    {
        //Debug.Log($"UpdateSelection -inspectorview-: Type: {nodeView.GetType()}");
        Clear(); // Clear this.contentContainer

        if (nodeView is BTNodeViewNode)
        {
            editor = Editor.CreateEditor((nodeView as BTNodeViewNode).node);
        }
        else
        {
            editor = Editor.CreateEditor((nodeView as BTNodeViewTree).Tree);
        }

        IMGUIContainer container = new IMGUIContainer(() => { editor.OnInspectorGUI(); });
        Add(container);
    }
}
