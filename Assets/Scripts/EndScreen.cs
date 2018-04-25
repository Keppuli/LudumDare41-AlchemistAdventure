using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    private GameObject audioManager;
    private GameObject gameManager;
    private GameObject canvas;


    // Use this for initialization
    void Awake () {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
        canvas = GameObject.FindGameObjectWithTag("Canvas");

        Destroy(audioManager);
        Destroy(gameManager);
        Destroy(canvas);

    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown("enter") || Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.ResetGame();
        }
    }
}
