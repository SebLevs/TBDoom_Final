using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 
/// Any behaviour that modify character vertices (color, shape or otherwise) from tmp mesh MUST 
/// declare eitherone or both of the following:<br/>
/// <br/>
/// myTextMeshPro.ForceMeshUpdate(); // Before modifications<br/>
/// myTextMeshPro.canvasRenderer.SetMesh(myTextMeshPro.mesh); // After modifications
/// 
/// </summary>

public class TextBubble : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Header("General Text Bubble Informations")]
    [SerializeField] private TextBubbleSO textBubbleSO;

    [Header("Modifiables")]
    [SerializeField] private UnityEvent myTextModifiers;

    [Header("Text Bubble Variables")]
    [ContextMenuItem("Set text bubbles variables", nameof(SetDefaultVariables_Inspector))]
    [Tooltip("MB2 > Set text bubbles variables\nSet rects and textMeshPro's through inspector")]
    [SerializeField] private Vector2 rectSizes = new(0.64f, 0.64f);

    [Header("Text to print")]
    [SerializeField] private StringReference myText;

    private TextBubbleManager myBubbleManager;

    private TextMeshProUGUI myTextMeshPro;
    private RectTransform myRectTransform;
    private RectTransform myParentRectTransform; // Used for background image quick access
    private Color[] charsColors;


    // SECTION - Property ===================================================================
    public RectTransform MyRectTransform { get => myRectTransform; }
    public RectTransform MyParentRectTransform { get => myParentRectTransform; }

    public string CurrentText { get => myTextMeshPro.text; }


    // SECTION - Method ===================================================================
    #region Unity
    private void Awake()
    {
        SetComponentReferences();

        SetDefaultVariables();
        myBubbleManager.Subscribe(this);
    }
    #endregion

    #region Getter
    public int GetPageAtCharacter(int index)
    {
        return myTextMeshPro.textInfo.characterInfo[index].pageNumber;
    }

    public char GetCharacter(int index)
    {
        return myTextMeshPro.textInfo.characterInfo[index].character;
    }

    public bool GetIsCharacterVisible(int index)
    {
        return myTextMeshPro.textInfo.characterInfo[index].isVisible;
    }

    /// <summary>
    /// Get EVERY vertices' color value of the text
    /// </summary>
    public Color[] GetColors()
    {
        myTextMeshPro.ForceMeshUpdate();
        return myTextMeshPro.mesh.colors;
    }
    #endregion

    #region Setter _ Component & Default Variable
    private void SetComponentReferences()
    {
        myBubbleManager = GetComponentInParent<TextBubbleManager>();
        myParentRectTransform = transform.parent.GetComponentInParent<RectTransform>();
        myTextMeshPro = GetComponent<TextMeshProUGUI>();
        myRectTransform = GetComponent<RectTransform>();
    }

    public void SetDefaultVariables()
    {
        SetColorsRef();

        // Font
        myTextMeshPro.fontSizeMin = textBubbleSO.FontSize.Value;

        // Color
        SetDefaultTextColor();

        // Margins
        float marginX = textBubbleSO.MarginSizesModifier * rectSizes.x;
        float marginY = textBubbleSO.MarginSizesModifier * rectSizes.y;
        myTextMeshPro.margin = new Vector4(marginX, marginY, marginX, marginY);

        // Spacing Character
        myTextMeshPro.characterSpacing = textBubbleSO.CharacterSpacing.Value;

        // Spacing Line
        myTextMeshPro.lineSpacing = textBubbleSO.LineSpacing.Value;

        // Rect Size
        myRectTransform.sizeDelta = rectSizes;

        // Sprite Renderer's Rect Size
        if (!myBubbleManager.IsSingleTextBubbleManager)
            myParentRectTransform.sizeDelta = rectSizes;
    }

    public void SetDefaultVariables_Inspector()
    {
        myBubbleManager = GetComponentInParent<TextBubbleManager>();
        myParentRectTransform = transform.parent.GetComponentInParent<RectTransform>();
        myTextMeshPro = GetComponent<TextMeshProUGUI>();
        myRectTransform = GetComponent<RectTransform>();
        

        myTextMeshPro.fontSizeMin = textBubbleSO.FontSize.Value;

        // Color
        SetDefaultTextColor();

        // Margins
        float marginX = textBubbleSO.MarginSizesModifier * rectSizes.x;
        float marginY = textBubbleSO.MarginSizesModifier * rectSizes.y;
        myTextMeshPro.margin = new Vector4(marginX, marginY, marginX, marginY);

        // Spacing Character
        myTextMeshPro.characterSpacing = textBubbleSO.CharacterSpacing.Value;

        // Spacing Line
        myTextMeshPro.lineSpacing = textBubbleSO.LineSpacing.Value;

        // Rect Size
        myRectTransform.sizeDelta = rectSizes;

        // Image's Rect Size
        if (!myBubbleManager.IsSingleTextBubbleManager)
            myParentRectTransform.sizeDelta = rectSizes;
        else
            myBubbleManager.SetSingleManagerBackground(isInspector: true);
    }
    #endregion

    #region Setter _ Shorthand
    public void SetPage(int page)
    {
        myTextMeshPro.pageToDisplay = page;
    }

    /// <summary>
    /// Set charsColors[] variable<br/>
    /// Update for EVERY vertice's color values for this.TextBubble (4 color/vertices per character)
    /// </summary>
    public void SetColorsRef()
    {
        charsColors = GetColors();
    }

    public void SetDefaultTextString()
    {
        myTextMeshPro.text = myText.Value;
    }

    public void SetText(string text)
    {
        //myTextMeshPro.ForceMeshUpdate();
        myTextMeshPro.text = text;
        myTextMeshPro.canvasRenderer.SetMesh(myTextMeshPro.mesh);
    }

    public void SetDefaultTextColor()
    {
        myTextMeshPro.ForceMeshUpdate();

        if (myTextModifiers.GetPersistentEventCount() == 0)
            myTextMeshPro.color = textBubbleSO.FrontColor;
        else
            myTextMeshPro.color = textBubbleSO.BackColor;
    }

    public void SetCharacterColor(int charIndex, Color color)
    {
        TMP_CharacterInfo charInfo = myTextMeshPro.textInfo.characterInfo[charIndex];

        int index = charInfo.vertexIndex;

        charsColors[index] = color;
        charsColors[index+1] = color;
        charsColors[index+2] = color;
        charsColors[index+3] = color;

        myTextMeshPro.mesh.colors = charsColors;
        myTextMeshPro.canvasRenderer.SetMesh(myTextMeshPro.mesh);

    }
    #endregion

    #region Text Manipulation
    public void AppendChar(char character)
    {
        myTextMeshPro.text += character;
    }

    public void AppendString(string myString)
    {
        myTextMeshPro.text += myString;
    }
    #endregion

    #region Print Implementations
    public void Print(string text = " ")
    {
        //if (!myPrintImplementation)
            //return;

        if (myTextModifiers.GetPersistentEventCount() == 0)
            return;

        if (text != " ") SetText(text);
        
        myTextModifiers.Invoke();
        //StartCoroutine(PrintSimple());
    }

    public void PrintSimpleCoroutine()
    {
        StartCoroutine(PrintSimple());
    }

    private IEnumerator PrintSimple()
    {
        SetColorsRef();
        
        // Behaviour PER CHARACTER is done here
        for (int i = 0; i < myTextMeshPro.textInfo.characterCount; i++)
        {
            Debug.Log("A");
            SetCharacterColor(i, textBubbleSO.FrontColor);
            yield return new WaitForSeconds(textBubbleSO.TextSpeed.Value * Time.deltaTime);

            // DO NOT DELETE. PAGE TURN
            if (i > 2)
                if (GetCharacter(i - 1) != ' ' && GetCharacter(i - 1) != '\n' && !GetIsCharacterVisible(i - 1))
                {
                    myBubbleManager.MyInteractable.SetInteractableLayer(true);
                    myBubbleManager.MyInteractable.SetIsInteractable(true);
                    yield return new WaitUntil(() => !myBubbleManager.MyInteractable.IsInteractable);
                    SetPage(GetPageAtCharacter(i - 1) + 1);
                }
        }
    }
    #endregion

    #region Observer Pattern
    public void Notify()
    {
        myBubbleManager.MyInteractable.SetIsInteractable(false);
    }
    #endregion
}