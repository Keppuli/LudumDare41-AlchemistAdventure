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
    void Awake()
    {
        //Ensure the script is not deleted while loading
        DontDestroyOnLoad(this);
        //Make sure there are copies are not made of the GameObject when it isn't destroyed
        if (FindObjectsOfType(GetType()).Length > 1)
            //Destroy any copies
            Destroy(gameObject);
    }

    void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

}
