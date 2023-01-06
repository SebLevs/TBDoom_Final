using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingSpriteRendererBehaviour : MonoBehaviour
{
    // SECTION - Field ===================================================================
    [SerializeField] private float spriteRotationSpeed = 1.0f;
    [SerializeField] private float spriteVerticalRange = 0.05f;
    [SerializeField] private float spriteVerticalSpeed = 1.0f;

    private Transform myTransform;
    private Vector3 spriteInitialPosition;


    // SECTION - Method ===================================================================
    private void Start()
    {
        myTransform = GetComponentInChildren<Transform>();
        spriteInitialPosition = myTransform.transform.localPosition;
    }

    void FixedUpdate()
    {
        myTransform.Rotate(new Vector3(0, spriteRotationSpeed, 0));
        var verticalOffset = Mathf.Sin(Time.time * spriteVerticalSpeed) * spriteVerticalRange;
        myTransform.transform.localPosition = spriteInitialPosition + new Vector3(0, verticalOffset, 0);
    }
}
