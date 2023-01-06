using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    [SerializeField] private float lifetime = 3f;

    private float timer;

    private void Awake()
    {
        lifetime += Random.Range(-0.2f, 0.2f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > lifetime)
        {
            Destroy(gameObject);
        }
    }
}
