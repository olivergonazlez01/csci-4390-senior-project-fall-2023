using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
This script will deactiveate the parent object, not just what it is attached too
The awareness gameobject will have to be set up mannual as well, due to the parent
being a tilemap
*/

public class Obstacle : MonoBehaviour
{
    private bool check = false;
    private string parentName;
    private int cost;
    public SFX_Controller soundController;

    void OnTriggerEnter2D (Collider2D collider) {
        if (collider.transform.name == "pawl") {
            check = true;
        }
    }

    void OnTriggerExit2D (Collider2D collider) {
        if (collider.transform.name == "pawl") {
            Interact.turn_off();
            Cost_Popup.hide_price();
            check = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        soundController = GameObject.Find("SFX_Controller").transform.GetComponent<SFX_Controller>();
        parentName = transform.parent.name;
        switch (parentName) {
            case "Tilemap_O1":
                cost = 2000;
                break;
            case "Tilemap_O2":
                cost = 1000;
                break;
            case "Tilemap_O3":
                cost = 1000;
                break;
            case "Tilemap_O4":
                cost = 1000;
                break;
            default: 
                cost = int.MaxValue;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (check) {
            Interact.turn_on(isAffordable());
            Cost_Popup.show_price(cost, isAffordable());
            if (Input.GetKeyUp(KeyCode.E)) {
                if (isAffordable()) {
                    Interact.turn_off();
                    Cost_Popup.hide_price();
                    soundController.playUnlock();
                    PointsManager.decrease(cost);
                    transform.parent.gameObject.SetActive(false);
                } else {
                    //error message
                }
            }
        }
    }

    // returns true if the play has enough points to buy
    private bool isAffordable() {
        return PointsManager.PointValue >= cost;
    }
}
