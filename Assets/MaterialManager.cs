using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] private Material lavaMaterial;
    [SerializeField] private float lavaOffsetSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lavaMaterial.mainTextureOffset += Vector2.one * lavaOffsetSpeed * Time.deltaTime;
    }
}
