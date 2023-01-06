using UnityEditor;
using UnityEngine;

/// <summary>
/// Empty class<br/>
/// Must define a body at concretisation<br/><br/>
/// 
/// Proposed [CreateAssetMenu] nomenclature<br/>
/// [CreateAssetMenu(menuName = "Scriptable/Behaviour Tree/Node Control/Decorator/NAME", fileName = "DecoratorSO_TYPE_NAME")]
/// </summary>
public class Decorator : Control 
{
    public override Node CreateAsset(string path)
    {
        Control clone = Instantiate(this);
        string trueBaseType = "Control/Decorator/";

        //if (AssetDatabase.LoadAssetAtPath(path + name + ".asset", GetType()) == null) // Use if do not want to override
        {
            AssetDatabase.CreateAsset(clone, path + trueBaseType + name + ".asset");
        }

        Control node = AssetDatabase.LoadAssetAtPath(path + trueBaseType + name + ".asset", GetType()) as Control;//(path + name + ".asset", GetType()) as Control;

        node.SetChild(Children[0].CreateAsset(path), 0);

        EditorUtility.SetDirty(node);
        return node;
    }
}
