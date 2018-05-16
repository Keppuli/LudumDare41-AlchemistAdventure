using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour {
    void Awake()
    {
        // Se up references automatically, as it sometimes is lost duting scene load
        GetComponent<Canvas>().worldCamera = Camera.main;

        // Ensure the object is not deleted while changing scene
        DontDestroyOnLoad(this);
        // Make sure there are only one instance
        if (FindObjectsOfType(GetType()).Length > 1)
            // Destroy if copies found
            Destroy(gameObject);
    }


}
