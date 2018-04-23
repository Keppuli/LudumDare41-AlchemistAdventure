using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bomb;
    public GameObject blown;
    public AudioClip keyPick;
    public AudioClip bombPickSound;

    public AudioClip gateUnlock;
    public AudioClip satanGameOverSound;

    public AudioClip dieSound;
    public AudioClip blowUpSound;
    public AudioClip teleportSound;
    public GameObject audioManager;

    public bool disableInput;
    public bool facingRight;
    public enum Mode { None,Teleporting,Dying};
    public Mode mode;
    public float moveSpeed;
    public float curSpeed;
    public float movementModifier;
    private AudioSource audioSource;
    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        GameManager.mode = GameManager.Mode.GameOn;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mode = Mode.None;
    }
    private void Update()
    {
        Animate();
        if (mode == Mode.None)
        {
            //bool spaceKeyFree = true; // Prevents Space doing multiple actions at single frame
            // HANDLE USE KEY
           
            if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
            {
                Vector3 bombPos = new Vector3(transform.position.x, transform.position.y+0.2f);
                //spaceKeyFree = false;   // Reserve space key for the duration of this frame
                DropBomb();
            }
        }

    } // End update
 
    void DropBomb()
    {
        Vector3 bombPos = new Vector3(transform.position.x, transform.position.y + 0.2f);
        var bombType = GameManager.bombType;
        if (bombType == GameManager.BombType.Normal)
        {
            if (GameManager.bombs > 0)
            {
                GameObject newBomb = Instantiate(bomb, bombPos, Quaternion.identity);
                newBomb.GetComponent<Bomb>().type = Bomb.Type.Normal;
                Debug.Log("Placed Normal bomb");

            }
        }
        if (bombType == GameManager.BombType.Freeze)
        {
            if (GameManager.bombsFreeze > 0)
            {
                GameObject newBomb = Instantiate(bomb, bombPos, Quaternion.identity);
                newBomb.GetComponent<Bomb>().type = Bomb.Type.Freeze;
                Debug.Log("Placed Freezebomb");
            }
        }

    }

    void Animate()
    {
 
        float absHS = Mathf.Abs(rb2d.velocity.x);
        float absVS = Mathf.Abs(rb2d.velocity.y);
        if (absHS > 0 || absVS > 0) // Check against absolute value
        {
            animator.SetTrigger("Walk");
        }
        else
        {
            animator.SetTrigger("Idle");
        }
        if (Input.GetAxisRaw("Horizontal") < 0 && facingRight)
            Flip();
        else if (Input.GetAxisRaw("Horizontal") > 0 && !facingRight)
            Flip();
        
    }
    void FixedUpdate()
    {
        if (mode == Mode.None)
        {
        // ANIMATION
        Animate();
            // MOVEMENT
            curSpeed = moveSpeed;
        
            float horizontalSpeed = Input.GetAxisRaw("Horizontal");// GetAxis vs GetAxisRaw?
            float verticalSpeed = Input.GetAxisRaw("Vertical");
            rb2d.velocity = new Vector2(Mathf.Lerp(0, horizontalSpeed * curSpeed * movementModifier, 1f), Mathf.Lerp(0, verticalSpeed * curSpeed * movementModifier, 1f));
        }

    }
    void Flip() // Flips the player sprite
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Portal")
        { 
            Debug.Log("Collided with Portal");
            GameObject targetPortal = col.GetComponent<Portal>().targetPortal; // Destination portal where this portal is linked
            if (col.GetComponent<Portal>().isEndPortal)
            {
                audioManager.GetComponent<AudioManager>().Play(teleportSound);
                GameManager.LoadNextLevel();
            }
            else if (!col.GetComponent<Portal>().disabled)
            {
                Vector3 targetPortalPos = targetPortal.transform.position;
                if (targetPortal.GetComponent<Portal>())    // Check if target portal is actually portal (other obj's can be used for one way tele)
                    targetPortal.GetComponent<Portal>().disabled = true;

                gameObject.transform.position = new Vector2(targetPortalPos.x, targetPortalPos.y);
                audioManager.GetComponent<AudioManager>().Play(teleportSound);
            }

        }
        if (col.tag == "Key")
        {
            Debug.Log("Trigger with Key");
            GameManager.hasKey = true;
            audioManager.GetComponent<AudioManager>().Play(keyPick);
            Destroy(col.gameObject);
        }
        if (col.tag == "BombCollectable")
        {
            Debug.Log("Trigger with BombC");
            if (col.gameObject.GetComponent<BombCollectable>().type == BombCollectable.Type.Freeze)
                GameManager.bombsFreeze += col.gameObject.GetComponent<BombCollectable>().amount;
            else
                GameManager.bombs += col.gameObject.GetComponent<BombCollectable>().amount;

            audioManager.GetComponent<AudioManager>().Play(bombPickSound);
            Destroy(col.gameObject);
        }


    }
    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.tag == "Gate")
        {
            Debug.Log("Collided with Gate");
            if (GameManager.hasKey)
            {
                GameManager.hasKey = false;
                Destroy(col.gameObject);
                audioManager.GetComponent<AudioManager>().Play(gateUnlock);
            }
        }
        if (col.gameObject.tag == "Water")
        {
            Debug.Log("Collided with Water");

            GetEaten();
           
        }
    }

    public void GetEaten()
    {
        Hold();
        mode = Mode.Dying;
        Debug.Log("Player got eaten!");
        audioManager.GetComponent<AudioManager>().Play(dieSound);
        animator.SetTrigger("GetEaten");
    }
    public void BlowUp()
    {
        Instantiate(blown, transform.position, Quaternion.identity);
        audioManager.GetComponent<AudioManager>().Play(blowUpSound);
        Hold();
        Destroy();
    }

    public void Hold()
    {
        rb2d.velocity = new Vector2(0f, 0f);
    }

    public void Destroy()
    {
        audioManager.GetComponent<AudioManager>().Play(satanGameOverSound);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        GameManager.mode = GameManager.Mode.GameOver;
    }

}

