using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class Awareness : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision);
        if (collision.tag == "Player")
        {
            Spitter parent = GetComponentInParent<Spitter>();
            parent.attack();
        }
    }

    void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "Player") {
            Spitter parent = GetComponentInParent<Spitter>();
            parent.chase();
        }
    }
}
