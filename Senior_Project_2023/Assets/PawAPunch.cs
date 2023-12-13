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
    private int cost = 5000;
    bool inrange = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.name == "pawl")
        {
            inrange = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision){
        if (collision.transform.name == "pawl")
        {
            Interact.turn_off();
            Cost_Popup.hide_price();
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
        if (inrange) {
            Interact.turn_on(isAffordable());
            Cost_Popup.show_price(cost, isAffordable());
        }
        if(Input.GetKey(KeyCode.E) && inrange && timer<=0 && center.transform.childCount > 0){
                if(PointsManager.PointValue >= cost){
                    ui.changeMessage("Enjoy your upgrade chief!");
                    PointsManager.PointValue -= cost;
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
                else if(PointsManager.PointValue < cost) {
                    ui.changeMessage("This cost 5000 points, get your money up LOL broke ahhh!!");
                }
            }
    }

    public bool isAffordable() {
        return PointsManager.PointValue >= cost;
    }
}
