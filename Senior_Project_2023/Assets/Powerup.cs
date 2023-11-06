using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    float ik_Timer = 0;
    public bool ik_Active = false;

    float dp_timer = 0;
    public bool dp_Active = false;

    float activetimer = 5.0f;
    public bool active = false;

    public GameObject gameController;
    MainController controller;
    UI ui;
    GameObject references;


    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("Game Controller");
        controller = gameController.GetComponentInChildren<MainController>();
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        references = GameObject.Find("References to Bombs").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (activetimer <= 0 && active)
        {
            active = false;
            activetimer = 5.0f;
            transform.position = new Vector3(-6.48f, -78.17f, -1f);
        }

        if (active)
        {
            activetimer -= Time.deltaTime;
        }
        
        if (ik_Timer <= 0 && ik_Active)
        {
            ik_Active = false;
            ui.instaKillActive(ik_Active);
        }

        if (dp_timer <= 0 && dp_Active)
        {
            dp_Active = false;
            ui.douPoiActive(dp_Active);
        }

        if (ik_Active) 
        {
            ui.instaKillActive(ik_Active);
            ik_Timer -= Time.deltaTime;
        }

        if (dp_Active)
        {
            ui.douPoiActive(dp_Active);
            dp_timer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player") 
        {
            switch(transform.name)
            {
                case "Insta-Kill":
                    ik_Timer = 15f;
                    ik_Active = true;
                    Debug.Log("Insta-Kill");
                    break;

                case "Max Ammo":
                    Transform center = col.transform.GetChild(1);
                    for (int i = 0; i < center.transform.childCount; i++)
                    {
                        GunController gun = center.transform.GetChild(i).GetComponent<GunController>();
                        if (gun != null)
                        {
                            switch(center.transform.GetChild(i).name)
                            {
                                case "Pistol":
                                    gun.bulletCount = 7;
                                    gun.bulletCountTotal = 150;
                                    break;
                                
                                case "Rifle":
                                    gun.bulletCount = 30;
                                    gun.bulletCountTotal = 300;
                                    break;
                            }
                            if (center.transform.GetChild(i).gameObject.activeSelf)
                            {
                                ui.Change(gun.bulletCount, gun.bulletCountTotal);
                            }
                        }
                    }
                    Transform bombs = col.transform.GetChild(2);
                    Transform grenades = bombs.transform.Find("Grenades");
                    Transform yarns = bombs.transform.Find("Yarns");

                    for (int i = grenades.childCount; i < 2; i++)
                    {
                        GameObject tempGrenade = references.transform.Find("Grenade").gameObject;
                        tempGrenade.transform.parent = grenades;
                        ui.grenadeUI(true);
                    }
                    for (int i = yarns.childCount; i < 2; i++)
                    {
                        GameObject tempYarn = references.transform.Find("Yarn").gameObject;
                        tempYarn.transform.parent = yarns;
                        ui.yarnUI(true);
                    }

                    Debug.Log("Max Ammo");
                    break;

                case "Double Points":
                    dp_timer = 15f;
                    dp_Active = true;
                    Debug.Log("Double Points");
                    break;

                case "Nuke":
                    foreach (GameObject i in controller.activeZombies)
                    {
                        Zombie zombie = i.GetComponent<Zombie>();
                        zombie.health = 0;
                    }
                    Debug.Log("Nuke");
                    break;

                default:
                    Debug.Log("Default");
                    break;
            }

            transform.position = new Vector3(-6.48f, -78.17f, -1f);
        }
    }
}
