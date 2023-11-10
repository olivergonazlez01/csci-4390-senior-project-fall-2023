using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Controls the timers for instakill and double points
    float ik_Timer = 0;
    public bool ik_Active = false;

    float dp_timer = 0;
    public bool dp_Active = false;

    // Controls how long a powerup can wait to be picked up by the player before despawning
    float activetimer = 5.0f;
    public bool active = false;

    // Reference to the game controller, ui and the game object that stores the used bombs
    public GameObject gameController;
    MainController controller;
    UI ui;
    GameObject references;

    void Start()
    {
        // Intialize variables
        gameController = GameObject.Find("Game Controller");
        controller = gameController.GetComponentInChildren<MainController>();
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        references = GameObject.Find("References to Bombs").gameObject;
    }

    void Update()
    {
        // If the powerup was not picked after the timer ended, the powerup will despawn and will return to a location
        // That is away from the map like the grenades and zombies
        if (activetimer <= 0 && active)
        {
            active = false;
            activetimer = 5.0f;
            transform.position = new Vector3(-6.48f, -78.17f, -1f);
        }
        
        // If the timer for insta kill has run out, deactivate it, and make the visual change in the UI
        if (ik_Timer <= 0 && ik_Active)
        {
            ik_Active = false;
            ui.instaKillActive(ik_Active);
        }

        // If the timer for double points has run out, deactivate it, and make the visual change in the UI
        if (dp_timer <= 0 && dp_Active)
        {
            dp_Active = false;
            ui.douPoiActive(dp_Active);
        }

        // Timers for active, insta kill active, and double points active
        if (active) activetimer -= Time.deltaTime;

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
        // If the powerup collides with the player
        if (col.transform.tag == "Player") 
        {
            switch(transform.name)
            {
                // If the player collides with the insta kill powerup
                case "Insta-Kill":
                    ik_Timer = 15f;
                    ik_Active = true;
                    Debug.Log("Insta-Kill");
                break;

                // If the player collides with the max ammo powerup
                case "Max Ammo":
                    // Get reference to center game object and iterate through the guns the player has
                    Transform center = col.transform.GetChild(1);
                    for (int i = 0; i < center.transform.childCount; i++)
                    {
                        GunController gun = center.transform.GetChild(i).GetComponent<GunController>();
                        // If the children in the center game object have a gun controller script
                        // Check what gun it is and set its ammo in the magazine and total to their max values
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
                            // Update the ui to show the new ammo amount for the gun the player is currently using
                            if (center.transform.GetChild(i).gameObject.activeSelf) ui.Change(gun.bulletCount, gun.bulletCountTotal);
                        }
                    }
                    
                    // Get reference to the grenades and yarns game objects that store the bombs the player can use
                    Transform bombs = col.transform.GetChild(2);
                    Transform grenades = bombs.transform.Find("Grenades");
                    Transform yarns = bombs.transform.Find("Yarns");
                    
                    // If player has used at least a grenade or yarn, grab one from the references game object and move it 
                    // Inside the player, so that the player now has max grenades or max yarns available
                    // Make appropriatew update to the UI for the grenades and yarns
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

                // If the player collides with the double points powerup
                case "Double Points":
                    dp_timer = 15f;
                    dp_Active = true;
                    Debug.Log("Double Points");
                break;

                // If the player collides with the nuke powerup
                case "Nuke":
                    // For every zombie in the round that is active, kill them
                    foreach (GameObject i in controller.activeZombies)
                    {
                        Zombie zombie = i.GetComponent<Zombie>();
                        zombie.health = 0;
                    }
                    Debug.Log("Nuke");
                break;

                default:
                break;
            }

            // Move the powerup back to a location away from the map
            transform.position = new Vector3(-6.48f, -78.17f, -1f);
        }
    }
}
