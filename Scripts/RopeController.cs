using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public GameObject[] endStop;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (player.GetHolding())
            {
                for (int i = 0; i < endStop.Length; i++)
                {
                    endStop[i].SetActive(true);
                }

            }
            else
            {
                for (int i = 0; i < endStop.Length; i++)
                {
                    endStop[i].SetActive(false);
                    player = null;
                }
            }
        }
    }

    public void SetPlayer(PlayerController pc)
    {
        player = pc;
    }
}
