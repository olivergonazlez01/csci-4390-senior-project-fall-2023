using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Pathfinding_entity
{
    private const float SPEED = 3.0f;
    private Vector2 _velocity = Vector2.zero;

    public short health = 100;

    public GameObject Player;
    public SpriteRenderer zombie;
    public Animator animator;


    public GameObject gameController;
    MainController controller;
    public GameObject spawner;

    // Start is called before the first frame update
    protected override void Start()
    {
        // call Pathfinding-entity start method
        base.Start();

        Player = GameObject.Find("pawl");
        gameController = GameObject.Find("Game Controller");
        controller = gameController.GetComponentInChildren<MainController>();
        //spawner = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            controller.zombiesLeft--;
            spawner = GameObject.Find("Zombie Spawner");
            transform.SetParent(spawner.transform);
            transform.localPosition = new Vector2(0, 0);
            transform.gameObject.SetActive(false);
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
