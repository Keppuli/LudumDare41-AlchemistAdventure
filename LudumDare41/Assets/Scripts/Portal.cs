using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject targetPortal;
    public bool disabled;
    public bool isEndPortal;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (isEndPortal)
        {
            animator.SetTrigger("MakeViolet");

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
