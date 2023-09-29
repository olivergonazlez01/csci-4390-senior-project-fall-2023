using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    byte roundZomCount = 7;
    byte zomSpawned = 0;
    public byte zombiesLeft;

    public bool storyMode = false;

    bool spawning = true;
    private float _spawnTimer;
    public const float SPAWN_INTERVAL = 3.0f;
    public float[,] spawnAreas = new float[4,2] { {(float)4.9, (float)3.9} , 
                                                  {(float)4.9, (float)-3.9} , 
                                                  {(float)-4.9, (float)3.9} ,
                                                  {(float)-4.9, (float)-3.9} };

    public GameObject spawner;

    // Start is called before the first frame update
    void Start()
    {
        _spawnTimer = SPAWN_INTERVAL;
        spawner = GameObject.Find("Zombie Spawner");
        zombiesLeft = roundZomCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (zomSpawned == roundZomCount)    spawning = false;
        if (spawning)   _spawnTimer -= Time.deltaTime;


        if (_spawnTimer <= 0 && spawner.transform.childCount > 0 && zomSpawned < roundZomCount)
        {
            Transform childZombie = spawner.transform.GetChild(0);
            childZombie.SetParent(null);

            int rand = Random.Range(0, 3);
            childZombie.transform.position = new Vector2(spawnAreas[rand, 0], spawnAreas[rand, 1]);
            childZombie.gameObject.SetActive(true);
            
            zomSpawned++;
            _spawnTimer = SPAWN_INTERVAL;
        }

        if (zombiesLeft == 0)
        {
            roundZomCount += 3;
            zombiesLeft = roundZomCount;
            zomSpawned = 0;
            
            spawning = true;
            _spawnTimer = SPAWN_INTERVAL;
        }
    }
}
