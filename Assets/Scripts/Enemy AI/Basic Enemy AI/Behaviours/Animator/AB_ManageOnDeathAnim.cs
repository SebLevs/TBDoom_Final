using UnityEngine;

public class AB_ManageOnDeathAnim : StateMachineBehaviour
{
    // SECTION - Field ============================================================
    private LivingEntityContext myLEC;
    private AIBrain myBrain;
    private BasicEnemyContext myBEC; // DEPRECATED


    // SECTION - Method ============================================================
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myLEC = animator.GetComponentInParent<LivingEntityContext>();
        myBrain = animator.GetComponentInParent<AIBrain>();
        myBEC = animator.GetComponentInParent<BasicEnemyContext>(); // DEPRECATED
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        myLEC.AE_ManageObjectAtEndDeathAnim();

        // TODO: May need to add a boolean inside of script if we want to call Drop() as animation event
        if (myBrain != null)
        {
            AIManager.instance.ReturnToken(myBrain);
            myBrain.GetComponentInChildren<DropBehaviour>().Drop();
            Destroy(this);
        }

        if (myBEC && myBEC.enabled) // DEPRECATED
            myBEC.ReturnTokenDelegation();
    }
}
