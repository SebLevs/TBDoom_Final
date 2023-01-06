using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/Variable/String", fileName = "SO_NAME")]
public class StringVariable : ScriptableObject
{
    [TextArea(1, 100)]
    public string Value;
}
