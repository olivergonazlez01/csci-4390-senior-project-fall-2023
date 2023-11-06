using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Bombs : MonoBehaviour
{
    float countdown = 3.0f;

    List<Transform> zombies = new List<Transform>();
    Player player;
    UI ui;
    GameObject bombCollection;
 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("pawl").GetComponent<Player>();
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        bombCollection = GameObject.Find("References to Bombs");

        transform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown < 0)
        {
            foreach (Transform zombie in zombies)
                Damage(zombie);

            zombies.Clear();
            transform.position = new Vector3(-0.38f, -30.08f, 0);
            transform.parent = bombCollection.transform;
            countdown = 3.0f;
            transform.gameObject.SetActive(false);
            player.transform.tag = "Player";
        }
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Zombie")
            zombies.Add(col.transform);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform.tag == "Zombie" && zombies.Contains(col.transform))
        {
            zombies.Remove(col.transform);
        }
    }

    void Damage(Transform zombie)
    {
        Zombie zombieScipt = zombie.GetComponent<Zombie>();

        if (zombieScipt != null)
        {
            zombieScipt.health -= 100;
        }
    }
}
