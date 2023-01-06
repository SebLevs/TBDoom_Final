using UnityEngine;

public class AB_BasicEnemyToggleStateAtEndAttack : StateMachineBehaviour
{
    // SECTION - Field ============================================================
    private BasicEnemyContext myContext;


    // SECTION - Method ============================================================
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get BasicEnemyContext
        if (myContext == null)
            myContext = animator.GetComponent<BasicEnemyContext>();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Toggle handler

        if (myContext.CurrState is BasicEnemyState_Two && myContext.To_1_OnAtkExit)
            myContext.ToggleState();
        else if (myContext.CurrState is BasicEnemyState_One && myContext.To_2_OnAtkExit)
            myContext.ToggleState();
    }
}
