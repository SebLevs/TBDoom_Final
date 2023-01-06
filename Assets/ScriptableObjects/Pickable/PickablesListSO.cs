using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/PickablesList", fileName = "PickablesListSO")]
public class PickablesListSO : ScriptableObject
{
    [SerializeField] private List<PickableSO> pickableSO;

    public List<PickableSO> PickableSO { get => pickableSO; set => pickableSO = value; }
}
