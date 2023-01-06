using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleAfterQtyOfBehaviour : AbstractBehaviour
{
    // SECTION - Field ===================================================================
    [Header("========== Child Specific ==========")]
    [Header("Action to be performed")]
    [SerializeField] private bool noTokenToggleState = false;
    [SerializeField] private bool noTokenToggleToken = false;
    [SerializeField] private int desiredNoTokenCount = 3;
    private int noTokenCount = 0;

    [Space(10)]
    [SerializeField] private bool hasTokenToggleState = false;
    [SerializeField] private bool hasTokenToggleToken = false;
    [SerializeField] private int desiredTokenCount = 3;
    private int tokenCount = 0;


    // SECTION - Method - Implementation ===================================================================
    public override void Behaviour()
    {
        StartCoroutine(StartBehaviour());
    }

    public override bool ChildSpecificValidations()
    {
        return true;
    }


    // SECTION - Method - Behaviour Specific ===================================================================
    private IEnumerator StartBehaviour()
    {
        // Count update
        if (myContext.HasToken)
            tokenCount++;
        else
            noTokenCount++;

        // Behaviour
        if (noTokenCount >= desiredNoTokenCount)
        {
            ManageToken();
            ManageStateMachine();
            noTokenCount = 0;
        }
        else if (tokenCount >= desiredTokenCount)
        {
            ManageToken();
            ManageStateMachine();

            tokenCount = 0;
        }

        yield return new WaitForSeconds(0.14f);
        myContext.CanUseBehaviour = true;
    }

    public void ManageToken()
    {
        if ((noTokenToggleToken && noTokenCount >= desiredNoTokenCount) || (hasTokenToggleToken && tokenCount >= desiredTokenCount))
        {
            if (myContext.IsBypassTokenSystem)
            {
                if (myContext.HasToken)
                    myContext.HasToken = false;
                else
                    myContext.HasToken = true;
            }
            else
            {
                if (myContext.HasToken)
                    myContext.HasToken = AIManager.instance.MyTokenHandlerSO.ReturnToken(myContext.HasToken);
                else
                    myContext.HasToken = AIManager.instance.MyTokenHandlerSO.TryGetToken(myContext.HasToken);
            }
        }
    }

    public void ManageStateMachine()
    {
        if ((noTokenToggleState && noTokenCount >= desiredNoTokenCount) || (hasTokenToggleState && tokenCount >= desiredTokenCount))
            myContext.ToggleState();
    }
}
