using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    public Image[] icon; public GameObject[] buttons;
    public GameObject player;
    public GunController gc;
    public UI ui;

    bool equipped = false;
    Sprite newestItemIcon;
    SpriteRenderer sr;
    

    private void OnTriggerStay2D(Collider2D collider) {
        if (Input.GetKey(KeyCode.E)) {
            Transform t = player.transform.Find("center");
            if (collider.gameObject.CompareTag("Player")) {
                if (icon[0].gameObject.activeSelf == false) {
                    icon[0].gameObject.SetActive(true);
                    sr = this.gameObject.GetComponent<SpriteRenderer>();
                    icon[0].sprite = sr.sprite;
                    buttons[0].gameObject.GetComponent<Image>().color = Color.green;

                } else if (icon[1].gameObject.activeSelf == false) {
                    for (int i = 0; i < t.childCount; i++) {
                        if (t.GetChild(i).gameObject.tag == "Weapon") {
                            if (t.GetChild(i).gameObject.activeSelf) {
                                t.GetChild(i).gameObject.SetActive(false);
                                sr = this.gameObject.GetComponent<SpriteRenderer>();
                                icon[1].sprite = sr.sprite;
                                icon[1].gameObject.SetActive(true);
                                buttons[1].GetComponent<Image>().color = Color.green;
                                buttons[0].GetComponent<Image>().color = Color.white;
                            } else {
                                Destroy(t.GetChild(i).gameObject);
                            }
                        }
                    }
                } else if (icon[1].gameObject.activeSelf && icon[0].gameObject.activeSelf) {
                    for (int i = 0; i < t.childCount; i++) {
                        if (t.GetChild(i).gameObject.tag == "Weapon") {
                            if (t.GetChild(i).gameObject.activeSelf) {
                                Destroy(t.GetChild(i).gameObject);
                            }
                        }
                    }
                    for (int i = 0; i < 2; i++) {
                        if (buttons[i].GetComponent<Image>().color == Color.green) {
                            buttons[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
                        }
                    }
                }
                this.transform.parent = t;
                equipped = true;
                gc.PickedUp();
                ui.Change();
                /*if (t.localScale.x > 0) {
                    this.transform.position = new Vector3(t.position.x - 0.4f, t.position.y, t.position.z);
                } else {
                    Vector3 flip = this.transform.localScale;
                    flip.x *= -1;
                    this.transform.localScale = flip;
                    this.transform.position = new Vector3(t.position.x + 0.4f, t.position.y, t.position.z);
                }*/
            }
        }
    }
    public bool Equipped() {
        return equipped;
    }
}

