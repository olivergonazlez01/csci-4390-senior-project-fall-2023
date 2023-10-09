using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private const float SPEED = 5.0f;
    private Vector2 _velocity = Vector2.zero;
    public Animator animator;
    public SpriteRenderer pawl;
    public Health GUI;
    GameObject center;

    // PLayer has 3 chances of getting hit by the zombies before dying
    public byte health = 3;
    public float autoRegenTimer = 0.0f;
    public bool autoRegen = false;
    public float safeTimer = 0.0f;
    public bool safeTime = false;


    // Start is called before the first frame update
    void Start()
    {
        center = transform.Find("center").gameObject;
        Debug.Log(center.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
        center.transform.localPosition = new Vector2(-0.01f, -0.15f);
        
        // Checks if the player has ran out of health, and ends the game if it has (FOR NOW)
        if (health == 0)
        {
            // quit either from the editor or from a built application
            #if UNITY_EDITOR
                // Display how long the player was alive
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        // safeTime makes sure the player a downtime to the zombies' hits
        if(safeTime)
            safeTimer += Time.deltaTime;

        // Checks if the safetimer is out of time, and turns of the safeTime
        if (safeTimer >= 1.5f)
        {
            safeTime = false;
            safeTimer = 0.0f;
        }

        // Checks if autoRegen can be started
        if (health < 3)
        {
            autoRegen = true;
        }

        // Begins autoRegenTimer
        if (autoRegen)
        {
            autoRegenTimer += Time.deltaTime;
        }

        // Increments health by one
        if (autoRegenTimer >= 5.0f)
        {
            health++;
            autoRegen = false;
            autoRegenTimer = 0.0f;
        }


        // convert user input into a movement direction (could use Unity axis for this)
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) dir += Vector2.right;
        if (Input.GetKey(KeyCode.A)) dir += Vector2.left;
        if (Input.GetKey(KeyCode.W)) dir += Vector2.up;
        if (Input.GetKey(KeyCode.S)) dir += Vector2.down;

        // set velocity based on movement direction
        _velocity = dir.normalized * SPEED;
        animator.SetFloat("speed", dir.magnitude);
        if (Input.GetAxisRaw("Horizontal") > 0) {
            pawl.flipX = true;
        } else if (Input.GetAxisRaw("Horizontal") < 0) {
            pawl.flipX = false;
        }
        // integrate velocity to update position
        transform.position = transform.position + (Vector3)(_velocity * Time.deltaTime);
    }

    // Check if the player has hit by the zombie
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Zombie" && !safeTime)
        {
            health--;
            safeTime = true;
        }
    }

    // Check if player is close enough to upgrade bench and presses 'E'
    void OnTriggerStay2D(Collider2D collider) {
        if (collider.gameObject.CompareTag("Interactable") && Input.GetKey(KeyCode.E)) {
            Debug.Log("opened");
        }
    }
}
