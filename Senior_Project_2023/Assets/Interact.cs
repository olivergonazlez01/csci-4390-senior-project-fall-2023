using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interact : MonoBehaviour
{
    private static Image popup;

    // to call from script: Interact.turn_on(bool)
    // turns on popup to tell the player to press e. if bool is false, turn image red
    public static void turn_on(bool isGood) {
        popup.enabled = true;
        popup.color = isGood ? Color.white : Color.red;
    }

    // to call from script: Interact.turn_off()
    // turns off popup
    public static void turn_off() {
        popup.enabled = false;
    }

    void Awake()
    {
        popup = transform.GetComponent<Image>();
        popup.enabled = false;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
