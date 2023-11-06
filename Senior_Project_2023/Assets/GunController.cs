using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class GunController : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 mousePos;
    UI ui;
    Transform gunPoint;
    [SerializeField] Transform bulletTrail;
    [SerializeField] AudioSource gunshot;
    [SerializeField] AudioSource gunClick;
    [SerializeField] AudioSource emptyGun;
    [SerializeField] AudioSource hit;
    [SerializeField] public ButtonController[] buttons;
    public Pickup pickedUp;

    string gunName;
    public byte magazine;
    public byte bulletCount;
    public ushort bulletCountTotal;

    bool rotated = false;
    bool flipped = false;
    int layerMask;

     Powerup ikpu_Controller;

    private bool isReloading = false;
    public bool Get_isReloading() {
        Debug.Log("got it");
        return isReloading;
    }

    // Start is called before the first frame update
    public void PickedUp()
    {
        gunName = transform.name;

        rb = transform.parent.GetComponent<Rigidbody2D>();
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        gunPoint = transform.Find("GunPoint");
        ikpu_Controller = GameObject.Find("/Insta-Kill").GetComponent<Powerup>();
        //layerMask = LayerMask.GetMask("Zombie");
        layerMask = ~(LayerMask.GetMask("Zombie"));

        /*if (gunName == "pistol")
        {
            perRound = 7;
            bulletCount = perRound;
            bulletCountTotal = (ushort)(perRound * 7);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp.Equipped()) {
            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            {
                StartCoroutine(Reload());
            }

            if (Input.GetMouseButtonDown(0) && !isReloading)
            {
                if (bulletCount > 0) {
                    gunshot.PlayOneShot(gunshot.clip);
                    Shoot();
                } else {
                    emptyGun.PlayOneShot(emptyGun.clip);
                }
            }

            // Rotation
            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pointDir = mousePos - rb.position;
            float angle = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;

            if (!rotated && transform.rotation.eulerAngles.z < 270 && transform.rotation.eulerAngles.z > 90)
            {
                rotated = true;
                transform.localRotation = Quaternion.Euler(180,transform.localEulerAngles.y,transform.localEulerAngles.z);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0,transform.localEulerAngles.y,transform.localEulerAngles.z);
                rotated = false;
            }

            // if (!flipped && transform.parent.parent.localScale.x > 0) 
            // {
            //     transform.parent.localScale = new Vector3 (-transform.parent.localScale.x, 1, 1);
            //     flipped = true;
            // }
            // if (flipped && transform.parent.localScale.x < 0) 
            // {
            //     transform.parent.localScale = new Vector3 (-transform.parent.localScale.x, 1, 1);
            //     flipped = false;
            // }

            // if (transform.parent.parent.localScale.x > 0)   transform.parent.localScale = new Vector3 (-1, 1, 1);
            // else    transform.parent.localScale = new Vector3 (1, 1, 1);
        }
    }

    void Shoot()
    {
        bulletCount--;

        RaycastHit2D hit = Physics2D.Raycast
        (
            gunPoint.position,
            -transform.right,
            // layerMask
            LayerMask.GetMask("Zombie")
            //1 << 7
        );

        var trail = Instantiate
        (
            bulletTrail,
            gunPoint.position,
            transform.rotation
        );

        var trailScript = trail.GetComponent<BulletTrail>();

        if (hit.collider != null)
        {
            trailScript.SetTargetPosition(hit.point);
            //var hittable = hit.collider.GetComponent<IGraphDataWithVariables>();
            //hittable?.Hit();
            if (hit.transform != null)
                Damage(hit.transform);
        }
        else
        {
            var endPosition = gunPoint.position + -transform.right * 1000;
            trailScript.SetTargetPosition(endPosition);
        }


        if (bulletCount == 0)
        {
            StartCoroutine(Reload());
        }

        ui.Change(bulletCount, bulletCountTotal);
    }

    IEnumerator Reload()
    {
        if (bulletCountTotal == 0 || bulletCount == magazine) yield break;
        isReloading = true;
        gunClick.PlayOneShot(gunClick.clip);
        buttons[0].enabled = false;
        buttons[1].enabled = false;
        yield return new WaitForSeconds(2);
        while (bulletCount < magazine && bulletCountTotal > 0) {
            bulletCount += 1;
            bulletCountTotal -= 1;
        }
        gunClick.PlayOneShot(gunClick.clip);
        ui.Change(bulletCount, bulletCountTotal);
        buttons[0].enabled = true;
        buttons[1].enabled = true;
        isReloading = false;
    }

    void Damage(Transform zombie)
    {
        Zombie zombieScipt = zombie.GetComponent<Zombie>();

        if (zombieScipt != null)
        {
            hit.PlayOneShot(hit.clip);
            if (ikpu_Controller.ik_Active)
            {
                zombieScipt.health = 0;
            }
            else
            {
                if (gunName == "Pistol")
                {
                    if(zombieScipt.health - 20 < 0) {
                        zombieScipt.health = 0; 
                    } else {
                        zombieScipt.health -= 20;
                    }
                    Debug.Log(zombie.name + " " + zombieScipt.health);
                }
                else if (gunName == "Rifle")
                {
                    zombieScipt.health -= 40;
                    Debug.Log(zombie.name + " " + zombieScipt.health);
                }
                else if (gunName == "Sniper")
                {
                    zombieScipt.health -= 80;
                    Debug.Log(zombie.name + " " + zombieScipt.health);
                } 
            }
            // if (gunName == "Pistol")
            // {
            //     if(zombieScipt.health - 20 < 0) {
            //         zombieScipt.health = 0; 
            //     } else {
            //         zombieScipt.health -= 20;
            //     }
            //     Debug.Log(zombie.name + " " + zombieScipt.health);
            // }
            // else if (gunName == "Rifle")
            // {
            //     zombieScipt.health -= 40;
            //     Debug.Log(zombie.name + " " + zombieScipt.health);
            // }
            // else if (gunName == "Sniper")
            // {
            //     zombieScipt.health -= 80;
            //     Debug.Log(zombie.name + " " + zombieScipt.health);
            // }
        }
    }
}
