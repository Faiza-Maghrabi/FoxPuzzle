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
    [SerializeField] public AudioSource dogSFXSource;
    [SerializeField] public AudioSource bearSFXSource;
    [SerializeField] public AudioSource maleSFXSource;
    [SerializeField] public AudioSource femaleSFXSource;
    [SerializeField] public AudioSource boySFXSource;

    [Header("--------- Clip --------")]
    public AudioClip background1;
    public AudioClip background2;
    public AudioClip foxWalk;
    public AudioClip foxRun;
    public AudioClip foxLand;
    public AudioClip boyShout;
    public AudioClip snowballHit;
    public AudioClip womanShout;
    public AudioClip manShout;
    public AudioClip dogGrowl;
    public AudioClip bearGrowl;
    public AudioClip foxHurt;
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
        if (source.clip != clip || !source.isPlaying)
        {
            source.PlayOneShot(clip);
        }
    }

    public void Stop(AudioSource source)
    {
        source.Stop();
    }

}
