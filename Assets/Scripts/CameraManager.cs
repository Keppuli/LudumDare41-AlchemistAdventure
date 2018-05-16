using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject player;
    
    public float shakeDuration = 0f;    // How long the object should shake for
    public float shakeAmount = 0.7f;    // Amplitude of the shake. A larger value shakes the camera harder
    public float decreaseFactor = 1.0f; // Shake decrease time

    private Vector3 originalPos;                // Stores camera's coordinates before shakes and follows
    private Vector3 offset;             // Offset distance between the player and camera     
    public float smoothSpeed = 0.125f;  // How smoothly camera follows Player
    public int cameraZ = -10;           // Distance of the camera from the world

    void Awake()
    {
        // Automatically set reference to the player
        player = GameObject.FindWithTag("Player");

        // Ensure the object is not deleted while changing scene
        DontDestroyOnLoad(this);
        // Make sure there are only one instance
        if (FindObjectsOfType(GetType()).Length > 1)
            // Destroy if copies found
            Destroy(gameObject);
    }

    // When enabled store camera's coordinates before all shakes and follows
    void OnEnable()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        // Shake
        if (shakeDuration > 0)
        {
            // Randomize new position inside sphere shape, which size is calculated using shakeAmount
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            // Calculate time to stop shaking
            shakeDuration -= Time.deltaTime * decreaseFactor;
        }

        // Check if player reference is set
        else if (player) { 

            // Calculate camera's target position from player position + offset
            Vector3 targetPos = player.transform.position + offset;

            // Linearly interpolate current position with target position with set smoothing speed
            Vector3 smoothedPos = Vector3.Lerp(transform.position, targetPos, smoothSpeed);

            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = new Vector3(smoothedPos.x,smoothedPos.y, cameraZ);

            // Update 
            originalPos = transform.localPosition;
        }

        // If player refence is lost, recover it
        else
        {
            player = GameObject.FindWithTag("Player");
        }
    }

}
