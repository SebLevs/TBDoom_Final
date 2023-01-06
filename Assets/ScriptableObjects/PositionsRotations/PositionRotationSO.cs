using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PositionRotation", fileName = "PositionRotationSO")]
public class PositionRotationSO : ScriptableObject
{
    public Vector3 Position;
    public Quaternion Rotation;
}
