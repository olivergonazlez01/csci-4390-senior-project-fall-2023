using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Controls the sppeed of the character
    private const float SPEED = 5.0f;
    private const float SLOW_SPEED = 2.0f;
    private bool isSlowed = false;
    // Controls velocity of character
    private Vector2 _velocity = Vector2.zero;
    // Controls animator of character
    public Animator animator;
    public SpriteRenderer pawl;
    
    [SerializeField] AudioSource hit;

    // Reference to variables
    public Health GUI;
    GameObject center;
    GameObject gun;
    Transform availableGrenades;
    Transform availableYarn;
    UI ui;

    // PLayer has 2 chances of getting hit by the zombies before dying
    public byte health = 2;
    // Timer for auto regeneration
    public float autoRegenTimer = 0.0f;
    public bool autoRegen = false;
    // Timer for protection against instantanious zombie hits
    public float safeTimer = 0.0f;
    public bool safeTime = false;

    void Start()
    {
        // Initialize the variables
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        center = transform.Find("center").gameObject;
        availableGrenades = transform.Find("Bombs/Grenades");
        availableYarn = transform.Find("Bombs/Yarns");
    }

    void Update()
    {  
        // Always make the center follow the player
        center.transform.localPosition = new Vector2(-0.01f, -0.15f);
        
        // Checks if the player has ran out of health, and returns player to game over screen
        if (health == 0)    SceneManager.LoadScene("Game Over");

        // safeTime makes sure the player a downtime to the zombies' hits
        if(safeTime)    safeTimer += Time.deltaTime;

        // Checks if the safetimer is out of time, and turns of the safeTime
        if (safeTimer >= 1.5f)
        {
            safeTime = false;
            safeTimer = 0.0f;
        }

        // Checks if autoRegen can be started
        if (health < 2) autoRegen = true;

        // Begins autoRegenTimer
        if (autoRegen)  autoRegenTimer += Time.deltaTime;
        
        // Increments health by one if timer reaches certain time, and timer is reset
        if (autoRegenTimer >= 5.0f)
        {
            health++;
            autoRegen = false;
            autoRegenTimer = 0.0f;
        }

        // Checks if the player pressed the escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UI.isPaused)
            {
                ui.Resume();
            }
            else
            {
                ui.Pause();
            }
        }

        // Checks if the player has grenades and balls of yarn to throw
        if (Input.GetKeyDown(KeyCode.F))
        {
            // If player has available grenades, grab the first one, set it to active, drop it 
            // At the player's position, and update the ui to show that the player one less grenade 
            if (availableGrenades.childCount > 0)
            {
                Transform temp = availableGrenades.GetChild(0);
                temp.gameObject.SetActive(true);
                temp.parent = null;
                ui.grenadeUI();
                temp.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // If player has available yarn, grab the first one, set it to active, drop it 
            // At the player's position, and update the ui to show that the player one less yarn 
            if (availableYarn.childCount > 0)
            {
                Transform temp = availableYarn.GetChild(0);
                temp.gameObject.SetActive(true);
                temp.parent = null;
                // Because the zombies are attracted to game objects with the "player" tag andthe ball of yarns
                // Attracts the zommbies, the yarn's tag becomes "player", and the player's tag becomes "Hidden Player"
                temp.transform.tag = "Player";
                transform.tag = "Hidden Player";
                ui.yarnUI();
                temp.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
            }
        }

        // If the player presses the d key, the sprite will face towards the right
        // And if the player presses the a key, the sprite will face towards the left
            if (transform.localScale.x > 0) center.transform.localScale = new Vector3 (-1, 1, 1);
            else    center.transform.localScale = new Vector3 (1, 1, 1);


        // convert user input into a movement direction (could use Unity axis for this)
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) dir += Vector2.right;
        if (Input.GetKey(KeyCode.A)) dir += Vector2.left;
        if (Input.GetKey(KeyCode.W)) dir += Vector2.up;
        if (Input.GetKey(KeyCode.S)) dir += Vector2.down;

        // set velocity based on movement direction
        _velocity = dir.normalized * (isSlowed ? SLOW_SPEED : SPEED);
        animator.SetFloat("speed", dir.magnitude);
        if (Input.GetAxisRaw("Horizontal") > 0) {
            pawl.transform.localScale = new Vector3(-1, 1, 1);
        } else if (Input.GetAxisRaw("Horizontal") < 0) {
            pawl.transform.localScale = new Vector3(1, 1, 1);
        }
        // integrate velocity to update position
        transform.position = transform.position + (Vector3)(_velocity * Time.deltaTime);
    }

    // call this function from other scripts to deal damage to the player (if safeTime is not active)
    public void Damage_Player() {
        if (!safeTime) {
            hit.PlayOneShot(hit.clip);
            health--;
            safeTime = true;
            StartCoroutine(Damage());
        }
    }

    // call this function from other scripts to slow the player down
    public void Slow_Player(bool isSlow) {
        isSlowed = isSlow;
    }

    // // Check if the player has hit by the zombie
    // void OnCollisionStay2D(Collision2D collision)
    // {
    //     if (collision.collider.tag == "Zombie" && !safeTime)
    //     {
    //         hit.PlayOneShot(hit.clip);
    //         health--;
    //         safeTime = true;

    //         collision.collider.GetComponent<Zombie>().attack();
    //         StartCoroutine(Damage());
    //     }
    // }

    IEnumerator Damage() {
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.05f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
