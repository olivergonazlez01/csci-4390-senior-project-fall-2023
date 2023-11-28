using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Pathfinding_entity
{
    //multiplier
    int multiplier = 1;

    // Sets the speed and velocity of the zombies
    private const float SPEED = 3.0f;
    private Vector2 _velocity = Vector2.zero;
    
    // Sets the health and multiplier (will change per round) of the zombies
    public float healthMultiplier = 1;
    public short health;

    // References to the player, zombie sprite and animator, game controller, and spawner
    public GameObject Player;
    public SpriteRenderer zombie;
    public Animator animator;
    public GameObject gameController;
    MainController controller;
    public GameObject spawner;

    // Variables for powerup drops
    public Transform PUTemp;
    Vector3 dropPosition;

    public void attack() {
        StartCoroutine(Attacking());
    }

    public void pushBack(float pushForce) {
        setEntitySpeed(-pushForce);
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        // call Pathfinding-entity start method
        base.Start();

        // Set the variables
        Player = GameObject.Find("pawl");
        setTarget(Player.transform);
        setEntitySpeed(SPEED);
        health = (short) Math.Ceiling((double) (100 * healthMultiplier));
        gameController = GameObject.Find("Game Controller");
        controller = gameController.GetComponentInChildren<MainController>();
    }

    void Update()
    {
        // If a zombie's health reaches 0, decide if the zombie will drop a powerup, and return it to the  spawner
        if (health <= 0)
        {
            PointsManager.PointValue += 100 * multiplier;
            // Grabs position of the zombie
            dropPosition = transform.position;

            // Remove it from the list of active zombies and update the game controller
            controller.activeZombies.Remove(transform.gameObject);
            controller.zombiesLeft--;
            // Set the location and parent of the zombie to the spawner and turn off zombie
            spawner = GameObject.Find("Zombie Spawner");
            transform.SetParent(spawner.transform);
            transform.localPosition = new Vector2(0, 0);
            transform.gameObject.SetActive(false);
            
            // Random chance for dropping powerup
            int chance = UnityEngine.Random.Range(0, 50);

            if (chance <= 9)
            {
                // Chooses a random powerup to drop
                int powerup = UnityEngine.Random.Range(0, 4);
                // These values allow the game to properly choose a powerup that has not already spawned
                bool chosen = false;
                bool opt0 = false;
                bool opt1 = false;
                bool opt2 = false;
                bool opt3 = false;
                Powerup temp;
                // Will keep running until a powerup has been chosen, or no powerups are available at the moment
                while (!chosen)
                {
                    switch(powerup)
                    {
                        case 0:
                            PUTemp = GameObject.Find("Insta-Kill").transform;
                            temp = PUTemp.GetComponent<Powerup>();
                            // if the loop has already run through this case
                            // stop the loop and make the chosen powerup null
                            if (opt0)   { PUTemp = null; chosen = true; }
                            else 
                            {
                                // else if the chosen powerup is not spawned in the world, 
                                // choose it
                                if (!temp.active)   chosen = true;
                                // else, move to the next case and make sure that this 
                                // Case cannot be run by the loop again
                                else    { powerup = 1; opt0 = true; }
                            }
                            break;

                        case 1:
                            PUTemp = GameObject.Find("Max Ammo").transform;
                            temp = PUTemp.GetComponent<Powerup>();
                            if (opt1)   { PUTemp = null; chosen = true; }
                            else
                            {                        
                                if (!temp.active)   chosen = true;
                                else    { powerup = 2; opt1 = true; }
                            }
                            break;

                        case 2:
                            PUTemp = GameObject.Find("Nuke").transform;
                            temp = PUTemp.GetComponent<Powerup>();
                            if (opt2)   { PUTemp = null; chosen = true; }
                            else
                            {
                                if (!temp.active)   chosen = true;
                                else    { powerup = 3; opt2 = true; }
                            }
                            break;

                        case 3:
                            PUTemp = GameObject.Find("Double Points").transform;
                            temp = PUTemp.GetComponent<Powerup>();
                            if (opt3)   { PUTemp = null; chosen = true; }
                            else
                            {
                                if (!temp.active)   chosen = true;
                                else    { powerup = 0; opt3 = true; }
                            }
                            break;
                    }
                }
                
                // If PUTemp is not null, then a powerup has been chosen
                if (PUTemp != null)
                {
                    // Make its position the drop position of the zombie, and activate its script
                    PUTemp.position = new Vector3(dropPosition.x, dropPosition.y, -1f);
                    temp = PUTemp.GetComponent<Powerup>();
                    temp.active = true;
                }
            }
        }

        if (isMoving()) {
            // Follow player around the map no matter the distance
            // Make sure the zombie follows the game object with the player tag (NEEDED FOR THE YARN BOMBS)
            GameObject playerTag = GameObject.FindGameObjectWithTag("Player");
            Player = playerTag;
            setTarget(Player.transform);
            setEntitySpeed(SPEED);

            animator.SetFloat("speed", getDirection().magnitude);
            if (getDirection().x > 0) {
                zombie.flipX = true;
            } else {
                zombie.flipX = false;
            }
        } else {
            //Debug.Log("not moving lol");
        }
        
        //  Vector2 dir = Player.transform.position - transform.position;
        //  _velocity = dir.normalized * SPEED;
        //  animator.SetFloat("speed", dir.magnitude);
        //  if (dir.x > 0) {
        //      zombie.flipX = true;
        //  } else if (dir.x < 0) {
        //      zombie.flipX = false;
        //  }
        //  transform.position = transform.position + (Vector3)(_velocity * Time.deltaTime);
    }

    IEnumerator Attacking() {
        setTarget(null);
        yield return new WaitForSeconds(3.0f);
        GameObject playerTag = GameObject.FindGameObjectWithTag("Player");
        Player = playerTag;
        setTarget(Player.transform);
    }
}
