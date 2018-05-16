using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bomb;
    public GameObject blown;
    public AudioClip keyPickSound;
    public AudioClip bombPickSound;

    public AudioClip gateUnlock;
    public AudioClip satanGameOverSound;

    public AudioClip dieSound;
    public AudioClip blowUpSound;
    public AudioClip teleportSound;

    public enum Mode { None,Teleporting,Dying};
    public Mode mode;

    public float moveSpeed;
    public float curSpeed;
    public float movementModifier;

    private Rigidbody2D rb;
    private Animator animator;
    public GameObject audioManager;

    void Awake()
    {
        // Automatically set reference to Audio Manager, usually lost with Scene load
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");

        // Player spawning acts as a trigger to start game after game over
        GameManager.mode = GameManager.Mode.GameOn;
    }

    void Start()
    {
        // Set component references
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Set default mode for the player
        mode = Mode.None;   
    }

    void Update()
    {
        if (mode == Mode.None)
        {
            // Call player to be animated
            Animate();

            // Handle use key
            if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
            {
                DropBomb();
            }
        }
    } 
 
    // Spawns instance of a selected bomb type
    void DropBomb()
    {
        // Set position for bomb instance with slight Y-axis offset to get the bomb behind player
        Vector3 bombPos = new Vector3(transform.position.x, transform.position.y + 0.2f);

        // Check what bomb type is selected in GUI and instantiate according to that
        var bombType = GameManager.bombType; 
        if (bombType == GameManager.BombType.Normal)
        {
            if (GameManager.bombs > 0)
            {
                GameObject newBomb = Instantiate(bomb, bombPos, Quaternion.identity);
                newBomb.GetComponent<Bomb>().type = Bomb.Type.Normal; // Set type of the bomb on the fly. This modularily sets graphics and explosion in Bomb.cs.
            }
        }
        if (bombType == GameManager.BombType.Freeze)
        {
            if (GameManager.bombsFreeze > 0)
            {
                GameObject newBomb = Instantiate(bomb, bombPos, Quaternion.identity);
                newBomb.GetComponent<Bomb>().type = Bomb.Type.Freeze; // Set type of the bomb on the fly. This modularily sets graphics and explosion in Bomb.cs.
            }
        }
    }

    // Reads player velocity and sets animation accordingly
    void Animate()
    {
        // Change velocity to absolute values, for easier usage
        float absHS = Mathf.Abs(rb.velocity.x);
        float absVS = Mathf.Abs(rb.velocity.y);
        // Check against absolute values
        if (absHS > 0 || absVS > 0) // If more than 0 player is moving
        {
            animator.SetTrigger("Walk");
        }
        else // If 0 player must be idling
        {
            animator.SetTrigger("Idle");
        }

        
    }

    // Update method for physics
    void FixedUpdate()
    {
        if (mode == Mode.None)
        {
            // MOVEMENT
            curSpeed = moveSpeed;
            // Read raw movement key data
            float horizontalSpeed = Input.GetAxisRaw("Horizontal");// GetAxis vs. GetAxisRaw?
            float verticalSpeed = Input.GetAxisRaw("Vertical");
            // Calculate new velocity for the player from set speed and modifiers + Lerp it to keep more constant rate
            rb.velocity = new Vector2(Mathf.Lerp(0, horizontalSpeed * curSpeed * movementModifier, 1f), Mathf.Lerp(0, verticalSpeed * curSpeed * movementModifier, 1f));
        }

    }



    // For objects lacking collision
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Portal") // (Portal tag can be used to also receive teleportation )
        { 
            Debug.Log("Collided with Portal tagged object.");
            GameObject targetPortal = col.GetComponent<Portal>().targetPortal; // Destination portal where this portal is linked

            // Check if portal is marked as being end portal (violet)
            if (col.GetComponent<Portal>().isEndPortal)
            {
                audioManager.GetComponent<AudioManager>().Play(teleportSound);
                GameManager.SaveHighScore();
                GameManager.LoadNextLevel(); 
            }
            else if (col.GetComponent<Portal>().enabled)
            {
                Vector3 targetPortalPos = targetPortal.transform.position;
                if (targetPortal.GetComponent<Portal>())    // Check if target portal is actually portal (other obj's can be used for one way tele)
                    targetPortal.GetComponent<Portal>().enabled = false; // Temporarily disable the portal to avoid teleportation loop

                // Calculate the teleportation coordinates for player
                gameObject.transform.position = new Vector2(targetPortalPos.x, targetPortalPos.y);
                audioManager.GetComponent<AudioManager>().Play(teleportSound);
            }

        }
        if (col.tag == "Key")
        {
            Debug.Log("Trigger with Key.");
            GameManager.hasKey = true;
            // Destroy collectable object as it is now collected
            DestroyObject(col.gameObject, keyPickSound);
        }
        if (col.tag == "BombCollectable")
        {
            Debug.Log("Trigger with Bomb Collectable.");
            if (col.gameObject.GetComponent<BombCollectable>().type == BombCollectable.Type.Freeze)
                // Check the amount of bombs collectable holds and send it to the Game Manager treshold check
                GameManager.BombTresholdCheck("bombsFreeze", col.gameObject.GetComponent<BombCollectable>().amount);
            else
                // Check the amount of bombs collectable holds and send it to the Game Manager treshold check
                GameManager.BombTresholdCheck("bombs", col.gameObject.GetComponent<BombCollectable>().amount);

            // Destroy collectable object as it is now collected
            DestroyObject(col.gameObject, bombPickSound);
        }
    }

    // For objects with collision enabled
    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.tag == "Gate")
        {
            Debug.Log("Collided with Gate");
            if (GameManager.hasKey)
            {
                GameManager.hasKey = false;
                DestroyObject(col.gameObject, gateUnlock);
                
            }
        }

    }

    // Called to destroy collectable items and unlocked gates
    public void DestroyObject(GameObject obj, AudioClip destroySound)
    {
        Destroy(obj);
        audioManager.GetComponent<AudioManager>().Play(destroySound);
    }

    // Used when enemies touch player or player drops to water(removed in v1.0.1)
    public void GetEaten()
    {
        Hold();
        mode = Mode.Dying;
        Debug.Log("Player got eaten!");
        audioManager.GetComponent<AudioManager>().Play(dieSound);
        animator.SetTrigger("GetEaten"); // Animation that triggers Destroy() with animation event
    }

    // Used when player touches explosion
    public void BlowUp()
    {
        Instantiate(blown, transform.position, Quaternion.identity);
        audioManager.GetComponent<AudioManager>().Play(blowUpSound);
        Hold(); // Stop physics
        DestroyObject(gameObject, satanGameOverSound); // Destroy this.gameObject
    }

    // Stop player velocity
    public void Hold()
    {
        rb.velocity = new Vector2(0f, 0f);
    }

    // Triggered with animation event inside "GetEaten". This lets the animation play to the end before restarting the game
    void Destroy()
    {
        audioManager.GetComponent<AudioManager>().Play(satanGameOverSound);
        Destroy(gameObject);
    }
    // Method automatically called when player obj is destroyed
    void OnDestroy()
    {
        // Player acts as a trigger for game over when destroyed
        GameManager.mode = GameManager.Mode.GameOver;
    }

}

