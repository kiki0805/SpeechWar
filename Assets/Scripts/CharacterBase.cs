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
    [HideInInspector] public bool mode;              // Whether moving or aiming, true = moving, false = aiming
    PlayerManager manager;

    void Start()
    {
        moveStatus = MoveStatus.Still;
        rb = GetComponent<Rigidbody2D>();
        manager = GetComponentInParent<PlayerManager>();
    }

    public void SetActive()
    {
        isActive = true;
    }

    public void SetInactive()
    {
        isActive = false;
        UpdateMoveStatus(MoveStatus.Still);
    }

    // Updates move status, is done in SpeechController
    public void UpdateMoveStatus(string status)
    {
        moveStatus = status;
        Debug.Log("Updated moveStatus to " + moveStatus);
    }

    // Method to switch between aiming and moving
    private void SwitchMode()
    {
        mode = !mode;
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
        if(rb != null)
            rb.velocity = new Vector2(0f, 0f);
    }

    void FixedUpdate()
    {
        if (mode)
            rb.velocity = new Vector2(xDirection, yDirection);
        else
            rb.velocity = new Vector2(0f, 0f);
    }

    // Move direction on the map
    private void MoveDirection()
    {
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime); // forward is z-axis (towards us)

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    // Catch-all method that calls the right function depending on what mode we are in
    public void MoveCharacter()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchMode();
        }

        if (mode)
        {
            MovePosition();
        }
        else if (!mode)
        {
            MoveDirection();
        }
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
        if (!isActive) return;
        MoveCharacter();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ShootBullet();
        }
    }
}
