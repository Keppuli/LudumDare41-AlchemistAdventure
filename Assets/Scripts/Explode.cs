using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    public float explosionTimer;
    public float explosionRadius;
    public GameObject explosionRadiusObj; // Circular explosion sprite indicating radius of explosion
    public GameObject explosion;          // Reference to explosion instance
    public bool allowExplosion;           // Used to prevent barrel from exploding on it's own
    public string explosionType;          // Set by bomb.cs

    void Update()
    {
        if (allowExplosion) // Check for barrel
        {
            // Count down to explosion and call blast
            if (explosionTimer > 0)
                explosionTimer -= Time.deltaTime;
            if (explosionTimer <= 0)
                Blast(explosionType);
        }
    }

    public void Blast(string type)
    {
        // Explode under the bomb/barrel with slight Y-axis offset
        Vector3 ExplosionPos = new Vector3(transform.position.x, transform.position.y - 0.2f);

        // Create reference to the instantiated explosion
        GameObject newExplosion = Instantiate(explosion, ExplosionPos, Quaternion.identity);

        // Also spawn ring sprite to indicate explosion radius to player
        Instantiate(explosionRadiusObj, ExplosionPos, Quaternion.identity);

        // If bomb is type freeze, change the type of explosion
        if (type == "freeze")
        {
            newExplosion.GetComponent<Explosion>().type = Explosion.Type.Freeze; // Freeze type explosion freezes water
        }

        // Create circle raycast and store all collided objects(having Collider2D component with collision enabled) inside array of Collider2D's
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ExplosionPos, explosionRadius);
        Debug.Log("Obj's in blast() radius = " + colliders.Length);

        // For each obj in the array we do some checks
        foreach (var collider in colliders)
        {
            // Set quick reference to the obj we are iterating
            GameObject colObj = collider.transform.gameObject;

            // Skip everything if obj is indestructible or self
            if (colObj.tag != "Indestructible" && colObj != gameObject)
            {
                // Find out the obj type from tag property and decide what to do
                if (colObj.tag == "Player")
                {
                    colObj.GetComponent<Player>().BlowUp();
                }
                else if (colObj.tag == "Skelly")
                {
                    colObj.GetComponent<Enemy>().BlowUp(transform.position); // Feed explosion position for bone add force calculation
                }
                else if (colObj.tag == "Destructible")
                {
                    Destroy(colObj);
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
                    colObj.GetComponent<Bone>().AddExplosionForce(transform.position);
                }
            }
        }
        // Delete object as it has served it's purpose, to prevent multiple explosions
        Destroy(gameObject);
    }
   
}