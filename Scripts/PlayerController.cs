using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10;
    public float climbSpeed = 5;
    public float jumpPower = 8;

    public Slider healthSlide;
    public Animator animator;

    Vector2 moveDirection;
    bool isGrounded = true;
    LayerMask mask;
    Rigidbody2D rb;

    float grabTimer;
    float grabTime = 1; // How long the player has to reach a rope
    bool grabbing = false; // If player is trying to grab a rope
    bool Holding = false; // If the player is holding on to a rope

    public float maxPlayerHealth = 100; // Health player starts game with
    float playerHealth; // Current player health

    float lastVelocity; // y Velocity of last frame
    float lastIsGroundY; // World y position at last isGround

    GameObject destroyable; // Object in range of player that can be destroyed
    GameObject enemy; // Enemy in range

    MenuController menu; // Reference to MenuController on GameManager GameObject

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Ground");
        rb = GetComponent<Rigidbody2D>();
        playerHealth = maxPlayerHealth;
        healthSlide.maxValue = maxPlayerHealth;
        menu = GameManager.Instance.gameObject.GetComponent<MenuController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
        {
            moveDirection.y = 0;
            rb.gravityScale = 1;
        }

        // If timer reaches zero before player touches rope the player stops trying to grab rope
        if (grabTimer <= 0)
        {
            grabbing = false;
        }
        else
        {
            grabTimer -= Time.deltaTime;
        }

        healthSlide.value = playerHealth;

        // Increase players health over time, but don't let health get above max
        playerHealth += .1f * Time.deltaTime;
        if (playerHealth > maxPlayerHealth)
        {
            playerHealth = maxPlayerHealth;
        }

        if (playerHealth <= 0)
        {
            GameManager.Instance.AddDeath();
            playerHealth = maxPlayerHealth;
            gameObject.transform.position = GameManager.Instance.GetCheckpointPos();
        }
    }

    void FixedUpdate()
    {
        Vector3 dir = new Vector3(moveDirection.x, moveDirection.y, 0) * Time.deltaTime;
        animator.SetFloat("IsWalking", moveDirection.x);
        gameObject.transform.Translate(dir);
        //Debug.Log(moveDirection.y);

        float curVelocity = rb.velocity.y;

        if (curVelocity <= .5 && curVelocity >= -.5)
        {
            float yDelta = Mathf.Abs(lastVelocity - curVelocity);
            if (yDelta > 8 && (lastIsGroundY - transform.position.y) > 5)
            {
                float amountToRemove = Mathf.Pow(yDelta, 4) / 800;
                Debug.Log("Remove: " + amountToRemove);
                //Debug.Log("Y diff: " + (lastIsGroundY - transform.position.y));
                //Debug.Log("Last Y: " + lastIsGroundY);
                //Debug.Log("Cur Y: " + transform.position.y);
                RemoveHealth(amountToRemove);

            }
            lastVelocity = 0;
        }
        else
        {
            lastVelocity = curVelocity;
        }
    }

    public bool GetHolding()
    {
        return Holding;
    }

    public void OnMovement(InputValue value)
    {
        if (Holding == false)
        {
            moveDirection.x = value.Get<float>() * moveSpeed;
        }
    }

    public void OnClimb(InputValue value)
    {
        if (Holding)
        {
            moveDirection.y = value.Get<float>() * climbSpeed;
        }
    }

    public void OnJump()
    {

        moveDirection.y = jumpPower;
        isGrounded = false;
        Holding = false;
        rb.gravityScale = 2;
        lastIsGroundY = transform.position.y;

    }

    public void OnGrab()
    {
        if (Holding)
        {
            Holding = false;
            rb.gravityScale = 1;
            lastIsGroundY = transform.position.y;
        }
        else
        {
            grabbing = true;
            grabTimer = grabTime;
        }
    }

    public void OnFire()
    {
        if (destroyable)
        {
            Destroy(destroyable);
        }
        else if (enemy)
        {
            enemy.GetComponent<Enemy>().Hit();
        }
    }

    public void OnToggleMenu()
    {
        menu.MenuButtonPressed();
    }

    public void AddHealth(float h)
    {
        playerHealth += h;
    }

    public void RemoveHealth(float h)
    {
        playerHealth -= h;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            isGrounded = false;
            lastIsGroundY = transform.position.y;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Pickup")
        {
            Debug.Log("Picked up coin");
            GameManager.Instance.AddCoin();
            other.GetComponent<AudioSource>().Play();
            Destroy(other.gameObject, .25f);
        }
        else if (other.tag == "Destroyable")
        {
            destroyable = other.gameObject;
        }
        else if (other.tag == "Enemy")
        {
            enemy = other.gameObject;
        }
        else if (other.tag == "Key")
        {
            string keyColor = other.gameObject.name.Split(' ')[0];
            GameManager.Instance.AddKey(keyColor);
            Destroy(other.gameObject);
        }
        else if (other.tag == "Spike")
        {
            RemoveHealth(200);
        }
        else if (other.tag == "HealthPickup")
        {
            AddHealth(other.GetComponent<FoodPickController>().healthAmount);
            Destroy(other.gameObject);
        }

        if (other.tag == "Finish")
        {
            GameManager.Instance.Win();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Rope")
        {
            //Debug.Log("Hit rope");
            if (grabbing)
            {
                //Debug.Log("Grabbing rope");
                rb.gravityScale = 0;
                rb.velocity = new Vector2(0, 0);
                moveDirection.x = 0;
                moveDirection.y = 0;

                grabbing = false;
                Holding = true;
                other.GetComponent<RopeController>().SetPlayer(this);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Destroyable")
        {
            destroyable = null;
        }
        else if (other.tag == "Enemy")
        {
            enemy = null;
        }
    }
}
