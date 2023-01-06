using UnityEngine;

public class SetScriptableTransformRefOnEnable : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [Tooltip("If left empty, a temporary scriptableObject will be created instead\nRefere to variable consequently")]
    [SerializeField] private TransformSO myTransformRef;


    // SECTION - Method ===================================================================
    void OnEnable()
    {       
        if (!myTransformRef)
            myTransformRef = ScriptableObject.CreateInstance<TransformSO>();

        myTransformRef.Transform = GetComponent<Transform>();
    }
}
