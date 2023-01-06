using UnityEngine;

public class SetStateNearTarget : AbstractBehaviour
{
    // SECTION - Field ============================================================
    [Header("Important Variables")]
    [SerializeField] private BasicEnemy_States nextState;

    private Collider[] hits;


    // SECTION - Method - Unity Specific ============================================================
    private void FixedUpdate()
    {
        //if (isPassive && myBrain.GetCurrentStateHasEnum != nextState && IsExecutionValid())
        if (myContext.GetCurrentStateHasEnum != nextState)
            Execute();
    }


    // SECTION - Method - Implementation Specific ============================================================
    public override void Behaviour()
    {
        if (myContext.IsInAnimationState(BasicEnemy_AnimationStates.IDDLE))
        {
            // Set new NodeState
            if (!myContext.MyLivingEntity.IsDead)
            {
                myContext.SetTransitionAnim();

                myContext.SetFiniteStateMachine(nextState);
            }
        }

        myContext.CanUseBehaviour = true;
        isValidForExecute = false;
    }

    public override bool ChildSpecificValidations()
    {     
        hits = StaticRayCaster.IsOverlapSphereTouching
                    (myContext.transform, distance, targetMask, isDebuggerOn);

        foreach (Collider hit in hits) // May need debug : this.entity takes itself into account even when conditioning against
            if (hit.transform != null && hit.transform.GetInstanceID() != transform.parent.GetInstanceID()) //hit.name != transform.parent.name)
                return true;

        return false;
    }
}
