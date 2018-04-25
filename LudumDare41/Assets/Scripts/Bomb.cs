using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    private AudioSource audioSource;
    public enum Type { Normal, Freeze };
    public Type type;
    public Sprite freezeSprite;
    private SpriteRenderer sr;
    string explosionType = "normal";

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        if (type == Type.Normal)
        {
            GameManager.bombs -= 1;
        }
        if (type == Type.Freeze)
        {
            explosionType = "freeze";
            GameManager.bombsFreeze -= 1;
            sr.sprite = freezeSprite;
            GetComponent<Explode>().explosionType = "freeze";
        }
    }

}
