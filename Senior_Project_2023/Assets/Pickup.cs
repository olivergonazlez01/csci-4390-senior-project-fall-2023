using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    // Get reference to buttons, player, the gun controller, and the ui
    [SerializeField]
    public Image[] icon; public GameObject[] buttons;
    public GameObject player;
    public GunController gc;
    public UI ui;

    // Standing and Equipped as originally false
    bool standing = false;
    bool equipped = false;
    // Visual representation of the guns in inverntory
    SpriteRenderer sr;

    public void Update() {
        // if the gun is already in the player's inventory, do nothing
        if (this.gameObject.transform.parent == player.transform)   return;

        // Else if the player presses the e key and the gun is able to be picked up
        else if (Input.GetKey(KeyCode.E) && standing) {
            // Grab the center game object and the sprite of the gun
            Transform t = player.transform.Find("center");
            sr = this.gameObject.GetComponent<SpriteRenderer>();

            for (int i = 0; i < 2; i++) {
                // If the icon on the ui is not active, then activate it, set its sprite to the gun sprite
                // And change the color of the icon according to how many weapons the player currently has 
                if (!icon[i].gameObject.activeSelf) {
                    icon[i].gameObject.SetActive(true);
                    icon[i].sprite = sr.sprite;
                    // if player has 0 weapons in inventory, make icon 1's color green
                    buttons[i].gameObject.GetComponent<Image>().color = Color.green;
                    //  Else make icon 2's color green, and icon 1's color white
                    if (i == 1) {
                        buttons[0].gameObject.GetComponent<Image>().color = Color.white;
                    }
                    break;
                }
            }
            
            // Make the icon's sprite the sprite of the gun according to which icon is green
            if (buttons[0].gameObject.GetComponent<Image>().color == Color.green) {
                icon[0].sprite = sr.sprite;
            } else if (buttons[1].gameObject.GetComponent<Image>().color == Color.green) {
                icon[1].sprite = sr.sprite;
            }

            // If the player has more 2 guns, destroy the gun that was picked up first than the others
            // The player should always have only 2 guns
            if (t.childCount > 0) {
                for (int i = 0; i < t.childCount; i++) {
                    if (t.GetChild(i).gameObject.activeSelf) {
                        if (t.childCount >= 2) {
                            Destroy(t.GetChild(i).gameObject);
                        }
                        t.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }

            // Make the parent of the gun the center game object found in the player
            this.transform.parent = t;
            // Set equipped and standing values
            equipped = true;
            standing = false;
            // Set proper position and location of the gun according to its name
            if (this.transform.name == "Pistol")
            {
                this.transform.localPosition = new Vector3(-0.268f, -0.077f, 0);
                this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
                
            }
            else if (this.transform.name == "Sniper")
            {
                this.transform.localPosition = new Vector3(-0.18f, 0, 0);    
                this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, 180, transform.localEulerAngles.z);

                if (t.childCount > 1)
                    this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + 180, 180);
            }
            else if (this.transform.name == "Rifle")
            {
                this.transform.localPosition = new Vector3(-0.1f, -0.05f, 0f);
                this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
            }
            
            // Reinitialize all the variables of the gun that was picked up in the gun controller script
            gc.PickedUp();
            // Change the ui to reflect the bullets of the new gun that was picked up, and equipped
            ui.Change(gc.bulletCount, gc.bulletCountTotal);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        // If the gun has no parent, make it able to be picked up
        if (this.gameObject.transform.parent == null) {
            standing = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collider) {
        // After the player leaves it area, make it unable to be picked up
        standing = false;
    }

    public bool Equipped() {
        return equipped;
    }
}

