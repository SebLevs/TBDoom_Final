using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private AudioClip hubTheme;
    [SerializeField] private AudioClip explorationThemeOutOfCombat;
    [SerializeField] private AudioClip explorationThemeInCombat;
    [SerializeField] private AudioClip bossTheme;

    private AudioSource myAudioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myAudioSource.loop = true;
        PlayHubTheme();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopMusic()
    {
        myAudioSource.Stop();
    }

    public void PlayHubTheme()
    {
        myAudioSource.clip = hubTheme;
        myAudioSource.Play();
    }

    public void SwitchToBoss()
    {
        myAudioSource.clip = bossTheme;
        myAudioSource.Play();
    }

    public void SwitchToInCombat()
    {
        myAudioSource.clip = explorationThemeInCombat;
        myAudioSource.Play();
    }

    public void SwitchToOutOfCombat()
    {
        myAudioSource.clip = explorationThemeOutOfCombat;
        myAudioSource.Play();
    }
}
