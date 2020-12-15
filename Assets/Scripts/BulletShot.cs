using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    Vector3 bulletDir;          // Direction of bullet
    public float moveSpeed;
    float range;                // Time in seconds before bullet is destroyed
    int power;                  // How much health the bullet will take/give
    bool isDamagingShot;        // Keeps track if it's healing or damaging shot
    Quaternion rot;
    public Rigidbody2D rb;      // This rb should be attached to bullet prefab
    
    // These two floats used to get the proper movement angle
    float x;
    float y;

    /*  Constructor for bullet objects. Called in CharacterBase script */
    public void Setup(Vector3 bulletDir, float range, int power)
    {
        this.bulletDir = bulletDir;
        this.range = range;

        transform.rotation = Quaternion.Euler(bulletDir);   // Spawn bullet at the facing direction of character
        Destroy(gameObject, range);                         // Destroy bullet after a certain time (range)
    }

    /*  Method for comparing vectors (because I do not trust Unity's built in comparison)
        @param v1, v2: Two vectors
        @return boolean representing if vectors are close enought to be considered equal or not
    */
    public bool V3Equal(Vector3 v1, Vector3 v2)
    {
        return Vector3.SqrMagnitude(v1 - v2) < 0.0001;
    }

    /* Collision handler method
        @param collisionObject: Object which bullet is colliding with
    */
    public void OnCollisionEnter2D(Collision2D collisionObject)
    {
        if (collisionObject.transform.CompareTag("Wall"))
        {
            Vector3 newDirection = Vector3.Reflect(rb.velocity, collisionObject.contacts[0].normal);

            // Ugly solution but for some reason if inDirection = (0,0) => newDirection = (0,0,0)
            if(V3Equal(newDirection, Vector3.zero)) // If inDirection is perpendicular to walls normal
            {
                newDirection = collisionObject.contacts[0].normal;  // Let bullet reflect 180 degrees
                bulletDir = newDirection;
                transform.rotation = Quaternion.Euler(-bulletDir);  // Rotate bullet 'front' 180 degrees to face new direction
            }
            else
            {
                bulletDir = newDirection;
                transform.rotation = Quaternion.Euler(bulletDir);   // Face new direction
            }
        }
        else if(collisionObject.transform.CompareTag("Character"))
        {
            // Adjust character health accordingly
        }
    }

    // Move bullet each frame
    private void Update()
    {
        rb.velocity = bulletDir * moveSpeed;
    }
}
