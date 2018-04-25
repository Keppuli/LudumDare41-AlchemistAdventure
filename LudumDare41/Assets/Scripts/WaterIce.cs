using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterIce : MonoBehaviour {

    public Sprite ice;
    BoxCollider2D col;
    SpriteRenderer sr;
    private Animator animator;
    public enum Type { Normal, Ice };
    public Type type;

    void Start () {
        type = Type.Normal;
        animator = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void Freeze()
    {
        type = Type.Ice;
        animator.enabled = false;
        sr.sprite = ice;
        col.enabled = false;
    }
    public void UnFreeze()
    {
        type = Type.Normal;
        animator.enabled = true;
        col.enabled = true;
    }
}
