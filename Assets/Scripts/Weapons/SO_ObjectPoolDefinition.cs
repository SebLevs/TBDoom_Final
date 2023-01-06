using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/ItemPooling/ItemPoolDefinition", fileName = "SO_ItemPoolDefinition")]
public class SO_ObjectPoolDefinition : ScriptableObject
{
    [SerializeField] private int m_maxRecycledEntities;

    public int MaxRecycledEntities { get => m_maxRecycledEntities; set => m_maxRecycledEntities = value; }
}
