using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private const float SPEED = 3.0f;
    private Vector2 _velocity = Vector2.zero;

    public byte health = 100;

    public GameObject Player;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the zombie has touched the player
        //if (Player.health)

        // Follow player around the map no matter the distance
        Vector2 dir = Player.transform.position - transform.position;
        _velocity = dir.normalized * SPEED;
        transform.position = transform.position + (Vector3)(_velocity * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
