using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    // Stores the initial number of zombies that will spawn
    byte roundZomCount = 7;
    // Stores the number of zombies have been left
    byte zomSpawned = 0;
    // Stores the number of zombies that are left to die in that round
    public byte zombiesLeft;
    // Stores the round the player is on
    byte round = 0;
    // Turn on and off spawning of zombies
    bool spawning = true;
    // The spawn timer and interval for zombies to spawn
    private float _spawnTimer;
    public const float SPAWN_INTERVAL = 3.0f;
    // Reference to the spawner
    public GameObject spawner;
    // Keeps track of the zombies that are active in the round
    public List<GameObject> activeZombies = new List<GameObject>();
    // The possible locations zombies can spawn
    public float[,] spawnAreas = new float[4,2] 
    { {(float)4.9, (float)3.9} , {(float)4.9, (float)-3.9} , {(float)-4.9, (float)3.9} , {(float)-4.9, (float)-3.9} };

    void Start()
    {
        // Initialize variables
        _spawnTimer = SPAWN_INTERVAL;
        spawner = GameObject.Find("Zombie Spawner");
        zombiesLeft = roundZomCount;
    }

    void Update()
    {
        // If the max number of zombies in that round have spawned, turn off spawning
        if (zomSpawned == roundZomCount)    spawning = false;
        // If spawning is true, keep decreasing timer
        if (spawning)   _spawnTimer -= Time.deltaTime;

        // If timer reaches 0, the spawner still has children, and the max number of zombies still has not been reached
        if (_spawnTimer <= 0 && spawner.transform.childCount > 0 && zomSpawned <= roundZomCount)
        {
            // Grab the first zombie in spawner and grab its zombie script
            Transform childZombie = spawner.transform.GetChild(0);
            Zombie zombieScript = childZombie.GetComponent<Zombie>();
            // Set the zombie's health according to the round
            zombieScript.health = (short)(100 + (round - 1) * 20);
            // Set the parent to nothing and add to the list of active zombies
            childZombie.SetParent(null);
            activeZombies.Add(childZombie.gameObject);

            // Choose a random spawn area to spawn the chosen zombie and activate the game object in the chosen location
            int rand = Random.Range(0, 3);
            childZombie.transform.position = new Vector2(spawnAreas[rand, 0], spawnAreas[rand, 1]);
            childZombie.gameObject.SetActive(true);
            
            // Increase the number of zombies spawned and reset timer
            zomSpawned++;
            _spawnTimer = SPAWN_INTERVAL;
        }

        // If there are no more zombies left, increase the round, increase max number of spawnable zombies,
        // reset zombiesLeft, zomSpawned variables, turn on spawning, and reset timer
        if (zombiesLeft == 0)
        {
            round++;
            roundZomCount += 3;
            zombiesLeft = roundZomCount;
            zomSpawned = 0;
            
            spawning = true;
            _spawnTimer = SPAWN_INTERVAL;
        }
    }
}
