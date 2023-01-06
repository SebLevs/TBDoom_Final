using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTimer : MonoBehaviour
{
    [SerializeField] private float timer = 0.5f;

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
            Destroy(gameObject);
    }
}
