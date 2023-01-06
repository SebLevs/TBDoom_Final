using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/TextBubble/Variables", fileName = "SO _ Text Bubble")]
public class TextBubbleSO : ScriptableObject
{
    // SECTION - Field ===================================================================
    [Header("General")]
    [SerializeField] private FloatVariable textSpeed;
    
    [Header("TextMeshPro")]
    [SerializeField] private Color backColor;
    [SerializeField] private Color frontColor;

    [Space(10)]
    [Tooltip("0.05f is a desireable size")]
    [SerializeField] private FloatVariable fontSize;
    [Tooltip("0.025f is a desireable size")]
    [SerializeField] private const float marginSizesModifier = 0.1f;
    [SerializeField] private FloatVariable characterSpacing;
    [SerializeField] private FloatVariable lineSpacing;



    // SECTION - Method ===================================================================
    #region Property
    public FloatVariable TextSpeed { get => textSpeed; set => textSpeed = value; }
    public Color BackColor { get => backColor; set => backColor = value; }
    public Color FrontColor { get => frontColor; set => frontColor = value; }
    public FloatVariable FontSize { get => fontSize; set => fontSize = value; }
    public float MarginSizesModifier { get => marginSizesModifier; }
    public FloatVariable CharacterSpacing { get => characterSpacing; set => characterSpacing = value; }
    public FloatVariable LineSpacing { get => lineSpacing; set => lineSpacing = value; }
    #endregion
}
