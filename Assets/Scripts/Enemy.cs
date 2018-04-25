using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public AudioClip attackSound;
    public AudioClip dieSound;
    public AudioClip teleportSound;

    public GameObject bone;
    public GameObject skull;

    public GameObject triggerObject;
    public float moveSpeed = 1f;
    public float moveForceX; // added as velocity to RB2D
    public float moveForceY; // added as velocity to RB2D
    public bool facingRight = false;
    public bool touchingTarget = false;

    public enum Mode { Attacking, Dying,Idling};
    public Mode mode;

    private Rigidbody2D rb2d;
    public GameObject player;
    private AudioSource audioSource;
    private Animator animator;
    private SpriteRenderer sr;
    public GameObject audioManager;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");
        audioSource = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update () {

        if (mode == Mode.Idling)
        {
            animator.SetTrigger("Idle");
        if (!triggerObject)
            {
                audioManager.GetComponent<AudioManager>().Play(attackSound);
                mode = Mode.Attacking;
            }
        }

        if (rb2d.velocity.x < 0f && facingRight)       // vel x -val and facing right
        {
            Flip();
        }
        else if (rb2d.velocity.x > 0f && !facingRight) // vel x +val and facing left
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        if (mode == Mode.Attacking)
        {
            animator.SetTrigger("Walk");
            MoveToTarget();
        }

        else
            Hold(); // Reset velocity
            
    }
    public void BlowUp()
    {
        audioSource.PlayOneShot(dieSound, 1f);
        audioManager.GetComponent<AudioManager>().Play(dieSound);
        Instantiate(bone, transform.position, Quaternion.identity);
        Instantiate(bone, transform.position, Quaternion.identity);
        Instantiate(bone, transform.position, Quaternion.identity);
        Instantiate(skull, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void Flip() // Flips the player sprite
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    void MoveToTarget()
    {
        if (player)
        {
        float distanceX = Mathf.Clamp((player.transform.position.x - transform.position.x), -1f, 1f);
        float distanceY = Mathf.Clamp((player.transform.position.y - transform.position.y), -1f, 1f);
        rb2d.velocity = new Vector2(distanceX, distanceY);
        }
    }

    public void Hold()
    {
        //Debug.Log("Slime Halted.");
        if ((rb2d.velocity.x > 0f) || (rb2d.velocity.y > 0f))
        {
            rb2d.velocity = new Vector2(0f, 0f); // Reset Slime's velocity
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Hold();
            Debug.Log("Enemy is touching Player");
            mode = Mode.Idling;
            col.gameObject.GetComponent<Player>().GetEaten();
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.name == "Player")
        {
            Debug.Log("Enemy is touching Player");
            mode = Mode.Attacking;
        }
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
        if (col.tag == "Player")
        {
            Hold();
            Debug.Log("Enemy is touching Player");
            mode = Mode.Idling;
            col.gameObject.GetComponent<Player>().GetEaten();
        }
    }
}