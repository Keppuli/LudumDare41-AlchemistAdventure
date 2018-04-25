using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject player;        //Public variable to store a reference to the player game object
    private Vector3 offset;         //Private variable to store the offset distance between the player and camera
    public float smoothSpeed = 0.125f;
    public int cameraZ = -10;
    void Awake()
    {
        //Ensure the script is not deleted while loading
        DontDestroyOnLoad(this);
        //Make sure there are copies are not made of the GameObject when it isn't destroyed
        if (FindObjectsOfType(GetType()).Length > 1)
            //Destroy any copies
            Destroy(gameObject);
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {

        if (player) { 
        Vector3 targetPos = player.transform.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = new Vector3(smoothedPos.x,smoothedPos.y, cameraZ);
        }
        else
        {
            player = GameObject.FindWithTag("Player");

        }
    }
}
