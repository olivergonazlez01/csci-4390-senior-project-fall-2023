using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    // Countdown till explosion
    float countdown = 3.0f;

    // Keeps track of zombies inside the range of a bomb
    List<Transform> zombies = new List<Transform>();

    // Reference to player so that the ball of yarn can set the player's tag
    Player player;

    // Reference to UI to visually keep track of the bombs the player has left
    UI ui;

    // Once bombs explode, they are put inside this game object for easy access in max ammo powerup
    GameObject bombCollection;

    // Animator for explosion and yarn
    public Animator explosion;

    // Reference to sound 
    public SFX_Controller soundController;

    // Vairables to set animations
    bool unravel = false;
    bool grenadeAnim = false;
 
    void Start()
    {
        // Instantiating to game objects inside the scene
        player = GameObject.Find("pawl").GetComponent<Player>();
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        bombCollection = GameObject.Find("References to Bombs");
        soundController = GameObject.Find("SFX_Controller").transform.GetComponent<SFX_Controller>();
        // After instantiating, turn off game object so that countdown does not start
        transform.gameObject.SetActive(false);
    }

    void Update()
    {
        // Countdown the timer
        if (countdown > 0)
        {
            if (transform.name == "Yarn" && !unravel)
            {
                unravel = true;
                explosion.Play("Yarn unravel");
                StartCoroutine(UnravelYarn());
            }
            else if (transform.name == "Grenade" && !grenadeAnim)
            {
                grenadeAnim = true;
                explosion.Play("Grenade State");
                StartCoroutine(UnravelYarn());
            }
            countdown -= Time.deltaTime;
        }

        // When countdown reaches 0, for each zombie in the list, deal them damage
        if (countdown < 0)
        {
            explosion.Play("Bigger Explosion");
            soundController.playExplosion();
            StartCoroutine(waitForExplosion());

            foreach (Transform zombie in zombies)
                Damage(zombie);

            // Clear list 
            zombies.Clear();
            // Move grenade game object to be inside the bomb collection game object
            transform.parent = bombCollection.transform;
            // Reset countdown for multiple usage
            countdown = 3.0f;
            // Reset player's tag back to player because the ball of yarn messes with it
            player.transform.tag = "Player";
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // If a zombie walks in range of the grenade, add it to the list
        if (col.transform.tag == "Zombie")
            zombies.Add(col.transform);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // If a zombie leaves the range of the grenade, delete it from the list
        if (col.transform.tag == "Zombie" && zombies.Contains(col.transform))
            zombies.Remove(col.transform);
    }

    void Damage(Transform zombie)
    {
        // Make sure each the parameter variable has the zombiescript, and decrease its health
        Pathfinding_entity zombieScript = zombie.GetComponent<Pathfinding_entity>();
        if (zombieScript != null)
            zombieScript.Damage_Zombie(100);
    }


    IEnumerator waitForExplosion()
    {
        // Wait for the explosion animation to finish
        yield return new WaitWhile(() => explosion.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f);

        // Move grenade off the map like how we do with the zombies and powerups
        transform.position = new Vector3(-0.38f, -30.08f, 0);
        // Turn off game object
        transform.gameObject.SetActive(false);
    }


    IEnumerator UnravelYarn()
    {
        yield return new WaitWhile(() => explosion.GetCurrentAnimatorStateInfo(0).normalizedTime <= 3.0f);
    }

    // IEnumerator GrenadeAnimation()
    // {
    //     yield return new WaitWhile(() => explosion.GetCurrentAnimatorStateInfo(0).normalizedTime <= 3.0f);
    // }
}
