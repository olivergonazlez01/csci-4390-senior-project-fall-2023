using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinder : MonoBehaviour
{
    // Check if player is touching this tile
    void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("touched");
    }
}
