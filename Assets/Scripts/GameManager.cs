using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public AudioClip clickSound;

    public enum Mode { GameOn, GameOver };
    public static Mode mode;

    public enum BombType { Normal, Freeze };
    public static BombType bombType;

    public static bool hasKey;
    public static int bombs;
    public static int bombsFreeze;

    public Image image_hasKey;
    public Text text_continue;
    public Text text_bombAmount;
    public Text text_freezeBombAmount;
    public Text text_gamePaused;

    // Time based high score system
    public Text text_currentTime;
    public Text text_bestTime;
    public static float timer;

    public GameObject gui_bomb;
    public GameObject gui_freezebomb;

    public static bool gamePaused = false;
    public static bool escKeyReserved = false;
    public GameObject audioManager;
    public Camera mainCamera;

    void Awake()
    {
        // Automatically set reference to Audio Manager, usually lost with Scene load
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        mainCamera = Camera.main;   // Store reference to the main camera

        bombType = BombType.Normal; // Set normal bomb to be selected by default

        // Ensure the object is not deleted while changing scene
        DontDestroyOnLoad(this);
        // Make sure there are only one instance
        if (FindObjectsOfType(GetType()).Length > 1)
            // Destroy if copies found
            Destroy(gameObject);

        LoadHighScore();
    }

    void Update ()
    {
        // Way to reset player pref data structure
        if (Input.GetKeyDown(KeyCode.F12))
        {
            ResetHighScore();
        }
        // Time based high score system
        UpdateTimer();

        // Check if reference to the audio manager is lost and recover it
        if (!audioManager)
        {
            audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        }

        // Update bomb statistics to the GUI
        text_bombAmount.text = bombs.ToString();
        text_freezeBombAmount.text = bombsFreeze.ToString();

        // What happens when player dies
        if (mode == Mode.GameOver)
        {
            // Reset shake from bomb blast
            mainCamera.GetComponent<CameraManager>().shakeDuration = 0f;    

            // Show disabled Game Over text
            text_continue.GetComponent<RandomTextColorNoPause>().enabled = true;
            text_continue.GetComponent<Text>().enabled = true;

            // Pause everything
            mainCamera.GetComponent<CameraManager>().shakeDuration = 0f; // Stop camera shaking when game over
            Time.timeScale = 0F;

            // Wait for player to press Enter
            if (Input.GetKeyDown("enter") || Input.GetKeyDown(KeyCode.Return))
            {
                // Return normal game speed
                Time.timeScale = 1F;

                audioManager.GetComponent<AudioManager>().Play(clickSound);

                // Hide Game Over text
                text_continue.GetComponent<RandomTextColorNoPause>().enabled = false;
                text_continue.GetComponent<Text>().enabled = false;

                ReloadLevel();
                Debug.Log("Level Reloaded!");

            }
        }
        else  // Game On
        {
            if (Input.GetKeyDown(KeyCode.R) && gamePaused)
            {
                UnPause();
                ReloadLevel();
            }
            if (Input.GetKeyDown(KeyCode.Q) && gamePaused)
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
            {
                Pause();
                escKeyReserved = true;  // Prevents ESC to be used withing same frame to unpause
            }
            if (Input.GetKeyDown(KeyCode.Escape) && !escKeyReserved && gamePaused)
            {
                UnPause();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                bombType = BombType.Normal; // BombType is checked by Player.cs DropBomb() method and also sent to the GUI
                gui_bomb.GetComponent<Animator>().enabled = true;           // Continue Normal bomb GUI image animation
                gui_freezebomb.GetComponent<Animator>().enabled = false;    // Hold Freeze bomb GUI image animation
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bombType = BombType.Freeze; // BombType is checked by Player.cs DropBomb() method and also sent to the GUI
                gui_bomb.GetComponent<Animator>().enabled = false;          // Hold Normal bomb GUI image animation
                gui_freezebomb.GetComponent<Animator>().enabled = true;     // Continue Freeze bomb GUI image animation
            }
            escKeyReserved = false; // Release ESC key for the next frame
        }

        // Check if player has Golden key to unlock gates, set GUI accordingly
        if (hasKey)
        {
            image_hasKey.GetComponent<Image>().enabled = true;
        }
        else
        {
            image_hasKey.GetComponent<Image>().enabled = false;
        }
    }

    void UpdateTimer()
    {
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        text_currentTime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    public static void SaveHighScore()
    {
        float bestTime = PlayerPrefs.GetFloat("HighScore");
        if (bestTime == 0)
        {
            PlayerPrefs.SetFloat("HighScore", timer);
            Debug.Log("Saved High Score: " + timer);

        }
        else if (timer < bestTime)
        {
            PlayerPrefs.SetFloat("HighScore", timer);
            Debug.Log("Saved High Score: " + timer);

        }
    }
    public void LoadHighScore()
    {
        float bestTime = PlayerPrefs.GetFloat("HighScore");
        int minutes = Mathf.FloorToInt(bestTime / 60F);
        int seconds = Mathf.FloorToInt(bestTime - minutes * 60);
        text_bestTime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        Debug.Log("High Score: " + bestTime + "loaded");
    }
    public void ResetHighScore()
    {
        PlayerPrefs.SetFloat("HighScore", 0f);
        Debug.Log("High Score reset");
        LoadHighScore();
    }
    void Pause()
    {
        mainCamera.GetComponent<CameraManager>().shakeDuration = 0f; // Stop camera shaking when paused
        Time.timeScale = 0F;
        gamePaused = true;
        text_gamePaused.GetComponent<Text>().enabled = true;
    }

    void UnPause()
    {
        Time.timeScale = 1F;
        gamePaused = false;
        text_gamePaused.GetComponent<Text>().enabled = false;
    }

    // Keep bomb count in range of 0-999 (1000 will mess up GUI)
    public static void BombTresholdCheck(string bombType, int amount)
    {

        if (bombType == "bombs")
        {
            bombs += amount;
            if (bombs < 0)
            {
                bombs = 0;
            }
            else if (bombs > 999)
            {
                bombs = 999;
            }
        }

        else if (bombType == "bombsFreeze")
        {
            bombsFreeze += amount;
            if (bombsFreeze < 0)
            {
                bombsFreeze = 0;
            }
            else if (bombsFreeze > 999)
            {
                bombsFreeze = 999;
            }
        }

        else
        {
            Debug.Log("Error in 'bombType' in BombTresholdCheck()!");
        }
    }

    // Resets all used variables
    static void ResetVariables()
    {
        timer = 0;
        hasKey = false;
        bombs = 0;
        bombsFreeze = 0;
        gamePaused = false;
        escKeyReserved = false;
    }

    // Reloads current level
    public static void ReloadLevel()
    {
        ResetVariables();
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }

    // Currently at v1.1 loads the END Scene, ending the game
    public static void LoadNextLevel()
    {
        ResetVariables();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single); // Loads the next scene from index
    }

    // Simply loads scene 0
    public static void ResetGame()
    {
        ResetVariables();
        SceneManager.LoadScene(0, LoadSceneMode.Single); 
    }
}
