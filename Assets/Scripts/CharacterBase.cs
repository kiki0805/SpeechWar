using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Separate class for speech input */
public static class MoveStatus
{
    public const string Stop = "stop";
    public const string Left = "left";
    public const string Right = "right";
    public const string Up = "up";
    public const string Down = "below";
    public const string TurnRight = "turn";
    public const string TurnLeft = "back";
    public const string Shoot = "shoot";
}

public class CharacterBase : MonoBehaviour
{
    /* Public variables, set these in Unity Inspector*/
    public float speed;
    public bool isHealer;                           // Check if healer or not
    public float rotationSpeed;
    public float range;                             // Indicates after how many seconds the bullet disappears
    public int power;                               // Indicates how much health a bullet takes
    public float startingHealth;                    // Character starting health
    public GameManager gameManager;

    public bool dead = false;

    public float health;                                   // Update health
    Rigidbody2D rb;
    string moveStatus;                              // Used for speech control
    float xDirection, yDirection;
    bool isActive = false;                          // Control which character is active
    [HideInInspector] public bool mode;             // Input mode, true = moving, false = aiming ** This is now deprecated
    [SerializeField] private Transform pfBullet;    // Is needed to instantiate bullet objects
    PlayerManager manager;
    SpriteRenderer m_SpriteRenderer;
    bool directionMode = true;                      // Used for keyboard control to switch between aiming and moving

    Material matWhite;
    Material matDefault;

    /* Setup */
    void Start()
    {
        health = startingHealth;
        moveStatus = MoveStatus.Stop;
        rb = GetComponent<Rigidbody2D>();
        manager = GetComponentInParent<PlayerManager>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        matWhite = Resources.Load("WhiteFlash.mat", typeof(Material)) as Material;
        matDefault = m_SpriteRenderer.material;
    }

    /* Method for setting character as active */
    public void SetActive()
    {
        if (dead) return;
        isActive = true;
        if (m_SpriteRenderer is null)
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = Color.red;
    }

    /* Method for setting character as inactive */
    public void SetInactive()
    {
        rb.velocity = new Vector2(0, 0);
        isActive = false;
        UpdateMoveStatus(MoveStatus.Stop);
        StopMovement();
        if (dead) return;
        m_SpriteRenderer.color = Color.white;
    }

    private void Die()
    {
        dead = true;
        m_SpriteRenderer.color = Color.grey;
        manager.RemoveCharacter();
    }

    /* Updates move status, is done in SpeechController */
    public void UpdateMoveStatus(string status)
    {
        moveStatus = status;
        //Debug.Log("Updated moveStatus to " + moveStatus);
    }

    /* Method to switch between aiming and moving (KEYBOARD CONTROL ONLY) */
    private void SwitchMode()
    {
        StopMovement();                     // Prevent gliding when switching from movement to direction
        directionMode = !directionMode;
    }

    /* Stop movement */
    public void StopMovement()
    {
        xDirection = 0;
        yDirection = 0;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
    }

    /* Move x,y position on the map */
    private void MovePosition()
    {
        if (!manager.speechInput)   // If speech input not on, move character with keyboard
        {
            xDirection = Input.GetAxis("Horizontal");       // A,D keys
            yDirection = Input.GetAxis("Vertical");         // W,S keys
        }
        else
        {
            switch (moveStatus)     // Else update status and move character
            {
                case MoveStatus.Stop:
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

        // Update velocity accordingly
        Vector3 moveDirection = new Vector3(xDirection, yDirection, 0.0f);
        rb.velocity = moveDirection * speed;
    }

    public void TurnLeft()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    public void TurnRight()
    {
        transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
    }
    // Move direction on the map
    private void MoveDirection()
    {
        if (manager.speechInput)
        {
            switch (moveStatus)     // Else update status and move character
            {
                case MoveStatus.TurnRight:
                    TurnRight();
                    break;
                case MoveStatus.TurnLeft:
                    TurnLeft();
                    break;
                default:
                    break;
            }
            return;
        }
        if (Input.GetKey(KeyCode.A))
            TurnLeft(); // forward is z-axis (towards us)

        if (Input.GetKey(KeyCode.D))
            TurnRight();
    }

    /* Catch-all method that calls the right function depending on what mode we are in */
    public void MoveCharacter()
    {
        if (manager.speechInput)
        {
            MoveDirection();
            MovePosition();
            return;
        }

        if (directionMode)
            MoveDirection();
        else
            MovePosition();

        // For keyboard-input only
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchMode();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            ShootBullet();
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) // Skip turn
        {
            Input.ResetInputAxes();
            gameManager.ChangePlayer();
        }
    }

    public void ShootBullet()
    {
        StopMovement();
        if (gameManager is null)
        {
            gameManager = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>();
        }
        gameManager.EndTurn();

        Vector3 currentPosition = transform.position + (transform.right * (float)0.30);             // Spawn bullet in front of character
        Transform bulletTransform = Instantiate(pfBullet, currentPosition, Quaternion.identity);    // Create new bullet prefab

        // Calculate bullet movement direction from knowing z-rotation (magnitude of angle in unit circle)
        float z = transform.localRotation.eulerAngles.z;     // Get rotation around z-axis as Euler angles in degrees
        float y = Mathf.Sin(z * Mathf.Deg2Rad);              // Mathf.Sin and Mathf.Cos both calculate from radians
        float x = Mathf.Cos(z * Mathf.Deg2Rad);              // so we need to convert first
        Vector3 bulletDirection = new Vector3(x, y, z);
        bulletTransform.GetComponent<BulletShot>().Setup(bulletDirection, range, power, !isHealer); // if isHealer = true => !isHealer = false => isBullet = false
    }

    /* Adjust character health accordingly */
    public void ChangeCharacterHealth(bool isBullet, int power)
    {
        if (dead) return;

        if (isBullet)
        {
            health -= power;
            if (health <= 0)
            {
                health = 0;
                Die();
                SetInactive();
                //Debug.Log("Character died! Health is now: " + health);
            }
            else
            {
                m_SpriteRenderer.material = matWhite;
                Invoke("ResetMaterial", .2f);
                //Debug.Log("Character hit! Health is now: " + health);
            }
        }
        else
        {
            health += power;
            if (health >= startingHealth)
            {
                health = startingHealth;
            }
            //Debug.Log("Character healed! Health is now: " + health);
        }
        rb.velocity = Vector3.zero;
    }

    public float GetHealth()
    {
        return health;
    }

    void ResetMaterial()
    {
        m_SpriteRenderer.material = matDefault;
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        MoveCharacter();
    }
}
