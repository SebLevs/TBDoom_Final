using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class InteractCanvasHandler : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("Animator")]
    [SerializeField] private Animator anim;
    [SerializeField] private string popUpString;
    [SerializeField] private string popOutString;
    [Range(0.0f, 0.5f)][SerializeField] private float visualCueTimer = 0.1f; // Best result if <= FixedUpdate tick

    [Header("Sprites")]
    [Tooltip("0 = Base | 1 = Valid | 2 = Invalid")]
    [SerializeField] private Sprite[] defaultInteract;
    [Tooltip("0 = Base | 1 = Valid | 2 = Invalid")]
    [SerializeField] private Sprite[] textBubbleInteract;

    private CanvasRenderer canvasRendererBG;
    private CanvasRenderer canvasRendererInteract;

    private Interactable previousInteract = null;

    private Sprite[] desiredSpriteArray;

    // SECTION - Method ===================================================================

    #region Unity
    private void Start()
    {
        //RaycastHit hit = default;
        //SetActive(hit);

        canvasRendererBG = transform.GetChild(0).GetComponent<CanvasRenderer>();
        canvasRendererInteract = transform.GetChild(1).GetComponent<CanvasRenderer>();
    }
    #endregion

    #region Utility
    public void SetActive(RaycastHit hit) // Fade in - Fade out [Set Animator]
    {
        //desiredSpriteArray = (hit.transform != null && hit.transform.CompareTag("TextBubble")) ? textBubbleInteract : defaultInteract;

        if (hit.transform != null)
        {
            var interact = hit.transform.GetComponent<Interactable>();
            if (interact.UseInteractCanvas && !anim.GetCurrentAnimatorStateInfo(0).IsName(popUpString))
                anim.SetTrigger(popUpString);
            else if (previousInteract != null)
            {
                previousInteract.OnInteractionExit();
                previousInteract = interact;
                previousInteract.OnInteractionEnter();
            }
            else if (previousInteract == null)
            {
                previousInteract = interact;
                previousInteract.OnInteractionEnter();
            }
        }
        else if (hit.transform == null)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName(popUpString))
                anim.SetTrigger(popOutString);
            else if (previousInteract != null)
            {
                previousInteract.OnInteractionExit();
                previousInteract = null;
            }
        }

        SetBackgroundSprite(defaultInteract[0]);
        //SetBackgroundSprite(desiredSpriteArray[0]);
    }

    public void SetVisualCue(bool isInteractable = true)
    {
        ///*
        if (isInteractable)
            SetBackgroundSprite(defaultInteract[1]); // Valid
        else
            SetBackgroundSprite(defaultInteract[2]); // invalid
        //*/
        /*
        if (isInteractable)
            SetBackgroundSprite(desiredSpriteArray[1]);
        else
            SetBackgroundSprite(desiredSpriteArray[2]);
        */
        Invoke(nameof(SetDefaultSprite), visualCueTimer);
    }


    // SECTION - Method - Utility ===================================================================
    private void SetBackgroundSprite(Sprite backgroundSprite)
    {
        canvasRendererBG.GetComponent<Image>().sprite = backgroundSprite;
    }

    private void SetDefaultSprite()
    {
        canvasRendererBG.GetComponent<Image>().sprite = defaultInteract[0];
        //canvasRendererBG.GetComponent<Image>().sprite = desiredSpriteArray[0];
    }
    #endregion
}
