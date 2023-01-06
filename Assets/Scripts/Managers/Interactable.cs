using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Interactable : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private bool isInteractable = true;
    [SerializeField] private bool useInteractCanvas = true;
    public UnityEvent interacted;
    public UnityEvent interactionEnter;
    public UnityEvent interactionExit;


    // SECTION - Field ===================================================================
    public bool IsInteractable { get => isInteractable; }
    public bool UseInteractCanvas { get => useInteractCanvas; set => useInteractCanvas = value; }


    // SECTION - Method - Main ===================================================================
    public void OnInteraction()
    {
        if(isInteractable)
            interacted.Invoke();
    }

    public void OnInteractionEnter()
    {
        interactionEnter.Invoke();
    }

    public void OnInteractionExit()
    {
        interactionExit.Invoke();
    }

    // // SECTION - Method - Utility ===================================================================
    public void ToggleIsInteractable()
    {
        isInteractable = !isInteractable;
    }

    public void SetIsInteractable(bool setAs)
    {
        isInteractable = setAs;
    }

    public void ToggleInteractableLayer()
    {
        // 128 == 010000000 
        string interactableBinary = Convert.ToString(GameManager.instance.interactableMask.value, 2);
        string currBinary = Convert.ToString(LayerMask.GetMask(LayerMask.LayerToName(gameObject.layer)), 2);

        // Prevent/Enable Interact popup window for this.gameObject
        if (currBinary == interactableBinary)
            gameObject.layer = 0; //LayerMask.GetMask("Default");
        else
            gameObject.layer = 7; //GameManager.instance.interactableMask; (threw bug for some reason?)
    }

    public void SetInteractableLayer(bool isInteractable)
    {
        if (isInteractable)
            gameObject.layer = 7; //GameManager.instance.interactableMask;
        else
            gameObject.layer = 0; //LayerMask.GetMask("Default"); (threw bug for some reason?)
    }
}
