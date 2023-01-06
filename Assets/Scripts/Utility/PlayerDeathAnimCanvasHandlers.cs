using UnityEngine;

public class PlayerDeathAnimCanvasHandlers : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private TransformSO onDeathManagerTransformSO;
                     private OnDeathManager onDeathManager;


    // SECTION - Method - Utility Specific ===================================================================
    private void Start()
    {
        Invoke("LateStart", 1.0f);
    }

    private void Update()
    {
        if(onDeathManager == null)
        {
            GameObject myCanvasObject = GameObject.Find("Death Canvas");

            if (myCanvasObject)
                onDeathManager = myCanvasObject.GetComponent<OnDeathManager>();
        }
    }


    // SECTION - Method - Utility Specific ===================================================================
    private void LateStart()
    {
        onDeathManager = onDeathManagerTransformSO.Transform.GetComponent<OnDeathManager>();
    }

    public void ToggleActiveCanvasRef(TransformSO gameObjectRef = null)
    {
       if (gameObjectRef != null && gameObjectRef.Transform != null)
           gameObjectRef.Transform.gameObject.SetActive(!gameObjectRef.Transform.gameObject.activeSelf);
    }

    public void CheckEndAsync()
    {
        if (onDeathManager != null)
        {
            EnableOnDeathManager();
            onDeathManager.OnDeathAnimationEnd();
        }

    }

    public void EnableOnDeathManager()
    {
        if (onDeathManager != null)
            onDeathManager.enabled = true;
    }

    public void ShowDeathCue()
    {
        onDeathManager.transform.GetChild(0).gameObject.SetActive(true);
    }
}
