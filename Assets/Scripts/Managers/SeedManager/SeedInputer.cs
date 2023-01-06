using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedInputer : MonoBehaviour
{
    [SerializeField] private RandomSO mySeedSO;
    [SerializeField] private SeedInputerButton[] myButtons;
    private bool isActive = false;

    public bool IsActive { get => isActive; set => isActive = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle()
    {
        isActive = !isActive;
        //mySeedSO.RandomizeOnStart = !isActive;
        if (isActive)
        {
            foreach (SeedInputerButton button in myButtons)
            {
                button.Reset();
            }
        }
        else
        {
            foreach (SeedInputerButton button in myButtons)
            {
                button.MyText.text = "X";
            }
        }
        UpdateSeed();
    }

    public void UpdateSeed()
    {
        int multiplier = 100000;
        int seed = 0;
        for (int i = 0; i < myButtons.Length; i++)
        {
            seed += myButtons[i].Digit * multiplier;
            multiplier /= 10;
        }
        //mySeedSO.Seed = seed;
        //mySeedSO.UpdateSeed();
    }
}
