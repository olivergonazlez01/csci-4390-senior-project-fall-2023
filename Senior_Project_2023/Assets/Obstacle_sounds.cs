using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle_sounds : MonoBehaviour
{
    public static AudioSource unlocking;

    public static void playSound() {
        unlocking.PlayOneShot(unlocking.clip);
    }

    // Start is called before the first frame update
    void Start()
    {
        unlocking = transform.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
