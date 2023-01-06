using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    [SerializeField] private GameObject myVisual;
    [SerializeField] private AudioClip breakingSFX;

    private Collider myCollider;
    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        myVisual.SetActive(false);
        myCollider.enabled = false;
        myAudioSource.PlayOneShot(breakingSFX);
    }
}
