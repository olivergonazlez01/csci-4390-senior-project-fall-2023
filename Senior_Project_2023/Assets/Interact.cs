using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interact : MonoBehaviour
{
    private static GameObject popup;
    private Transform parent;

    // to call from script: Interact.turn_on
    // turns on popup to tell the player to press e
    public static void turn_on() {
        popup.SetActive(true);
    }

    // to call from script: Interact.turn_on
    // turns on popup to tell the player to press e
    public static void turn_off() {
        popup.SetActive(false);
    }

    void Awake()
    {
        popup = gameObject;
        popup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //popup.transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        // if (Input.GetAxisRaw("Horizontal") > 0) {
        //     //popup.GetComponent<SpriteRenderer>().flipX = true;
        // } else {
        //     //popup.GetComponent<SpriteRenderer>().flipX = false;
        // }
        //parent = popup.GetComponentInParent<Transform>();
        if  (popup.transform.parent.localScale.x < 0) {
            popup.GetComponent<SpriteRenderer>().flipX = true;
        } else {
            popup.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
