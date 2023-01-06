using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHole : MonoBehaviour
{
    [SerializeField] private float visibleTime;

    private float visibleTimer;
    private Color white = Color.white;
    private Color transparent;

    private SpriteRenderer mySpriteRenderer;

    private void Awake()
    {
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        visibleTimer = visibleTime;
        transparent = Color.white;
        transparent.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (visibleTimer > 0)
        {
            visibleTimer -= Time.deltaTime;
            var ratio = (visibleTime - visibleTimer) / visibleTime;
            mySpriteRenderer.color = Color.Lerp(white, transparent, ratio);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
