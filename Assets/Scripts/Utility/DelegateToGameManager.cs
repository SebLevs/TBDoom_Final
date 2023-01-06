using UnityEngine;

public class DelegateToGameManager : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private bool isGargoyle = false;


    // SECTION - Method - Unity Specific ===================================================================
    private void Start()
    {
        if (isGargoyle)
        {
            #if UNITY_WEBGL
            Destroy(gameObject);
            #endif
        }
    }


    // SECTION - Method - Utility Specific ===================================================================
    public void DelegateQuitGame()
    {
        GameManager.instance.QuitGame();
    }
}
