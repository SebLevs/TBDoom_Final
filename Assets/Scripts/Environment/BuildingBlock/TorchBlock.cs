using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchBlock : MonoBehaviour
{
    [SerializeField] private ParticleSystem myFire;
    
    private Light myLight;
    private float myLightIntensity;
    [SerializeField] private float lightFlickerTime = 0.5f;
    private float lightFlikerTimer;

    private AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        myFire.Play();
        myLight = GetComponentInChildren<Light>();
        myLightIntensity = myLight.intensity;
        lightFlikerTimer = lightFlickerTime;
        myAudioSource = GetComponentInChildren<AudioSource>();
        myAudioSource.time = Random.value * myAudioSource.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        //if (lightFlikerTimer > 0)
        //{
        //    lightFlikerTimer -= Time.deltaTime;
        //}
        //else
        //{
        //    myLight.intensity = myLightIntensity * Random.Range(0.9f, 1.1f);
        //    lightFlikerTimer = lightFlickerTime * Random.Range(0.75f, 1.25f);
        //}
    }

    public void LitTorch()
    {
        myFire.gameObject.SetActive(true);
    }

    public void UnlitTorch()
    {
        myFire.gameObject.SetActive(false);
    }
}
