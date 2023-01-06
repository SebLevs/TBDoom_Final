using UnityEngine;

public class FloatingCanvasImageBehaviour : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private float spriteRotationSpeed = 1.0f;
    [SerializeField] private float spriteVerticalRange = 0.05f;
    [SerializeField] private float spriteVerticalSpeed = 1.0f;

    private RectTransform myRectTransform;
    private Vector3 spriteInitialPosition;


    // SECTION - Method ===================================================================
    private void Start()
    {
        myRectTransform = GetComponentInChildren<RectTransform>();
        spriteInitialPosition = myRectTransform.transform.localPosition;
    }

    void FixedUpdate()
    {
        myRectTransform.Rotate(new Vector3(0, spriteRotationSpeed, 0));
        var verticalOffset = Mathf.Sin(Time.time * spriteVerticalSpeed) * spriteVerticalRange;
        myRectTransform.transform.localPosition = spriteInitialPosition + new Vector3(0, verticalOffset, 0);
    }
}
