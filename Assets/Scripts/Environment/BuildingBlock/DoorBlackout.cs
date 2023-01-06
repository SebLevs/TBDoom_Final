using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBlackout : MonoBehaviour
{
    [SerializeField] private float minDistance = 10;
    [SerializeField] private float maxDistance = 15;

    private SpriteRenderer mySpriteRenderer;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = PlayerContext.instance.transform;
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisual();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (playerTransform == null)
        {
            return;
        }

        var distance = (playerTransform.position - transform.position).magnitude;
        var newColor = mySpriteRenderer.color;
        newColor.a = (distance - minDistance) / (maxDistance - minDistance);
        mySpriteRenderer.color = newColor;
    }
}
