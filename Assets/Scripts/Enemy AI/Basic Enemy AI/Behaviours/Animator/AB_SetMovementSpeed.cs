using UnityEngine;

public class AB_SetMovementSpeed : StateMachineBehaviour
{
    // SECTION - Field ============================================================
    private AIBrain myBrain;
    [SerializeField] private bool isMaxSpeedOnEnter = false;
    [SerializeField] float speedOnEnterIfNotMax = 0.0f;
    [SerializeField] private bool isMaxSpeedOnExit = true;


    // SECTION - Method ============================================================
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myBrain == null)
            myBrain = animator.GetComponent<AIBrain>();

        if (myBrain == null || !myBrain.enabled)
            return;

        if (isMaxSpeedOnEnter)
            myBrain.SetSpeedAsDefault();
        else
            myBrain.SetSpeed(speedOnEnterIfNotMax);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (myBrain == null || !myBrain.enabled)
            return;

        if (isMaxSpeedOnExit)
            myBrain.SetSpeedAsDefault();
    }
}
