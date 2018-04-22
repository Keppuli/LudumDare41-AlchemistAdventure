using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public GameObject targetPortal;
    public bool disabled;

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            disabled = false;
        }
    }
}
