using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public AudioClip foxWalk1;
    public AudioClip foxWalk2;
    public AudioClip foxRun1;
    public AudioClip foxRun2;
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

    // Checks if music is paused
    private bool isPaused =  false;


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

    void Update(){
        Scene scene = SceneManager.GetActiveScene();

        string sceneName = scene.name;
        if(!sceneName.Contains("Den"))
        {
            if(isPaused){
                musicSource.UnPause();
                isPaused = false;
            }
        } 
        else 
        {
            musicSource.Pause();
            isPaused = true;
        }
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
