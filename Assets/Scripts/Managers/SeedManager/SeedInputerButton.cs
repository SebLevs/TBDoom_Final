using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedInputerButton : MonoBehaviour
{
    [SerializeField] private bool isActivationButton = false;
    [SerializeField] private TextMeshPro myText;

    private int digit = 0;
    private SeedInputer mySeedInputer;

    public TextMeshPro MyText { get => myText; set => myText = value; }
    public int Digit { get => digit; set => digit = value; }

    // Start is called before the first frame update
    void Start()
    {
        mySeedInputer = GetComponentInParent<SeedInputer>();
        if (isActivationButton)
        {
            myText.text = "X";
        }
        else
        {
            myText.text = "X";
        }
    }

    public void Reset()
    {
        digit = 0;
        myText.text = digit.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateButton()
    {
        if (isActivationButton)
        {
            mySeedInputer.Toggle();
            if (mySeedInputer.IsActive)
            {
                myText.text = "S";
            }
            else
            {
                myText.text = "X";
            }
        }
        else
        {
            if (mySeedInputer.IsActive)
            {
                digit++;
                if (digit > 9)
                {
                    digit = 0;
                }
                myText.text = digit.ToString();
                mySeedInputer.UpdateSeed();
            }
        }
    }
}
