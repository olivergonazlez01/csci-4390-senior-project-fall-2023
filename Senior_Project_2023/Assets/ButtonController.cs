using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ButtonController : MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    public Image[] buttons; public Image[] slots;

    // Get references to player
    public GameObject player, prefab;
    public UI ui;

    void Start() {

    }
    
    void Update() {
        // If the player presses the 1 key and there is a gun in the first slot, then switch to the gun in the first slot
        if (Input.GetKeyUp("1"))    if (this.gameObject.GetComponent<Image>() == buttons[0])    SwitchGun(0);
        // If the player presses the 2 key and there is a gun in the second slot, then switch to the gun in the second slot
        if (Input.GetKeyUp("2"))    if (this.gameObject.GetComponent<Image>() == buttons[1])    SwitchGun(1);
    }

    public void SwitchGun(int data) {
        // If the player is clicking on key 1 and the current gun is in slot 1, do nothing
        if (data == 0 && buttons[0].gameObject.GetComponent<Image>().color == Color.green)  return;
        // If the player is clicking on key 2 and the current gun is in slot 2, do nothing
        if (data == 1 && buttons[1].gameObject.GetComponent<Image>().color == Color.green)  return;

        // Get reference to center game object inside player game object
        Transform t = player.transform.Find("center");

        // If the player has no weapons, do nothing
        if (t.childCount == 0)  return;
        // If the player clicks on a key, but the corresponding slot has not gun, do nothing
        if (!buttons[data].gameObject.transform.GetChild(0).gameObject.activeSelf)  return;

        // If the player clicks on key 1, the second slot will become white
        if (data == 0)  buttons[1].gameObject.GetComponent<Image>().color = Color.white;
        // Else if the player clicks on key 2, the first slot will become white
        else   buttons[0].gameObject.GetComponent<Image>().color = Color.white;
        
        // The corresponding slot of the key pressed will become green
        buttons[data].gameObject.GetComponent<Image>().color = Color.green;

        // For every gun the player has flip its activeSelf state (turn gun on if off, and turn gun off if on)
        for (int i = 0; i < t.childCount; i++) {
            // if the gun is not active, turn it on, and change the ui to show the number of bullets the gun has in the UI
            if (!t.GetChild(i).gameObject.activeSelf) {
                t.GetChild(i).gameObject.SetActive(true);
                GunController temp = t.GetChild(i).gameObject.GetComponent<GunController>();
                ui.Change(temp.bulletCount, temp.bulletCountTotal);
            } 
            // Else if the gun is active, simply turn it off
            else if (t.GetChild(i).gameObject.tag == "Weapon" && t.GetChild(i).gameObject.activeSelf) {
                t.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}