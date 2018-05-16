using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public AudioClip attackSound;
    public AudioClip dieSound;
    public AudioClip teleportSound;

    // References to instances which are spawned when enemy(skeleton) dies
    public GameObject bone; 
    public GameObject skull;

    public GameObject triggerObject; // Obj that needs to be destroyed(wall,gate) for the enemy to be activated
    public float moveSpeed = 1f;
    public float moveForceX; // added as velocity to RB2D
    public float moveForceY; // added as velocity to RB2D

    // Basic AI state machine base
    public enum Mode { Attacking, Dying,Idling};
    public Mode mode;

    private Rigidbody2D rb2d;
    public GameObject player;
    private Animator animator;
    private SpriteRenderer sr;
    public GameObject audioManager;

    void Start () {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        // Automatically set some references
        player = GameObject.FindWithTag("Player");
        audioManager = GameObject.FindGameObjectWithTag("AudioManager");

    }

    void Update () {

        if (mode == Mode.Idling)
        {
            animator.SetTrigger("Idle");

            // If trigger obj is destroyed, activate enemy
            if (!triggerObject)
            {
                audioManager.GetComponent<AudioManager>().Play(attackSound);
                mode = Mode.Attacking;
            }
        }

    }

    void FixedUpdate()
    {
        if (mode == Mode.Attacking)
        {
            animator.SetTrigger("Walk");
            MoveToTarget();
        }
        // Reset velocity if mode is suddenly changed(when player dies etc.)
        else
            Hold(); 
            
    }
    // Blow up to pieces (only used for skelly)
    public void BlowUp(Vector3 explosionPosition)
    {
        audioManager.GetComponent<AudioManager>().Play(dieSound);

        // Create few bones
        for (int i = 0; i < 3; i++)
        {
            // Instantiate and make reference to new bone 
            GameObject newBone = Instantiate(bone, transform.position, Quaternion.identity);

            // Feed new bone the information where explosion came from
            newBone.GetComponent<Bone>().explosionPosition = explosionPosition; 
        }
        // Only one skull
        GameObject newSkull = Instantiate(skull, transform.position, Quaternion.identity);
        newSkull.GetComponent<Bone>().explosionPosition = explosionPosition;

        // Destroy skelly
        Destroy(gameObject);
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