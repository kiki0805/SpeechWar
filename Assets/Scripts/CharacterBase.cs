using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Separate class for speech input
public static class MoveStatus
{
    public const string Still = "stop";
    public const string Left = "left";
    public const string Right = "right";
    public const string Up = "up";
    public const string Down = "below";
}

public class CharacterBase : MonoBehaviour
{

    [SerializeField] private Transform pfBullet;     // Is needed to instantiate bullet objects
    public float speed;
    float xDirection, yDirection;
    public float rotationSpeed;
    public float range;        // Indicates after how many seconds the bullet disappears
    public int power;          // Indicates how much health a bullet takes
    string moveStatus;  // Will be used for speech control
    Rigidbody2D rb;
    bool isActive = false;
    PlayerManager manager;
    public float healthAmount = 1.0f;
    public bool dead = false;
    SpriteRenderer m_SpriteRenderer;

    void Start()
    {
        moveStatus = MoveStatus.Still;
        rb = GetComponent<Rigidbody2D>();
        manager = GetComponentInParent<PlayerManager>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetActive()
    {
        if (dead) return;
        isActive = true;
        if (m_SpriteRenderer is null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.red;
    }

    public void SetInactive()
    {
        isActive = false;
        UpdateMoveStatus(MoveStatus.Still);
        StopMovement();
        if (dead) return;
        m_SpriteRenderer.color = Color.white;
    }

    public void Shot()
    {
        if (dead) return;
        healthAmount -= 0.1f;
        if (healthAmount <= 0)
        {
            Die();
            SetInactive();
        }
    }

    private void Die()
    {
        dead = true;
        m_SpriteRenderer.color = Color.grey;
    }

    // Updates move status, is done in SpeechController
    public void UpdateMoveStatus(string status)
    {
        moveStatus = status;
        Debug.Log("Updated moveStatus to " + moveStatus);
    }

    // Move x,y position on the map
    private void MovePosition()
    {
        if (!manager.speechInput)
        {
            xDirection = Input.GetAxis("Horizontal");
            yDirection = Input.GetAxis("Vertical");
        }
        else
        {
            switch (moveStatus)
            {
                case MoveStatus.Still:
                    xDirection = 0;
                    yDirection = 0;
                    break;
                case MoveStatus.Right:
                    xDirection = 1;
                    yDirection = 0;
                    break;
                case MoveStatus.Left:
                    xDirection = -1;
                    yDirection = 0;
                    break;
                case MoveStatus.Up:
                    xDirection = 0;
                    yDirection = 1;
                    break;
                case MoveStatus.Down:
                    xDirection = 0;
                    yDirection = -1;
                    break;
                default:
                    xDirection = 0;
                    yDirection = 0;
                    break;
            }
        }

        // Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);

        //transform.position += moveDirection * speed;
    }

    public void StopMovement()
    {
        xDirection = 0;
        yDirection = 0;
        if (rb != null)
            rb.velocity = new Vector2(0f, 0f);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xDirection, yDirection);
    }

    public void TurnLeft()
    {
        if (!manager.speechInput)
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        else
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime * 100);
    }

    public void TurnRight()
    {
        if (!manager.speechInput)
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        else
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime * 100);
    }
    // Move direction on the map
    private void MoveDirection()
    {
        if (manager.speechInput) return;
        if (Input.GetKey(KeyCode.A))
            TurnLeft(); // forward is z-axis (towards us)

        if (Input.GetKey(KeyCode.D))
            TurnRight();
    }

    // Catch-all method that calls the right function depending on what mode we are in
    public void MoveCharacter()
    {
        MovePosition();
        MoveDirection();
    }

    public void ShootBullet()
    {
        Vector3 currentPosition = transform.position + (transform.right * 1); // Spawn bullet in front of character, 1 might be subject to change
        Transform bulletTransform = Instantiate(pfBullet, currentPosition, Quaternion.identity); // Create new bullet prefab

        // Calculate bullet movement direction from knowing z-rotation (magnitude of angle in unit circle)
        float z = transform.localRotation.eulerAngles.z;     // Get rotation around z-axis as Euler angles in degrees
        float y = Mathf.Sin(z * Mathf.Deg2Rad);              // Mathf.Sin and Mathf.Cos both calculate from radians
        float x = Mathf.Cos(z * Mathf.Deg2Rad);              // so we need to convert first
        Vector3 bulletDirection = new Vector3(x, y, z);
        bulletTransform.GetComponent<BulletShot>().Setup(bulletDirection, range, power);
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        MoveCharacter();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShootBullet();
        }
    }
}
