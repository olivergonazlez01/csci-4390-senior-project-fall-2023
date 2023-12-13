using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GunController : MonoBehaviour
{
    //multipliers
    private int multiplier = 2;
    private int dmgmultiplier;
    // References to the rigid body in the center game object, the camera, the ui, and position of the mouse
    Rigidbody2D rb;
    Camera cam;
    Vector2 mousePos;
    UI ui;

    // Reference to the gunpoint at the tip of the gun, and a bullet trail that will be created when shooting
    Transform gunPoint;
    Transform gunPoint_2;
    Transform gunPoint_3;
    [SerializeField] Transform bulletTrail;
    [SerializeField] Transform bulletTrail_2;
    [SerializeField] Transform bulletTrail_3;

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
    public ushort bulletCountTotalInitial;

    // Keeps time of the automatic shooting
    float timer = 0f;

    // Checks if the gun is rotate so that it is always upright
    bool rotated = false;
    
    // Checks if the player has the instakill powerup active at the time of damaging zombies
    Powerup ikpu_Controller;
    Powerup dppu_Controller;

    // Checks if the player is reloading their gun
    private bool isReloading = false;

    // Checks if the player is in cooldown
    private bool isCooldown = false;

    public void PickedUp()
    {
        // Get the name of the gun so that it can know how much damage to deal
        bulletCountTotalInitial =  bulletCountTotal;
        gunName = transform.name;
        // Initialize variables
        rb = transform.parent.GetComponent<Rigidbody2D>();
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        gunPoint = transform.Find("GunPoint");
        ikpu_Controller = GameObject.Find("/Insta-Kill").GetComponent<Powerup>();
        dppu_Controller = GameObject.Find("/Double Points").GetComponent<Powerup>();

        // If the player picked up the shotgun, it will initialize the other gunpoint variables
        if (transform.name == "Shotgun")
        {
            gunPoint_2 = transform.Find("GunPoint (1)");
            gunPoint_3 = transform.Find("GunPoint (2)");
        }
    }

    void Update()
    {
        // Check if the gun is equipped
        if (pickedUp.Equipped() && !UI.isPaused) {
            // If r key is pressed and reloading is not happening, reload
            if (Input.GetKeyDown(KeyCode.R) && !isReloading && !isCooldown)    StartCoroutine(Reload());
            // If left click and not realoding, if gun has ammo, shoot
            if (transform.name == "Rifle" && Input.GetMouseButton(0) && !isReloading)
            {
                if (timer <= 0)
                {
                    gunshot.PlayOneShot(gunshot.clip);
                    Shoot();
                    timer = 0.10f;
                }
                else
                    timer -= Time.deltaTime;
            }
            else if (Input.GetMouseButtonDown(0) && !isReloading && !isCooldown)
            {
                if (bulletCount > 0) {
                    gunshot.PlayOneShot(gunshot.clip);
                    if (transform.name == "Shotgun")
                        MultiShoot();
                    else
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
            if (hit.transform != null)  Damage(hit.transform, gunPoint);
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
        if (gunName == "Sniper" && !isCooldown) {
            StartCoroutine(Cooldown());
        }
    }

    void MultiShoot()
    {
        float angle = 0.0872665f;
        Vector2 direction_2 = new Vector2(Mathf.Cos(Mathf.Atan(-transform.right.y / -transform.right.x) + angle), Mathf.Sin(Mathf.Atan(-transform.right.y / -transform.right.x) + angle));
        Vector2 direction_3 = new Vector2(Mathf.Cos(Mathf.Atan(-transform.right.y / -transform.right.x) - angle), Mathf.Sin(Mathf.Atan(-transform.right.y / -transform.right.x) - angle));

        if (transform.localRotation.y < 0)
        {
            direction_2 = new Vector2(-Mathf.Cos(Mathf.Atan(-transform.right.y / -transform.right.x) + angle), -Mathf.Sin(Mathf.Atan(-transform.right.y / -transform.right.x) + angle));
            direction_3 = new Vector2(-Mathf.Cos(Mathf.Atan(-transform.right.y / -transform.right.x) - angle), -Mathf.Sin(Mathf.Atan(-transform.right.y / -transform.right.x) - angle));
        }
        // Decrease the number of bullets the gun has, and throw a raycast in the direction of the mouse position
        bulletCount--;
        // Only return a non-null hit if the raycast collided with a zombie
        RaycastHit2D hit = Physics2D.Raycast
        (
            gunPoint.position,
            -transform.right,
            LayerMask.GetMask("Zombie")
        );
        RaycastHit2D hit_2 = Physics2D.Raycast
        (
            gunPoint_2.position,
            direction_2,
            LayerMask.GetMask("Zombie")
        );
        RaycastHit2D hit_3 = Physics2D.Raycast
        (
            gunPoint_3.position,
            direction_3,
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
        var trail_2 = Instantiate
        (
            bulletTrail_2,
            gunPoint_2.position,
            transform.rotation
        );
        var trailScript_2 = trail_2.GetComponent<BulletTrail>();
        var trail_3 = Instantiate
        (
            bulletTrail_3,
            gunPoint_3.position,
            transform.rotation
        );
        var trailScript_3 = trail_3.GetComponent<BulletTrail>();


        // if the collider hits something, set the target position of the bullet trail that hit, and check for damage 
        if (hit.collider != null)
        {
            trailScript.SetTargetPosition(hit.point);
            if (hit.transform != null)  Damage(hit.transform, gunPoint);
        }
        // Else make the end position a location away from the map
        else
        {
            var endPosition = gunPoint.position + -transform.right * 100;
            trailScript.SetTargetPosition(endPosition);
        }
        if (hit_2.collider != null)
        {
            trailScript_2.SetTargetPosition(hit_2.point);
            if (hit_2.transform != null) Damage(hit_2.transform, gunPoint_2);
        }
        else
        {
            var endPosition = gunPoint_2.position + -transform.right * 100;
            trailScript_2.SetTargetPosition(endPosition);
        }
        if (hit_3.collider != null)
        {
            trailScript_3.SetTargetPosition(hit_3.point);
            if (hit_3.transform != null) Damage(hit_3.transform, gunPoint_3);
        }
        else
        {
            var endPosition = gunPoint_3.position + -transform.right * 100;
            trailScript_3.SetTargetPosition(endPosition);
        }

        // If clip is empty, reload
        if (bulletCount == 0)   StartCoroutine(Reload());
        // Change UI to show that player has shot a bullet
        ui.Change(bulletCount, bulletCountTotal);
        // if (gunName == "Sniper") {
        //     StartCoroutine(Cooldown());
        // }
    }

    IEnumerator Cooldown() {
        isCooldown = true;
        buttons[0].enabled = false;
        buttons[1].enabled = false;
        Debug.Log("enter");
        yield return new WaitForSeconds(1.25f);
        isCooldown = false;
        buttons[0].enabled = true;
        buttons[1].enabled = true;
        Debug.Log("exit");
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

    void Damage(Transform zombie, Transform currentGunPoint)
    {
        // try to load corresponding enemy script
        Pathfinding_entity zombieScript = zombie.GetComponent<Pathfinding_entity>();
        if (zombieScript == null ) return;

        // play hit sound effect
        hit.PlayOneShot(hit.clip);

        // if the insta kill powerup is active, instantly kill the zombie
        // accomplish this by sending a negative number as damage to assign health to zero
        if (ikpu_Controller.ik_Active) {
            zombieScript.Damage_Zombie(-1);
        } else {
            // checks if double points is active
            if (dppu_Controller.dp_Active) {
                PointsManager.increase(multiplier * 2);
            } else {
                PointsManager.increase(multiplier);
            }

            // determine which gun is being used
            // use the following functions to deal a certain amount of damage to zombie
            // and determine how far each should push it back, depending on gunPoint direction
            switch(gunName) 
            {
                case "Pistol":
                    zombieScript.Damage_Zombie((short)(20*PawAPunch.pMult));
                    zombieScript.pushBack(30.0f, gunPoint.position);
                break;
                
                case "Rifle":
                    zombieScript.Damage_Zombie((short)(30*PawAPunch.arMult));
                    zombieScript.pushBack(60.0f, gunPoint.position);
                break;

                case "Sniper":
                    zombieScript.Damage_Zombie((short)(150*PawAPunch.snipMult));
                    zombieScript.pushBack(90.0f, gunPoint.position);
                break;
                case "Shotgun":
                    zombieScript.Damage_Zombie((short)(40*PawAPunch.shotMult));
                    zombieScript.pushBack(50.0f, currentGunPoint.position);
                break;
            }
        }

        // Zombie zombieScript = zombie.GetComponent<Zombie>();
        // // Check if the parameter variable has the zombie script
        // if (zombieScript != null)
        // {
        //     hit.PlayOneShot(hit.clip);
        //     // If the insta kill powerup is active, instantly kill the zombies
        //     if (ikpu_Controller.ik_Active)    zombieScript.health = 0;
        //     else
        //     {
        //         // Increase the player's points according to the multiplier
        //         if (dppu_Controller.dp_Active)    PointsManager.increase(multiplier * 2);
        //         else    PointsManager.increase(multiplier);

        //         // Deal the damage to each zombie according the name of each gun
        //         switch(gunName)
        //         {
        //             case "Pistol":
        //                 zombieScript.health -= 20;
        //                 zombieScript.pushBack(50.0f);
        //             break;

        //             case "Rifle":
        //                 zombieScript.health -= 40;
        //                 zombieScript.pushBack(100.0f);
        //             break;

        //             case "Sniper":
        //                 zombieScript.health -= 80;
        //                 zombieScript.pushBack(150.0f);
        //             break;
        //         }
        //     }
        // }
    }
}
