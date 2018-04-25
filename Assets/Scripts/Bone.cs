using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour {

    private Rigidbody2D rb;
    private float targetTime;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Rotate(Vector3.forward, Random.Range(-45, 45));

        rb.AddExplosionForce(1009, new Vector2(transform.position.x - 0.1f, transform.position.y-0.1f), 1500);

    }
    public void AddExplosionForce() //https://forum.unity.com/threads/need-rigidbody2d-addexplosionforce.212173/#post-1426983
    {
        var dir = (rb.transform.position - new Vector3(0f,0f,0f));
        float wearoff = 1 - (dir.magnitude / 2f);
        rb.AddForce(dir.normalized * 2 * wearoff);
    }
    // Update is called once per frame
    void Update()
    {
        targetTime -= Time.deltaTime;                                           //Count down timer

        if (targetTime <= 0.0f)                                                 //Timer done   
        {
        }
    }
}
