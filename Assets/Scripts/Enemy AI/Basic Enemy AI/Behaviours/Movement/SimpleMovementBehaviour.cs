using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

public class SimpleMovementBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("=============== Child Specific ===============")]
    [Header("Base PathFinding Infos")]
    [SerializeField] private AIPossiblePositionsSO myAPPSO;

    [Header("Next position modifiers")]
    [SerializeField] private float getPosRadiusModifier = 2.56f;
                     private const float moveMinimumDistance = 0.96f;


    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        StartCoroutine(StartBehaviour());
    }

    public override bool ChildSpecificValidations()
    {
        if (!myContext.CanUseBehaviour)
            return false;

        return true;
    }


    // SECTION - Method - Utility Specific ===================================================================
    private IEnumerator StartBehaviour()
    {
        // Set target as [myTemporaryTarget]
        myContext.SetTarget();

        // Set [MyTemporaryTarget]'s position
        if (myAPPSO)
            SetTargetWithRandomPoint(PickRandomPoint(myAPPSO.GetRandomNode()));
        else
            SetTargetWithRandomPoint(PickRandomPoint(myContext.transform.position));

        yield return new WaitUntil(() => (!myContext.HasPath || myContext.HasReachedEndOfPath));

        myContext.CanUseBehaviour = true;
    }

    private Vector3 PickRandomPoint(Vector3 fromPosition)
    {
        float x = moveMinimumDistance * ((UnityEngine.Random.Range(-1, 1) < 0) ? -1 : 1);
        float z = moveMinimumDistance * ((UnityEngine.Random.Range(-1, 1) < 0) ? -1 : 1);
        Vector3 addedDistance = new (x, 0.0f, z);

        Vector3 point = Random.insideUnitSphere * getPosRadiusModifier + addedDistance;
        point.y = 0;
        point += fromPosition;

        return point;
    }

    private void SetTargetWithRandomPoint(Vector3 towardsPosition)
    {
        GraphNode toNode = null;
        GraphNode fromNode = null;

        toNode = AstarPath.active.graphs[(int)myContext.Type].GetNearest(towardsPosition, myContext.Constraint).node;
        fromNode = AstarPath.active.graphs[(int)myContext.Type].GetNearest(myContext.transform.position, myContext.Constraint).node;

        if (PathUtilities.IsPathPossible(fromNode, toNode))
        {
            myContext.SetEndReachedDistance(distance); // Set here to avoid flooding system with multiple set if no path found
            myContext.SetMyTemporaryTargetAs((Vector3)toNode.position);   
        }

    }
}