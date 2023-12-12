using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hinder : MonoBehaviour
{
    public SFX_Controller soundController;

    void Start()
    {
        soundController = GameObject.Find("SFX_Controller").transform.GetComponent<SFX_Controller>();
    }

    // Checks if player is in tile
    // adds slow effect on player
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.transform.name == "pawl") {
            collider.transform.GetComponent<Player>().Slow_Player(true);
            soundController.playCornLoop(true);
        }
    }

    // Removes slow effect on player
    void OnTriggerExit2D(Collider2D collider) {
        if (collider.transform.name == "pawl") {
            collider.transform.GetComponent<Player>().Slow_Player(false);
            soundController.playCornLoop(false);
        }
    }

    
}
