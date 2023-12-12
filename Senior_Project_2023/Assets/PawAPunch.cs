using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawAPunch : MonoBehaviour
{ 
    //Gun Damage multipliers and ammo mags
    public static short pMult = 1;
    public static short shotMult = 1;
    public static short snipMult = 1;
    public static short arMult = 1;
    
    GameObject center;
    public UI ui;
    float timer = 0f;
    bool inrange = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "pawl")
        {
            Interact.turn_on();
            inrange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        if (collision.transform.name == "pawl")
        {
            Interact.turn_off();
            inrange = false;
            
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("/UI Controller").GetComponent<UI>();
        center = GameObject.Find("/pawl/center");
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0){
            timer -= Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.E) && inrange && timer<=0 && center.transform.childCount > 0){
                if(PointsManager.PointValue >= 5000){
                    ui.changeMessage("Enjoy your upgrade chief!");
                    PointsManager.PointValue -= 5000;
                    timer = 2.0f;
                    Transform gun = center.transform.GetChild(0).gameObject.activeSelf ? center.transform.GetChild(0):center.transform.GetChild(1) ;
                    string GunName = gun.name;
                    GunController gunVariables = gun.GetComponent<GunController>();
                    switch(GunName){
                        case "Rifle": 
                        arMult += 2;
                        gunVariables.magazine += 30;
                        gunVariables.bulletCount = gunVariables.magazine;
                        gunVariables.bulletCountTotal += (ushort)(0.5 * gunVariables.bulletCountTotalInitial);
                        break;

                        case "Sniper":
                        snipMult += 2;
                        gunVariables.magazine += 10;
                        gunVariables.bulletCount = gunVariables.magazine;
                        gunVariables.bulletCountTotal += (ushort)(0.5 * gunVariables.bulletCountTotalInitial);
                        break;

                        case "Shotgun":
                        shotMult += 2;
                        gunVariables.magazine += 4;
                        gunVariables.bulletCount = gunVariables.magazine;
                        gunVariables.bulletCountTotal += (ushort)(0.5 * gunVariables.bulletCountTotalInitial);
                        break;

                        case "Pistol":
                        pMult += 3;
                        gunVariables.magazine += 7;
                        gunVariables.bulletCount = gunVariables.magazine;
                        gunVariables.bulletCountTotal += (ushort)(0.5 * gunVariables.bulletCountTotalInitial);
                        break;
                    }
                    ui.Change(gunVariables.bulletCount,gunVariables.bulletCountTotal);
                }
                else if(PointsManager.PointValue < 5000) {
                    ui.changeMessage("This cost 5000 points, get your money up LOL broke ahhh!!");
                }
            }
    }
}
