using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    public float explosionTimer;
    public float explosionRadius;
    public GameObject explosionRadiusObj;
    public GameObject explosion;
    public bool allowExplosion;
    public string explosionType;
    BoxCollider2D col;
    private void Awake()
    {
        col = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        if (allowExplosion)
        {
            if (explosionTimer > 0)
                explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0)
                Blast(explosionType);
        }
    }

    public void Blast(string type)
    {
        //col.enabled = true;
        
        //Debug.Log("Bomb destroyed.");
        Vector3 ExplosionPos = new Vector3(transform.position.x, transform.position.y - 0.2f);
        GameObject newExplosion = Instantiate(explosion, ExplosionPos, Quaternion.identity);
        Instantiate(explosionRadiusObj, ExplosionPos, Quaternion.identity);
        if (type == "freeze")
        {
            newExplosion.GetComponent<Explosion>().type = Explosion.Type.Freeze;
        }
        //Debug.Log("Explosion at: " + origin);
        var colliders = Physics2D.OverlapCircleAll(ExplosionPos, explosionRadius);
        Debug.Log("Obj in radius = " + colliders.Length);
        // For each object in explosion radius, check if something is on the way

        foreach (var collider in colliders)
        {
            GameObject colObj = collider.transform.gameObject;
            if (colObj.tag != "Indestructible" && colObj != this.gameObject)
            {
                if (colObj.tag == "Player")
                {
                    colObj.GetComponent<Player>().BlowUp();
                }
                else if (colObj.tag == "Skelly")
                {
                    colObj.GetComponent<Enemy>().BlowUp();
                }
                else if (colObj.tag == "Destructible")
                {
                    Destroy(colObj);
                }
                else if (colObj.tag == "Water" && type != "freeze")
                {
                    colObj.GetComponent<WaterIce>().UnFreeze();
                }
                else if (colObj.tag == "Water" && type == "freeze")
                {
                    colObj.GetComponent<WaterIce>().Freeze();
                }
                else if (colObj.tag == "Barrel")
                {
                    Debug.Log("Explosion hit barrel");
                    colObj.GetComponent<Explode>().allowExplosion = true;
                }
                else if (colObj.tag == "Bone")
                {
                    Debug.Log("Explosion hit bone");
                    colObj.GetComponent<Rigidbody2D>().AddExplosionForce(1009, transform.position, 1500);
                }
            }
        }

        Destroy(gameObject);
    }
    /*
    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject colObj = col.transform.gameObject;

        if (colObj.tag != "Indestructible" && colObj != this.gameObject)
        {
            if (colObj.tag == "Player")
            {
                Debug.Log("TRIGGER PLAUER");
                colObj.GetComponent<Player>().BlowUp();
            }
            else if (colObj.tag == "Skelly")
            {
                colObj.GetComponent<Enemy>().BlowUp();
            }
            else if (colObj.tag == "Destructible")
            {
                Destroy(colObj);
            }
            else if (colObj.tag == "Water")
            {
                Debug.Log("TRIGGER WATER");
                colObj.GetComponent<WaterIce>().UnFreeze();
        
                if (explosionType == "freeze")
                {
                    Debug.Log("TRIGGER WATER F");
                    colObj.GetComponent<WaterIce>().Freeze();
                }
                else
                {
                    Debug.Log("TRIGGER WATER NORMAL");
                    colObj.GetComponent<WaterIce>().UnFreeze();
                }
            }
            else if (colObj.tag == "Barrel")
            {
                Debug.Log("Explosion hit barrel");
                colObj.GetComponent<Explode>().allowExplosion = true;
            }
            else if (colObj.tag == "Bone")
            {
                Debug.Log("Explosion hit bone");
                colObj.GetComponent<Rigidbody2D>().AddExplosionForce(1009, transform.position, 1500);
            }
        }
    }
    */
}