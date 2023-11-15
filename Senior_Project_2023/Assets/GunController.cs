using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GunController : MonoBehaviour
{
    //multiplier
    private int multiplier = 2;
    // References to the rigid body in the center game object, the camera, the ui, and position of the mouse
    Rigidbody2D rb;
    Camera cam;
    Vector2 mousePos;
    UI ui;

    // Reference to the gunpoint at the tip of the gun, and a bullet trail that will be created when shooting
    Transform gunPoint;
    [SerializeField] Transform bulletTrail;

    // Audio for gunshots, gunclicks for reloading, empty gun, and zombie hit
    [SerializeField] AudioSource gunshot;
    [SerializeField] AudioSource gunClick;
    [SerializeField] AudioSource emptyGun;
    [SerializeField] AudioSource hit;

    // Reference to the buttons in the ui for visualizing reloading
    [SerializeField] public ButtonController[] buttons;

    // Reference to script that allows the guns to be picked up
    public Pickup pickedUp;

    // Store the name of the gun using this script, the max size of its magazine
    // And the amount of ammo outside and inside the magazine
    string gunName;
    public byte magazine;
    public byte bulletCount;
    public ushort bulletCountTotal;

    // Checks if the gun is rotate so that it is always upright
    bool rotated = false;
    
    // Checks if the player has the instakill powerup active at the time of damaging zombies
    Powerup ikpu_Controller;

    // Checks if the player is reloading their gun
    private bool isReloading = false;

    public void PickedUp()
    {
        // Get the name of the gun so that it can know how much damage to deal 
        gunName = transform.name;

        // Initialize variables
        rb = transform.parent.GetComponent<Rigidbody2D>();
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        gunPoint = transform.Find("GunPoint");
        ikpu_Controller = GameObject.Find("/Insta-Kill").GetComponent<Powerup>();
    }

    void Update()
    {
        // Check if the gun is equipped
        if (pickedUp.Equipped()) {
            // If r key is pressed and reloading is not happening, reload
            if (Input.GetKeyDown(KeyCode.R) && !isReloading)    StartCoroutine(Reload());
            // If left click and not realoding, if gun has ammo, shoot
            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                if (bulletCount > 0) {
                    gunshot.PlayOneShot(gunshot.clip);
                    Shoot();
                } 
                else  emptyGun.PlayOneShot(emptyGun.clip);
            }

            // Rotation of gun to always be upright
            // Get mouse position, directiong of the mouse compared to the center game object, and angle of direction 
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pointDir = mousePos - rb.position;
            float angle = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg;
            // rotate the rigid body of the center game object to the angle
            rb.rotation = angle;

            // if the gun has not been rotated and it is "behind" the player, rotate the gun once so that it is upright
            if (!rotated && transform.rotation.eulerAngles.z < 270 && transform.rotation.eulerAngles.z > 90)
            {
                rotated = true;
                transform.localRotation = Quaternion.Euler(180,transform.localEulerAngles.y,transform.localEulerAngles.z);
            }
            // if the gun is "in front" of the player, rotate the gun so that it is still upright
            else
            {
                transform.localRotation = Quaternion.Euler(0,transform.localEulerAngles.y,transform.localEulerAngles.z);
                rotated = false;
            }
        }
    }

    void Shoot()
    {
        // Decrease the number of bullets the gun has, and throw a raycast in the direction of the mouse position
        bulletCount--;
        // Only return a non-null hit if the raycast collided with a zombie
        RaycastHit2D hit = Physics2D.Raycast
        (
            gunPoint.position,
            -transform.right,
            LayerMask.GetMask("Zombie")
        );

        // Instantiate a bullet trail
        var trail = Instantiate
        (
            bulletTrail,
            gunPoint.position,
            transform.rotation
        );
        var trailScript = trail.GetComponent<BulletTrail>();

        // if the collider hits something, set the target position of the bullet trail that hit, and check for damage 
        if (hit.collider != null)
        {
            trailScript.SetTargetPosition(hit.point);
            if (hit.transform != null)  Damage(hit.transform);
        }
        // Else make the end position a location away from the map
        else
        {
            var endPosition = gunPoint.position + -transform.right * 100;
            trailScript.SetTargetPosition(endPosition);
        }

        // If clip is empty, reload
        if (bulletCount == 0)   StartCoroutine(Reload());
        // Change UI to show that player has shot a bullet
        ui.Change(bulletCount, bulletCountTotal);
    }

    IEnumerator Reload()
    {
        // If the player has no bullets left or if their magazine is full, do not reload
        if (bulletCountTotal == 0 || bulletCount == magazine) yield break;

        // Make reloading true and disable both buttons so that switching between guns is not possible
        isReloading = true;
        gunClick.PlayOneShot(gunClick.clip);
        buttons[0].enabled = false;
        buttons[1].enabled = false;
        // Make the player wait for some time
        ui.Reloading(1);
        yield return new WaitForSeconds(0.5f);
        ui.Reloading(2);
        yield return new WaitForSeconds(0.5f);
        ui.Reloading(3);
        yield return new WaitForSeconds(0.5f);
        ui.Reloading(4);
        // Decrement number of bullets outside the clip and increase bullets in clip proportional to
        // The number of bullets in clip at time of reloading
        while (bulletCount < magazine && bulletCountTotal > 0) {
            bulletCount += 1;
            bulletCountTotal -= 1;
        }

        // UI gets updated with new number of bullets, buttons get enabled again, and make reloading is false
        gunClick.PlayOneShot(gunClick.clip);
        ui.Change(bulletCount, bulletCountTotal);
        buttons[0].enabled = true;
        buttons[1].enabled = true;
        isReloading = false;
    }

    void Damage(Transform zombie)
    {
        Zombie zombieScript = zombie.GetComponent<Zombie>();
        // Check if the parameter variable has the zombie script
        if (zombieScript != null)
        {
            hit.PlayOneShot(hit.clip);
            // If the insta kill powerup is active, instantly kill the zombies
            if (ikpu_Controller.ik_Active)
            {
                zombieScript.health = 0;
            }
            else
            {
                // Deal the damage to each zombie according the name of each gun
                PointsManager.increase(multiplier);
                switch(gunName)
                {
                    case "Pistol":
                        zombieScript.health -= 20;
                        zombieScript.pushBack(50.0f);
                    break;

                    case "Rifle":
                        zombieScript.health -= 40;
                        zombieScript.pushBack(100.0f);
                    break;

                    case "Sniper":
                        zombieScript.health -= 80;
                        zombieScript.pushBack(150.0f);
                    break;
                }
            }
        }
    }
}
