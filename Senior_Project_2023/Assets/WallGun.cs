using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGun : MonoBehaviour
{
    // Checks if player is in range to buy gun from wall
    public bool buy = false;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name == "pawl")   buy = true;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.name == "pawl")   buy = false;
    }
}
