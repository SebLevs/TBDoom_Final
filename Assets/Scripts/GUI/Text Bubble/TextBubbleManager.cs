using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class TextBubbleManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Tooltip("Whenever no trigger zone is used to [SetActive()] the Text Bubbles")]
    [SerializeField] private bool isAlwaysActive = false;
    
    [Header("Trigger Zone (Active/Inactive)")]
    [Range(0, 15)]
    [SerializeField] private float triggerSizeX = 1.0f;
    [Range(0, 15)]
    [SerializeField] private float triggerSizeY = 3.0f;
    [Range(0, 15)]
    [SerializeField] private float triggerSizeZ = 3.0f;


    private List<TextBubble> myTextBubbles = new List<TextBubble>();
    private BoxCollider myCollider;

    private readonly string TBMSingleTag = "TBM Single";


    // SECTION - Property ===================================================================
    #region Property
    public float TriggerSizeX { get => triggerSizeX; }
    public float TriggerSizeY { get => triggerSizeY; }
    public float TriggerSizeZ { get => triggerSizeZ; }
    public Interactable MyInteractable { get; set; }
    public List<TextBubble> MyTextBubbles { get => myTextBubbles; }

    public bool IsSingleTextBubbleManager => CompareTag(TBMSingleTag);
    #endregion


    // SECTION - Method ===================================================================
    #region Unity
    private void Start()
    {
        MyInteractable = GetComponentInChildren<Interactable>();
        myCollider = GetComponent<BoxCollider>();

        SetDefaultText();

        SetVariables();

        SetActiveAll(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetActiveAll(true);

            Print();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            MyInteractable.SetInteractableLayer(false);

            foreach (TextBubble myTextBubble in myTextBubbles)
            {
                myTextBubble.StopAllCoroutines();
                myTextBubble.SetPage(0); // TODO: Reset of page could be set as a bool instead
                myTextBubble.SetDefaultTextColor();
            }

            SetActiveAll(false);
        }
    }
    #endregion

    #region Setter
    public void SetActiveAll(bool isActive)
    {
        foreach (TextBubble textBubble in myTextBubbles)
        {
            GameObject trueObject = (IsSingleTextBubbleManager) ? textBubble.gameObject : 
                                                                  textBubble.transform.parent.gameObject;

            if (isAlwaysActive && !textBubble.gameObject.activeSelf)
                trueObject.SetActive(true);
            else if (!isAlwaysActive)
            {
                trueObject.SetActive(isActive);
                if (IsSingleTextBubbleManager)
                    trueObject.transform.parent.GetChild(0).gameObject.SetActive(isActive); // Set Interactable & Background
            }
                
        }
    }

    public void SetVariables()
    {
        SetSingleManagerBackground();

        SetTriggers();

        // Text
        //foreach (TextBubble textBubble in myTextBubbles)
            //textBubble.SetDefaultVariables();
    }

    private void SetTriggers()
    {
        myCollider.size = new Vector3(triggerSizeX, triggerSizeY, triggerSizeZ);

        float sizeX = 0, sizeY = 0;

        foreach (TextBubble textBubble in myTextBubbles)
        {
            sizeX = textBubble.MyParentRectTransform.sizeDelta.x > sizeX ? textBubble.MyParentRectTransform.sizeDelta.x : sizeX;
            sizeY = textBubble.MyParentRectTransform.sizeDelta.y > sizeY ? textBubble.MyParentRectTransform.sizeDelta.y : sizeY;
        }

        MyInteractable.GetComponent<BoxCollider>().size = new Vector3(sizeX, sizeY, 0.0f);
    }

    public void SetSingleManagerBackground(bool isInspector = false)
    {
        if (!IsSingleTextBubbleManager) return;

        MyInteractable = MyInteractable == null ? GetComponentInChildren<Interactable>() : MyInteractable;

        float width = 0;
        float height = 0;

        if (!isInspector)
            foreach (TextBubble textBubble in myTextBubbles)
            {
                //if (!textBubble.gameObject.activeSelf) continue;

                float textBubbleDeltaX = textBubble.MyRectTransform.sizeDelta.x;
                width = textBubbleDeltaX > width ? textBubbleDeltaX : width;

                height += textBubble.MyRectTransform.sizeDelta.y;
            }
        else
        {
            TextBubble[] myTextBubblesArray = GetComponentsInChildren<TextBubble>();
            foreach (TextBubble textBubble in myTextBubblesArray)
            {
                //if (!textBubble.gameObject.activeSelf) continue;

                float textBubbleDeltaX = textBubble.GetComponent<RectTransform>().sizeDelta.x;
                width = textBubbleDeltaX > width ? textBubbleDeltaX : width;

                height += textBubble.GetComponent<RectTransform>().sizeDelta.y;
            }
        }

        MyInteractable.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        MyInteractable.GetComponent<BoxCollider>().size = new Vector3(width, height, width);
    }

    public void SetDefaultText()
    {
        foreach (TextBubble textBubble in myTextBubbles)
            textBubble.SetDefaultTextString();
    }
    #endregion

    #region Utility
    public void Print(string textToPrint = " ")
    {
        foreach (TextBubble textBubble in myTextBubbles)
            textBubble.Print(textToPrint);
    }

    public void SetPages(int page)
    {
        foreach (TextBubble textBubble in myTextBubbles)
            textBubble.SetPage(page);
    }
    #endregion

    #region Observer Pattern
    public void Subscribe(TextBubble subscriber)
    {
        myTextBubbles.Add(subscriber);
    }

    public void UnSubscribe(TextBubble subscriber)
    {
        myTextBubbles.Remove(subscriber);
    }

    public void Notify()
    {
        foreach (TextBubble textBubble in myTextBubbles)
            textBubble.Notify();
    }
    #endregion
}
