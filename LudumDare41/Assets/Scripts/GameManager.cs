using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    public AudioClip clickSound;

    public enum Mode { GameOn, GameOver };
    public static Mode mode;
    public static bool hasKey;
    public static int bombs = 222;
    public Image image_hasKey;
    public Text text_continue;
    public Text text_bombAmount;

    public GameObject audioManager;
    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        //Ensure the script is not deleted while loading
        DontDestroyOnLoad(this);
        //Make sure there are copies are not made of the GameObject when it isn't destroyed
        if (FindObjectsOfType(GetType()).Length > 1)
            //Destroy any copies
            Destroy(gameObject);
    }

    void Update () {
        text_bombAmount.text = bombs.ToString();
        if ( mode == Mode.GameOver) 
        {
            text_continue.GetComponent<Text>().enabled = true;
            Time.timeScale = 0F;
            //Debug.Log("GAME OVER!!!");
            if (Input.GetKeyDown("enter") || Input.GetKeyDown(KeyCode.Return)) {
                audioManager.GetComponent<AudioManager>().Play(clickSound);

                Debug.Log("Level Reloaded!");
                Time.timeScale = 1F;
                text_continue.GetComponent<Text>().enabled = false;
                ReloadLevel();
            }
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
    void ResetVariables()
    {
        hasKey = false;
        bombs = 2;
    }
    public static void ReloadLevel()
    {
        hasKey = false;
        bombs = 2;
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
