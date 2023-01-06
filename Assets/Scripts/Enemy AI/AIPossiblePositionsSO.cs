using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/AI/AI Possible Positions", fileName = "AI Possible Positions SO")]
public class AIPossiblePositionsSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    [SerializeField] private List<Transform> possiblePositions = new List<Transform>();


    // SECTION - Property ===================================================================
    public List<Transform> PossiblePositions { get => possiblePositions; set => possiblePositions = value; }


    // SECTION - Method ===================================================================
    public Vector3 GetRandomNode()
    {
        return possiblePositions[Random.Range(0, possiblePositions.Count)].position;
    }
}
