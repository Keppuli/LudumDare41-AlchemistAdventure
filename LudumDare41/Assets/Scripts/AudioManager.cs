using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioClip bombExplode;
    public AudioClip playerBlown;
    public AudioClip playerEaten;
    public AudioClip playerTeleport;
    public AudioClip enemyAttack;
    private AudioSource audioSource;


    void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
