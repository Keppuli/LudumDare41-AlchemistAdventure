using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSprite : MonoBehaviour {

    // Default facing is always left, bool = false
    public bool facingRight;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // We are dealing with Player
        if (gameObject.tag == "Player")
        {
            // Check input data from horizontal movement key
            CheckVelocity(Input.GetAxisRaw("Horizontal"));
        }

        // We are dealing with AI without Input data
        else
        {
            // Check rigid body x-axis velocity 
            CheckVelocity(rb.velocity.x);
        }
    }
    void CheckVelocity(float velocity)
    {
        // If velocity is against current facing, we need a flip in sprite
        if ((velocity < 0 && facingRight) || (velocity > 0 && !facingRight))
            Flip();
    }

    // Flips the sprite
    void Flip()
    {
        // Invert the current facing whatever it may be
        facingRight = !facingRight; 

        // We can't directly alter localScale.x so lets make a reference
        Vector3 theScale = transform.localScale;

        // Alter reference inverting x-axis scale with (*-1)
        theScale.x *= -1;

        // Feed new values with y intact and inverted x back to localScale
        transform.localScale = theScale;
    }

}
