using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    private GameObject audioManager;
    private GameObject gameManager;
    private GameObject canvas;

    void Awake () {

        // Automatically set references
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        // These have to be specifically destroyed due to them using (DontDestroyOnLoad(this))
        Destroy(audioManager);
        Destroy(gameManager);
        Destroy(canvas);

    }

    void Update () {

        if (Input.GetKeyDown("enter") || Input.GetKeyDown(KeyCode.Return))
        {

            // Loads Main menu
            GameManager.ResetGame();
        }
    }
}
