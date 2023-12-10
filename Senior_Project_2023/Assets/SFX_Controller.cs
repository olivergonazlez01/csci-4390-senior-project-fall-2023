using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Controller : MonoBehaviour
{
    [SerializeField] public AudioSource pickup;
    [SerializeField] public AudioSource death;

    public void playPickup() {
        pickup.PlayOneShot(pickup.clip);
    }

    public void playDeath() {
        death.PlayOneShot(death.clip);
    }
    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
