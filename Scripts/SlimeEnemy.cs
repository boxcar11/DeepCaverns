using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    Vector3 startPosition;
    Vector3 endPosition;
    float travelLength;
    float startTime;

    int r; // Reverse int 1 or -1

    void FixedUpdate()
    {
        // BUG Enemy gets stuck when pushed in to wall
        if (Physics2D.Raycast(transform.position, Vector3.right, rayLength, mask) && reverse == false)
        {
            //Debug.Log("Enemy hit something");
            reverse = !reverse;
        }
        else if (Physics2D.Raycast(transform.position, Vector3.left, rayLength, mask) && reverse)
        {
            //Debug.Log("Hit Left");
            reverse = !reverse;
        }

        if (reverse == false)
        {
            Debug.DrawRay(transform.position, Vector3.right * rayLength, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, Vector3.left * rayLength, Color.red);
        }

        r = reverse ? -1 : 1;
    }

    void Move()
    {


        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x + r, startPosition.y, startPosition.z);

        travelLength = Vector3.Distance(transform.position, endPosition);
        startTime = Time.time;
        StartCoroutine(IMove());
    }

    System.Collections.IEnumerator IMove()
    {


        while (transform.position != endPosition)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float FractionOfTravel = distCovered / travelLength;

            transform.position = Vector3.Lerp(startPosition, endPosition, FractionOfTravel);
            yield return new WaitForSeconds(.1f);
        }

    }
}
