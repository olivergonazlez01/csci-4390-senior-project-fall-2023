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

    bool standing = false;
    bool equipped = false;
    Sprite newestItemIcon;
    SpriteRenderer sr;
    private void OnTriggerEnter2D(Collider2D collider) {
        if (this.gameObject.transform.parent == null) {
            Debug.Log("changing standing");
            standing = true;
        }
    }
    /*private void OnTriggerStay2D(Collider2D collider) {
        if (Input.GetKey(KeyCode.E) && standing) {
            Transform t = player.transform.Find("center");
            if (collider.gameObject.CompareTag("Player")) {
                for (int i = 0; i < 2; i++) {
                    if (!icon[i].gameObject.activeSelf) {
                        icon[i].gameObject.SetActive(true);
                        sr = this.gameObject.GetComponent<SpriteRenderer>();
                        icon[i].sprite = sr.sprite;
                        Debug.Log(icon[i]);
                        Debug.Log("test");
                        buttons[i].gameObject.GetComponent<Image>().color = Color.green;
                        break;
                    }
                }
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
                ui.Change(gc.bulletCount, gc.bulletCountTotal);
                /*if (t.localScale.x > 0) {
                    this.transform.position = new Vector3(t.position.x - 0.4f, t.position.y, t.position.z);
                } else {
                    Vector3 flip = this.transform.localScale;
                    flip.x *= -1;
                    this.transform.localScale = flip;
                    this.transform.position = new Vector3(t.position.x + 0.4f, t.position.y, t.position.z);
                }
            }
        } else {
            return;
        }
    }*/
    private void OnTriggerExit2D(Collider2D collider) {
        standing = false;
    }
    public bool Equipped() {
        return equipped;
    }

    public void Update() {
        if (this.gameObject.transform.parent == player.transform) {
            return;
        }
        else if (Input.GetKey(KeyCode.E) && standing) {
            Transform t = player.transform.Find("center");
            sr = this.gameObject.GetComponent<SpriteRenderer>();
            for (int i = 0; i < 2; i++) {
                if (!icon[i].gameObject.activeSelf) {
                    icon[i].gameObject.SetActive(true);
                    icon[i].sprite = sr.sprite;
                    buttons[i].gameObject.GetComponent<Image>().color = Color.green;
                    if (i == 1) {
                        buttons[0].gameObject.GetComponent<Image>().color = Color.white;
                    }
                    break;
                }
            }
            if (buttons[0].gameObject.GetComponent<Image>().color == Color.green) {
                icon[0].sprite = sr.sprite;
            } else if (buttons[1].gameObject.GetComponent<Image>().color == Color.green) {
                icon[1].sprite = sr.sprite;
            }
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
            this.transform.parent = t;
            equipped = true;
            standing = false;
            if (this.transform.name == "Pistol")
            {
                this.transform.localPosition = new Vector3(-0.268f, -0.077f, 0);
                this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
                if (t.parent.localScale.x > 0)
                    this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            }
            if (this.transform.name == "Sniper")
            {
                this.transform.localPosition = new Vector3(-0.18f, 0, 0);
                this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, 180, transform.localEulerAngles.z);
                if (t.parent.localScale.x > 0)
                    this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            }
            if (this.transform.name == "Rifle")
            {
                this.transform.localPosition = new Vector3(-0.1f, -0.05f, 0f);
                this.transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, 180);
                if (t.parent.localScale.x > 0)
                    this.transform.localScale = new Vector3(-this.transform.localScale.x, this.transform.localScale.y, this.transform.localScale.z);
            }
            gc.PickedUp();
            ui.Change(gc.bulletCount, gc.bulletCountTotal);
            /*if (icon[0].gameObject.activeSelf == false) {
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
            }*/
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

