using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

    private Rigidbody2D rb;
    public Vector3 explosionPosition; // Set by enemy.cs BlowUp()
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Start with random rotation
        transform.Rotate(Vector3.forward, Random.Range(-45, 45));

        // Add force when created(by explosion)
        AddExplosionForce(explosionPosition);
    }

    // Called by explosion to push bones around
    public void AddExplosionForce(Vector3 explosionPosition)
    {
        rb.AddExplosionForce(1000, new Vector2(explosionPosition.x, explosionPosition.y), 1500);

    }

}
