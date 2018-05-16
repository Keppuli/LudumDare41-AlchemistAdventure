using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    private AudioSource audioSource;

    void Awake()
    {
        // Ensure the object is not deleted while changing scene
        DontDestroyOnLoad(this);
        // Make sure there are only one instance
        if (FindObjectsOfType(GetType()).Length > 1)
            // Destroy if copies found
            Destroy(gameObject);
    }

    void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    //  Method for centralized audio playback
    public void Play(AudioClip clip)
    {
        // Reference to the audio clip is stored and given by the calling object
        audioSource.PlayOneShot(clip);  
    }

}
