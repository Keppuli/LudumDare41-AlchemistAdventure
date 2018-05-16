using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float fadeTimer = 1f;
    public float fadeMultiplier = 2f;

    public enum Type { Normal, Freeze };
    public Type type;

    private SpriteRenderer sr;
    private AudioSource audioSource; // Plays explosion sound upon awake
    private Animator animator;
    public Camera mainCamera;

    private void Awake()
    {
        // Store reference to the main camera for shaking
        mainCamera = Camera.main;
    }

    private void Start()
    {
        // Shake camera when explosion is spawned
        mainCamera.GetComponent<CameraManager>().shakeDuration = 0.1f;

        // Play explosion sound when spawned
        audioSource = GetComponent<AudioSource>();

        // Garbage collection inside Unity editor 
        GameObject _temp = GameObject.Find("TEMP"); 
        transform.SetParent(_temp.transform);

        // References for components
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (type == Type.Freeze)
        {
            // Changes explosion animation to icy explosion
            animator.SetTrigger("MakeIce");
        }
    }

    void Update()
    {
        // Count down fadeTimer for alpha
        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime * fadeMultiplier;
        }
        // Decreasing alpha with faceTimer, making sprite fade away smoothly
        sr.color = new Color(1, 1, 1, fadeTimer); 

        // When fadeTimer hits 0, sprite has faded away and obj can be erased
        if (fadeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

}
