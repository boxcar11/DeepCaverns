using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        string doorColor = name.Split(' ')[0];
        if (other.tag == "Player" && GameManager.Instance.HasKey(doorColor))
        {
            Destroy(gameObject);
        }
    }
}
