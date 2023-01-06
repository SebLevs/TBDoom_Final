using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiPositioning : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private List<Transform> myNodes;
    [SerializeField] private AIPossiblePositionsSO myReferenceSO;


    // SECTION - Property ===================================================================
    public AIPossiblePositionsSO PossiblePositionsSO { get => myReferenceSO; }


    // SECTION - Method ===================================================================
    private void Start()
    {
        myReferenceSO.PossiblePositions.Clear();
        myReferenceSO.PossiblePositions = myNodes;
    }

    private void Update()
    {
        if (myNodes == null || myNodes.Count == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                myNodes.Add(transform.GetChild(i).transform);
            }
        }

        if (myReferenceSO.PossiblePositions[0] == null)
        {
            PossiblePositionsSO.PossiblePositions.Clear();
            PossiblePositionsSO.PossiblePositions = myNodes;
        }

    }
}
