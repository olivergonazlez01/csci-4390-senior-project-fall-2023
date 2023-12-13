using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Controller : MonoBehaviour
{
    [SerializeField] public AudioSource music;
    [SerializeField] public AudioSource pickup;
    [SerializeField] public AudioSource death;
    [SerializeField] public AudioSource explosion;
    [SerializeField] public AudioSource unlock;
    [SerializeField] public AudioSource corn;

    void Start() {
        playMusicLoop();
    }

    public void playMusicLoop() {
        music.loop = true;
        music.Play();
    }

    public void playPickup() {
        pickup.PlayOneShot(pickup.clip);
    }

    public void playDeath() {
        death.PlayOneShot(death.clip);
    }

    public void playExplosion() {
        explosion.PlayOneShot(explosion.clip);
    }

    public void playUnlock() {
        unlock.PlayOneShot(unlock.clip);
    }

    public void playCornLoop(bool isLooping) {
        corn.loop = isLooping;
        if (isLooping) corn.Play();
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
