using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private AudioSource audioSource;
    private SpriteRenderer sr;

    // Type can be set in the editor to modularily use the same script for both types of bomb
    public enum Type { Normal, Freeze };
    public Type type;

    public Sprite freezeSprite; // Blue bomb sprite for Freeze bomb
    public string explosionType = "normal"; // Used to communicate with explosion, default normal


    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // When bomb is spawned, reduce global bomb amount by type
        if (type == Type.Normal)
        {
            GameManager.bombs -= 1;
        }
        if (type == Type.Freeze)
        {
            explosionType = "freeze";
            GameManager.bombsFreeze -= 1;
            // If bomb is type freeze also change sprite to blue
            sr.sprite = freezeSprite;
            // and set explosion to be type freeze
            GetComponent<Explode>().explosionType = "freeze";
        }
    }

}
