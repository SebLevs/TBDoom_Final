using UnityEngine;

public class AB_BasicEnemySetMovementSpeed : StateMachineBehaviour
{
    // SECTION - Field ============================================================
    private BasicEnemyContext myContext;
    [SerializeField] private bool isMaxSpeedOnEnter = false;
    [SerializeField] float speedOnEnterIfNotMax = 0.0f;
    [SerializeField] private bool isMaxSpeedOnExit = true;


    // SECTION - Method ============================================================
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myContext == null)
            myContext = animator.GetComponent<BasicEnemyContext>();

        if (myContext == null || !myContext.enabled)
            return;

        if (isMaxSpeedOnEnter)
            myContext.SetSpeedAsDefault();
        else
            myContext.SetSpeed(speedOnEnterIfNotMax);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myContext == null || !myContext.enabled)
            return;

        if (isMaxSpeedOnExit)
            myContext.SetSpeedAsDefault();
    }
}
