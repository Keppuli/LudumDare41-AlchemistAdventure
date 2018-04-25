using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableLevel : MonoBehaviour {

    public GameObject level;
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.tag == "Player")
        {
            Debug.Log("Level Enabled");
            level.SetActive(true);
        }
    }
}
