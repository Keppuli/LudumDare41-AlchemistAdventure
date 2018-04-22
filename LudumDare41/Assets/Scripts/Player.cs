using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject bomb;
    public GameObject blown;
    public AudioClip keyPick;
    public AudioClip gateUnlock;

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
        //Ensure the script is not deleted while loading
  
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
            if (GameManager.bombs > 0)
            { 
                if (Input.GetKeyDown("space") || Input.GetKeyDown("e"))
                {
                    //spaceKeyFree = false;   // Reserve space key for the duration of this frame
                    Instantiate(bomb, transform.position, Quaternion.identity);
                    GameManager.bombs -= 1;
                }
            }
        }

    } // End update
 
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
        if (Input.GetAxisRaw("Horizontal") < 0 && facingRight)    // Check against player input instead of rb.velocity, because slimes push player
            Flip();
        else if (Input.GetAxisRaw("Horizontal") > 0 && !facingRight)
            Flip();

    }
    void FixedUpdate()
    {
        if (!disableInput)
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
            GameObject targetPortal = col.GetComponent<Portal>().targetPortal;
            if (!col.GetComponent<Portal>().disabled)
            {
                Vector3 targetPortalPos = targetPortal.transform.position;
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
            GameManager.bombs += 1;
            audioManager.GetComponent<AudioManager>().Play(keyPick);
            Destroy(col.gameObject);
        }


    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Debug.Log("Player collided with Enemy");
            audioSource.PlayOneShot(dieSound, 1f);
            audioManager.GetComponent<AudioManager>().Play(dieSound);

            Destroy(gameObject);
        }
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
    }
    public void BlowUp()
    {
        Instantiate(blown, transform.position, Quaternion.identity);
        audioSource.PlayOneShot(dieSound, 1f);
        audioManager.GetComponent<AudioManager>().Play(blowUpSound);

        Destroy(gameObject);
    }

    public void Hold()
    {
        rb2d.velocity = new Vector2(0f, 0f);
    }

    private void OnDestroy()
    {
        GameManager.mode = GameManager.Mode.GameOver;
    }

}

