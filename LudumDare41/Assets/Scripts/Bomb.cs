using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float explosionTimer;
    public float explosionRadius;
    public GameObject explosion;
    private AudioSource audioSource;

    // Update is called once per frame
    void Update () {
        if (explosionTimer > 0)
        {
            explosionTimer -= Time.deltaTime;

        }
        if (explosionTimer <= 0)
        {
            //Debug.Log("Bomb destroyed.");
            Vector3 ExplosionPos = new Vector3(transform.position.x, transform.position.y - 0.2f);
            Instantiate(explosion, ExplosionPos, Quaternion.identity);
            Explode(transform.position);
            Destroy(gameObject);
        }
    }

    void Explode(Vector2 origin)
    {
        //Debug.Log("Explosion at: " + origin);
        var colliders = Physics2D.OverlapCircleAll(origin, explosionRadius);
        Debug.Log("Obj in radius = " + colliders.Length);
        // For each object in explosion radius, check if something is on the way

        foreach (var collider in colliders)
        {
            GameObject colObj = collider.transform.gameObject;
            if (colObj.tag != "Indestructible")
            {
                if (colObj.tag == "Player")
                {
                    colObj.GetComponent<Player>().BlowUp();
                }
                if (colObj.tag == "Player")
                {
                    colObj.GetComponent<Player>().BlowUp();
                }
                if (colObj.tag == "Skelly")
                {
                    colObj.GetComponent<Enemy>().BlowUp();
                }
                else
                    Destroy(colObj);
            }
            
        }
    }

}
