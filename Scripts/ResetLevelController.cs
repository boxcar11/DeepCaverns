using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetLevelController : MonoBehaviour
{
    GameObject player;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ResetLevel()
    {
        player.transform.position = startPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.name + " triggered");
        if (other.tag == "Player")
        {
            ResetLevel();
        }
    }
}
