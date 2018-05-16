using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIce : MonoBehaviour {

    public Sprite ice;
    BoxCollider2D col;
    SpriteRenderer sr;
    private Animator animator; // Water tile is animated and ice is not
    public enum Type { Water, Ice };
    public Type type;

    void Start () {
        type = Type.Water; // Default is water
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Freeze()
    {
        type = Type.Ice;
        animator.enabled = false; // Stop animating water
        sr.sprite = ice;          // Change sprite to more icy
        col.enabled = false;      // Disable collision so Player can walk over 
    }

    /* Planned UnFreeze mechanic for future update
    public void UnFreeze()
    {
        type = Type.Water;
        animator.enabled = true;  
        col.enabled = true;
    }
    */
}
