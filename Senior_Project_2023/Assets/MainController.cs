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
    private Transform graves;
    private Transform zombieSpawner;
    private Transform spitterSpawner;
    // 3 zombies : 1 spitter
    private const byte RATIO = 3;
    private byte zombieRatio = RATIO;
    public List<GameObject> activeZombies = new List<GameObject>();
    // The possible locations zombies can spawn
    public float[,] spawnAreas = new float[4,2] 
    { {(float)4.9, (float)3.9} , {(float)4.9, (float)-3.9} , {(float)-4.9, (float)3.9} , {(float)-4.9, (float)-3.9} };

    void Start()
    {
        // Initialize variables
        _spawnTimer = SPAWN_INTERVAL;
        spawner = GameObject.Find("Zombie Spawner");
        graves = spawner.transform.GetChild(0);
        zombieSpawner = spawner.transform.GetChild(1);
        spitterSpawner = spawner.transform.GetChild(2);
        zombiesLeft = roundZomCount;
    }

    void Update()
    {
        // If the max number of zombies in that round have spawned, turn off spawning
        if (zomSpawned == roundZomCount)    spawning = false;
        // If spawning is true, keep decreasing timer
        if (spawning)   _spawnTimer -= Time.deltaTime;

        // If timer reaches 0, the spawner still has children, and the max number of zombies still has not been reached
        if (_spawnTimer <= 0 && zomSpawned <= roundZomCount)
        {
            Transform newZombie = zombieRatio > 0 ? zombieSpawner.GetChild(0) : spitterSpawner.GetChild(0);
            if (zombieRatio > 0) {
                zombieRatio -= 1;
            } else {
                zombieRatio = RATIO;
            }
            Pathfinding_entity newScript = newZombie.GetComponent<Pathfinding_entity>();
            newScript.health = (short)(100 + (round -1) * 20);
            //Zombie zombieScript;
            //Spitter spitterScript;
            // if (zombieRatio > 0) {
            //     //spawn zombie
            //     //newZombie = zombieSpawner.GetChild(0);
            //     zombieScript = newZombie.GetComponent<Zombie>();
            //     zombieScript.health = (short)(100 + (round - 1) * 20);
            //     //newZombie.SetParent(null);
            //     //activeZombies.Add(newZombie.gameObject);

            //     //int rand = Random.Range(0, graves.childCount);
            //     //newZombie.transform.position = graves.GetChild(rand).transform.position;
            //     //newZombie.gameObject.SetActive(true);
            //     //zomSpawned++;
            //     //_spawnTimer = SPAWN_INTERVAL;
            //     zombieRatio -= 1;
            // } else {
            //     //spawn spitter
            //     spitterScript.health
            //     zombieRatio = RATIO;
            // }
        
            // Grab the first zombie in spawner and grab its zombie script
            
            // Set the zombie's health according to the round
            
            // Set the parent to nothing and add to the list of active zombies
            newZombie.SetParent(null);
            activeZombies.Add(newZombie.gameObject);

            // Choose a random spawn area to spawn the chosen zombie and activate the game object in the chosen location
            int rand = Random.Range(0, graves.childCount);
            newZombie.transform.position = graves.GetChild(rand).transform.position;
            newZombie.gameObject.SetActive(true);
            
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
            zombieRatio = RATIO;
            
            spawning = true;
            _spawnTimer = SPAWN_INTERVAL;
        }
    }
}
