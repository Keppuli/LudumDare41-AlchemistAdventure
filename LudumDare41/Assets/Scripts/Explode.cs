using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour {

    public float explosionTimer;
    public float explosionRadius;
    public GameObject explosionRadiusObj;
    public GameObject explosion;
    public bool allowExplosion;
    public string explosionType;

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
                else if (colObj.tag == "Player")
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
            }

        }
        Destroy(gameObject);

    }

}
