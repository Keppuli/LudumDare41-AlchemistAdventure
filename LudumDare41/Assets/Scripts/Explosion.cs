using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public float fadeTimer = 1f;
    public float fadeMultiplier = 2f;

    private new SpriteRenderer sr;
    private AudioSource audioSource;

    private void Start()
    {
        GameObject _temp = GameObject.Find("TEMP"); // Trashcan for particles etc.
        transform.SetParent(_temp.transform);
        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Play explosion sound on awake
    }

    void Update()
    {
        if (fadeTimer > 0)
        {
            fadeTimer -= Time.deltaTime * fadeMultiplier;
        }
        sr.color = new Color(1, 1, 1, fadeTimer); // Fade with alpha
        if (fadeTimer <= 0)
        {
            //Debug.Log("Explosion smoke destroyed.");
            Destroy(gameObject);
        }
    }

}
