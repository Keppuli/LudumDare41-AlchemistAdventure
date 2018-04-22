using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject targetPortal;
    public bool disabled;
    public bool isEndPortal;
    public bool isTutorialPortal;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (isEndPortal)
        {
            animator.SetTrigger("MakeViolet");

        }
        if (isTutorialPortal)
        {
            animator.SetTrigger("MakeGreen");

        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            disabled = false;
        }
    }
}
