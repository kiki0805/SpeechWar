using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    public float moveSpeed;
    public Rigidbody2D rb;      // This rb should be attached to bullet prefab

    Vector3 bulletDir;          // Direction of bullet
    float range;                // Time in seconds before bullet is destroyed
    int power;                  // How much health the bullet will take/give
    bool isBullet;              // Keeps track if it's healing or damaging shot
    GameObject gameManager;

    // These two floats used to get the proper movement angle
    float x;
    float y;

    /*  Constructor for bullet objects. Called in CharacterBase script */
    public void Setup(Vector3 bulletDir, float range, int power, bool isBullet)
    {
        this.bulletDir = bulletDir;
        this.range = range;
        this.isBullet = isBullet;
        this.power = power;

        transform.rotation = Quaternion.Euler(bulletDir);   // Spawn bullet at the facing direction of character
        Destroy(gameObject, range);                         // Destroy bullet after a certain time (range)
    }

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager");
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
        rb.velocity = bulletDir * 0;    // Stop bullet
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
            CharacterBase character = collisionObject.collider.GetComponent<CharacterBase>();

            if (character == null) return;
            if (character.dead) return;
            collisionObject.transform.GetComponent<CharacterBase>().ChangeCharacterHealth(isBullet, power);
            Destroy(gameObject, 0f);

            // Adjust character health accordingly
        }
    }

    void OnDestroy()
    {
        gameManager.GetComponent<GameManager>().StartTurn();
    }

    // Move bullet each frame
    private void Update()
    {
        rb.velocity = bulletDir * moveSpeed;
    }
}
