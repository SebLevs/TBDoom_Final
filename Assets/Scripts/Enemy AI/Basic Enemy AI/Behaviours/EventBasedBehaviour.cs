using UnityEngine;
using UnityEngine.Events;

public class EventBasedBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Tooltip("Warning: Take good consideration of when and if you want to start an animation as " +
             "there is no way to get a nested conditional validation with Events")]
    [Header("Attack Events")]
    [SerializeField] private UnityEvent myEvents;


    // SECTION - Method - Implementation Specific ===================================================================

    public override void Behaviour()
    {
        myContext.CanUseBehaviour = true;
        myEvents.Invoke();
    }

    public override bool ChildSpecificValidations()
    {
        return myContext.IsTargetNear() && myContext.HasReachedEndOfPath;
    }
}
