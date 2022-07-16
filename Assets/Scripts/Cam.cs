using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private Transform playerPosition;
    void Update()
    {
        if (playerPosition == null)
        {
            playerPosition = GameObject.FindGameObjectWithTag("Player").transform;
        }
        else
        {
            transform.position = new Vector3(playerPosition.position.x, playerPosition.position.y, transform.position.z);
            
        }
    }
}

