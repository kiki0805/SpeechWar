using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShot : MonoBehaviour
{
    Quaternion bulletDir;      // Direction of bullet
    public float moveSpeed;
    float range;
    int power;
    Quaternion rot;
    public Rigidbody2D rb;
    float x;
    float y;

    // Constructor
    public void Setup(Quaternion bulletDir, float range, int power)
    {
        this.bulletDir = bulletDir;
        this.range = range;

        transform.rotation = bulletDir; // Face direction of character
        Destroy(gameObject, range);     // Destroy bullet after a certain time (range)

    }

    // Move bullet each frame
    private void Update()
    {
        Vector3 a = new Vector3(x, y, 0);
        rb.velocity = transform.right * moveSpeed;
    }
}
