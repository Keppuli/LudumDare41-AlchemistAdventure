using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject targetPortal;
    public bool disabled;

    // Defines the type of portal
    public bool isEndPortal;
    public bool isTutorialPortal;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();

        if (isEndPortal)
        {
            animator.SetTrigger("MakeViolet"); // Changes animation that uses violet sprites

        }
        if (isTutorialPortal)
        {
            animator.SetTrigger("MakeGreen"); // Changes animation that uses green sprites
        }
    }

    // Portal gets disabled when player collides with it first time, exit activates it
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            disabled = false;
        }
    }
}
