using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source --------")]
    [SerializeField] public AudioSource musicSource;
    [SerializeField] public AudioSource foxSFXSource;
    [SerializeField] public AudioSource inventorySFXSource;

    [Header("--------- Clip --------")]
    public AudioClip background1;
    public AudioClip background2;
    public AudioClip foxWalk;
    public AudioClip foxRun;
    public AudioClip dogWalk;
    public AudioClip dogGrowl;
    public AudioClip bearWalk;
    public AudioClip bearGrowl;
    public AudioClip foxDeath;
    public AudioClip foxEat;
    public AudioClip pickUpFood;

    public static AudioManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance =  this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Start() 
    {
        musicSource.clip = background1;
        musicSource.Play();
    }

    public void PlayMusic(AudioClip clip, AudioSource source)
    {
        if (source.clip != clip || !source.isPlaying)
        {
            source.clip = clip;
            source.Play();
        }
    }

    public void PlaySFX(AudioClip clip, AudioSource source)
    {
        source.PlayOneShot(clip);
    }

    public void Stop(AudioSource source)
    {
        source.Stop();
    }

}
