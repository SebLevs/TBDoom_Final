using System.Collections.Generic;
using UnityEngine;

/// <NOTE>
///
/// This script suffers from a lack of generic of GenericArrayLinear
///     - TODO:
///         + Create a true generic array linear in the form of a SCRIPTABLE OBJECT -unity hates it, but find a way-
///         + Merge this.script && ShootingRangeArrowUIHandlerBell.cs into one 
///         + Correct ShootingRangeManager && this.script as necessary to implement one getcomponent only...
///             ... or -if generic allows it- plug proper generic array linear into this.script
///             ++ Getcomponent<MyDelegateScriptToScriptable>().StoredReference; could be used -not the best-
///
/// </NOTE>

public class PedestalArrowUIHandler : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Manage If Tag")]
    [SerializeField] protected string triggerTag = "Player";

    [Header("To X UI Elements")]
    [SerializeField] protected GameObject toLeft;
    [SerializeField] protected GameObject toRight;

    private List<GameObject> myInteractablePanels = new List<GameObject>();
    private IArrayLinear availableItemsSO; // TODO: Does not refer to CurrentCount property as it should???
    private PedestalGeneric myPedestal; // TODO: This is temp debug for above


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        SetChildren();

        availableItemsSO = transform.parent.GetComponent<PedestalGeneric>().AvailableItemsSO;
        myPedestal = transform.parent.GetComponent<PedestalGeneric>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Activate pedestal arrows here
        if (other.CompareTag(triggerTag) && myPedestal.GetArrayLinear() != null && myPedestal.GetArrayLinear().Count > 1)
            foreach (GameObject item in myInteractablePanels)
                item.SetActive(true);

        SetToUIVisual(isRight: false);
        SetToUIVisual(isRight: true);
    }


    private void OnTriggerExit(Collider other)
    {
        // deactivate pedestal arrows here
        if (other.CompareTag(triggerTag))
            foreach (GameObject item in myInteractablePanels)
                item.SetActive(false);
    }


    // SECTION - Method - Utility Specific ===================================================================
    #region REGION - UI Management
    protected void SetChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
            myInteractablePanels.Add(transform.GetChild(i).gameObject);
    }

    public void SetToUIVisual(bool isRight)
    {
        if (myPedestal.GetArrayLinear() == null)
        {
            toLeft.SetActive(false);
            toRight.SetActive(false);
            return;
        }

        if (!isRight)     // <-
        {
            if (myPedestal.GetArrayLinear().CurrentIndex == 0)                                 // At min index
                toLeft.SetActive(false);
            if (myPedestal.GetArrayLinear().CurrentIndex == myPedestal.GetArrayLinear().Count - 2) // At max index
                toRight.SetActive(true);
        }
        else if (isRight) // ->
        {
            if (myPedestal.GetArrayLinear().CurrentIndex == myPedestal.GetArrayLinear().Count - 1)     // At max index
                toRight.SetActive(false);
            if (myPedestal.GetArrayLinear().CurrentIndex == 1)                           // At min index
                toLeft.SetActive(true);
        }
    }
    #endregion
}
