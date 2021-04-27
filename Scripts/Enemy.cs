using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Tooltip("Number of attacks need to kill")]
    public int numberOfAttacks = 1;
    public float moveSpeed = 2;

    protected int numHits;

    protected LayerMask mask;
    protected bool reverse = false;
    protected float rayLength = 1;

    // Start is called before the first frame update
    void Start()
    {
        mask = LayerMask.GetMask("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        if (numHits >= numberOfAttacks)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        // BUG Enemy gets stuck when pushed in to wall
        if (Physics2D.Raycast(transform.position, Vector3.right, rayLength, mask) && reverse == false)
        {
            //Debug.Log("Enemy hit something");
            reverse = !reverse;
        }
        else if (Physics2D.Raycast(transform.position, Vector3.left, rayLength, mask))
        {
            reverse = !reverse;
        }

        /* if (reverse == false)
        {
            Debug.DrawRay(transform.position, Vector3.right * rayLength, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.left * rayLength, Color.red);
        } */

        int r = reverse ? -1 : 1;
        Vector3 dir = new Vector3(moveSpeed * Time.deltaTime * r, 0, 0);

        transform.Translate(dir);
    }

    public void Hit()
    {
        numHits++;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().RemoveHealth(5);

            if (reverse)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-500, -500));
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(500, -500));
            }
        }
    }
}
