using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/// <summary_SUGGESTED_HOW_TO_EXTEND>
/// 
/// [Header("new GUI system's canvas parts")]
/// -- Canvas parts[]
/// -- keyboard sprites in order for above's parts
/// -- Gamepad sprites in order for above's parts
/// 
/// -- Extend [OnAnyKey()] with a new [SetCanvasSprites()] method for...
///     ... both keyboard and gamepad if statement
/// 
/// </summary>


public class CtrlLayoutManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    public static CtrlLayoutManager instance;

    [Header("Interact Button")]
    [SerializeField] private CanvasRenderer[] interactCanvasRenderer;
    [SerializeField] private Sprite[] InteractKeyboardSprites;
    [SerializeField] private Sprite[] InteractGamepadSprites;


    // SECTION - Method - Unity Specific ===================================================================
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    // SECTION - Method ===================================================================
    public void OnAnyKey(InputAction.CallbackContext cbc)
    {
        if (cbc.performed)
        {       
            if (interactCanvasRenderer != null)
            {
                // Keyboard & Mouse
                if (InputControlPath.MatchesPrefix("<Keyboard>", cbc.control) || InputControlPath.MatchesPrefix("<Mouse>", cbc.control))
                {
                    SetCanvasSprites(interactCanvasRenderer, InteractKeyboardSprites);

                    // Extend Here
                }

                // Gamepad
                if (InputControlPath.MatchesPrefix("<GamePad>", cbc.control))
                {
                    SetCanvasSprites(interactCanvasRenderer, InteractGamepadSprites);

                    // Extend Here
                }
            }        
        }
    }

    // SECTION - Method - Utility ===================================================================
    private void SetCanvasSprites(CanvasRenderer[] canvasArray, Sprite[] sprites)
    {
        for (int i = 0; i < canvasArray.Length; i++)
            canvasArray[i].GetComponent<Image>().sprite = sprites[i];
    }
}
