using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GunController : MonoBehaviour
{
    Rigidbody2D rb;
    Camera cam;
    Vector2 mousePos;
    UI ui;
    Transform gunPoint;

    public string gunName = "";
    public byte perRound = 0;
    public byte bulletCount = 0;
    public ushort bulletCountTotal = 0;

    bool rotated = false;
    int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        gunName = transform.name;

        rb = transform.parent.GetComponent<Rigidbody2D>();
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        gunPoint = transform.Find("GunPoint");
        //layerMask = LayerMask.GetMask("Zombie");
        layerMask = ~(LayerMask.GetMask("Zombie"));


        if (gunName == "pistol")
        {
            perRound = 7;
            bulletCount = perRound;
            bulletCountTotal = (ushort)(perRound * 7);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (bulletCount > 0)
                Shoot();
        }

        // Rotation
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pointDir = mousePos - rb.position;
        float angle = Mathf.Atan2(pointDir.y, pointDir.x) * Mathf.Rad2Deg + 180f;
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
    }

    void Shoot()
    {
        bulletCount--;

        RaycastHit2D info = Physics2D.Raycast(
            gunPoint.position,
            -transform.right,
            // layerMask
            LayerMask.GetMask("Zombie")
            //1 << 7
        );

        if (info.transform != null)
            Debug.Log(info.transform.name);


        if (bulletCount == 0)
        {
            Reload();
        }

        ui.Change();
    }

    void Reload()
    {
        byte temp = (byte)(perRound - bulletCount);

        if (bulletCountTotal < temp)
        {
            bulletCount += (byte)bulletCountTotal;
            bulletCountTotal -= bulletCountTotal;
        }
        else
        {
            bulletCountTotal -= temp;
            bulletCount += temp;
        }

        ui.Change();
    }
}
