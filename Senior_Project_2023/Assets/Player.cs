using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float SPEED = 5.0f;
    private Vector2 _velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // convert user input into a movement direction (could use Unity axis for this)
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) dir += Vector2.right;
        if (Input.GetKey(KeyCode.A)) dir += Vector2.left;
        if (Input.GetKey(KeyCode.W)) dir += Vector2.up;
        if (Input.GetKey(KeyCode.S)) dir += Vector2.down;

        // set velocity based on movement direction
        _velocity = dir.normalized * SPEED;

        // integrate velocity to update position
        transform.position = transform.position + (Vector3)(_velocity * Time.deltaTime);
    }
}
