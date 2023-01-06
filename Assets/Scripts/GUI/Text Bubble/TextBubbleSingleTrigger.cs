using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class TextBubbleSingleTrigger : MonoBehaviour
{
    // SECTION - Field ===================================================================
    private BoxCollider mySingleBoxCollider;
    private List<TextBubbleManager> myTextBubbles = new List<TextBubbleManager>();


    // SECTION - Method ===================================================================
    private void Awake()
    {
        mySingleBoxCollider = GetComponent<BoxCollider>();

        TextBubbleManager temp;
        float x = mySingleBoxCollider.size.x;
        float y = mySingleBoxCollider.size.y;
        float z = mySingleBoxCollider.size.z;

        for (int i = 0; i < transform.childCount; i++)
        {
            temp = transform.GetChild(i).GetComponent<TextBubbleManager>();
            if (temp)
            {
                myTextBubbles.Add(temp);
                x = (temp.TriggerSizeX > mySingleBoxCollider.size.x) ? temp.TriggerSizeX : mySingleBoxCollider.size.x;
                y = (temp.TriggerSizeY > mySingleBoxCollider.size.y) ? temp.TriggerSizeY : mySingleBoxCollider.size.y;
                z = (temp.TriggerSizeZ > mySingleBoxCollider.size.z) ? temp.TriggerSizeZ : mySingleBoxCollider.size.z;
            }
            BoxCollider tempBoxCollider = temp.GetComponent<BoxCollider>();
            if (tempBoxCollider)
                tempBoxCollider.enabled = false;
        }

        mySingleBoxCollider.size = new Vector3(x, y, z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            foreach (TextBubbleManager textBubbleManager in myTextBubbles)
            {
                textBubbleManager.SetActiveAll(true);

                textBubbleManager.Print();
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (TextBubbleManager textBubbleManager in myTextBubbles)
            {
                textBubbleManager.MyInteractable.SetInteractableLayer(false);

                foreach (TextBubble myTextBubble in textBubbleManager.MyTextBubbles)
                {
                    myTextBubble.StopAllCoroutines();
                    myTextBubble.SetPage(0); // TODO: Reset of page could be set as a bool instead
                    myTextBubble.SetDefaultTextColor();
                }

                textBubbleManager.SetActiveAll(false);
            }
        }
    }
}
