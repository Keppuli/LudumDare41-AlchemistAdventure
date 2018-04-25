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

    public GameObject gui_bomb;
    public GameObject gui_freezebomb;

    public static bool gamePaused = false;
    public static bool escKeyReserved = false;
    public GameObject audioManager;

    void Awake()
    {

        bombType = BombType.Normal;
        //Ensure the script is not deleted while loading
        DontDestroyOnLoad(this);
        //Make sure there are copies are not made of the GameObject when it isn't destroyed
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);
    }

    void Update () {

        if (!audioManager)
        {
            audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        }

        text_bombAmount.text = bombs.ToString();
        text_freezeBombAmount.text = bombsFreeze.ToString();

        if (mode == Mode.GameOver)
        {
            text_continue.GetComponent<RandomTextColorNoPause>().enabled = true;
            text_continue.GetComponent<Text>().enabled = true;
            Time.timeScale = 0F;
            if (Input.GetKeyDown("enter") || Input.GetKeyDown(KeyCode.Return))
            {
                audioManager.GetComponent<AudioManager>().Play(clickSound);

                Debug.Log("Level Reloaded!");
                Time.timeScale = 1F;
                text_continue.GetComponent<RandomTextColorNoPause>().enabled = false;
                text_continue.GetComponent<Text>().enabled = false;
                ReloadLevel();
            }
        }
        else  // GameON
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
                bombType = BombType.Normal;
                gui_bomb.GetComponent<Animator>().enabled = true;           // Normal
                gui_freezebomb.GetComponent<Animator>().enabled = false;    // Freeze
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                bombType = BombType.Freeze;
                gui_bomb.GetComponent<Animator>().enabled = false;          // Normal
                gui_freezebomb.GetComponent<Animator>().enabled = true;     // Freeze
            }
            escKeyReserved = false;
        }

        if (hasKey)
        {
            image_hasKey.GetComponent<Image>().enabled = true;
        }
        else
        {
            image_hasKey.GetComponent<Image>().enabled = false;
        }
    }
    void Pause()
    {
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
    public static void ReloadLevel()
    {
        hasKey = false;
        bombs = 0;
        bombsFreeze = 0;
        gamePaused = false;
        escKeyReserved = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
    }
   
    public static void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public static void ResetGame()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
