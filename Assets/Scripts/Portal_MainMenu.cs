using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_MainMenu : MonoBehaviour {

    public GameObject audioManager;
    public AudioClip teleportSound;
    public AudioClip satanSound;     // Laugh sound to play when player enters the game

    void Awake()
    {
        // Automatically set reference
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Debug.Log("Game Started!");
            Destroy(col.gameObject);
            audioManager.GetComponent<AudioManager>().Play(teleportSound);
            audioManager.GetComponent<AudioManager>().Play(satanSound);

            // Loads the main level
            GameManager.LoadNextLevel();

        }
    }
}
