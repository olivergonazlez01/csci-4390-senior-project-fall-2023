using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinder : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Check if player is touching this tile
    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("touched");
    }
}
