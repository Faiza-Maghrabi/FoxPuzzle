using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--------- Audio Source --------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

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

    private void Start() {
        musicSource.clip = background1;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip){
        SFXSource.PlayOneShot(clip);
    }

    public void StopSFX(){
        SFXSource.Stop();
    }

}
