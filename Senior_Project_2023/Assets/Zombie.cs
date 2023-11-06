using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Zombie : Pathfinding_entity
{
    private const float SPEED = 3.0f;
    private Vector2 _velocity = Vector2.zero;

    public float healthMultiplier = 1;
    public short health;

    public GameObject Player;
    public SpriteRenderer zombie;
    public Animator animator;


    public GameObject gameController;
    MainController controller;
    public GameObject spawner;

    public Transform PUTemp;
    Vector3 dropPosition;

    // Start is called before the first frame update
    protected override void Start()
    {
        // call Pathfinding-entity start method
        base.Start();

        Player = GameObject.Find("pawl");
        health = (short) Math.Ceiling((double) (100 * healthMultiplier));

        //Player = GameObject.Find("pawl");
        gameController = GameObject.Find("Game Controller");
        controller = gameController.GetComponentInChildren<MainController>();
        //spawner = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {            
            dropPosition = transform.position;

            controller.activeZombies.Remove(transform.gameObject);
            controller.zombiesLeft--;
            spawner = GameObject.Find("Zombie Spawner");
            transform.SetParent(spawner.transform);
            transform.localPosition = new Vector2(0, 0);
            transform.gameObject.SetActive(false);

            int chance = UnityEngine.Random.Range(0, 50);

            if (chance <= 9)
            {
                int powerup = UnityEngine.Random.Range(0, 4);
                bool chosen = false;
                bool opt0 = false;
                bool opt1 = false;
                bool opt2 = false;
                bool opt3 = false;
                Powerup temp;
                while (!chosen)
                {
                    switch(powerup)
                    {
                        case 0:
                            PUTemp = GameObject.Find("Insta-Kill").transform;
                            temp = PUTemp.GetComponent<Powerup>();
                            if (opt0)   { PUTemp = null; chosen = true; }
                            else 
                            {
                                if (!temp.active)   chosen = true;
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

                if (PUTemp != null)
                {
                    PUTemp.position = new Vector3(dropPosition.x, dropPosition.y, -1f);
                    temp = PUTemp.GetComponent<Powerup>();
                    temp.active = true;
                }
            }
        }
        // Check if the zombie has touched the player
        //if (Player.health)

        // Follow player around the map no matter the distance
        setTarget(Player.transform);
        setEntitySpeed(SPEED);
        if (isMoving()) {
            //Debug.Log("true");
            animator.SetFloat("speed", getDirection().magnitude);
            if (getDirection().x > 0) {
                zombie.flipX = true;
            } else {
                zombie.flipX = false;
            }
        }

        // Vector2 dir = Player.transform.position - transform.position;
        // _velocity = dir.normalized * SPEED;
        // animator.SetFloat("speed", dir.magnitude);
        // if (dir.x > 0) {
        //     zombie.flipX = true;
        // } else if (dir.x < 0) {
        //     zombie.flipX = false;
        // }
        // transform.position = transform.position + (Vector3)(_velocity * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
