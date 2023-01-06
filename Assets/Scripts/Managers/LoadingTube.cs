using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingTube : MonoBehaviour
{
    [SerializeField] private Transform respawnHeight;

    public Transform RespawnHeight { get => respawnHeight; set => respawnHeight = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = new Vector3(other.transform.position.x, respawnHeight.position.y, other.transform.position.z);
        }
    }
}
