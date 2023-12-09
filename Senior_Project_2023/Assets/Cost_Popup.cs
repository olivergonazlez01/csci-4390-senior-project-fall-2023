using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cost_Popup : MonoBehaviour
{
    private static Image popup;
    public static TextMeshProUGUI costString;

    // to call from script: Cost_Popup.show_price(int, bool)
    // turns on popup to show price. If bool is false, text turns red
    public static void show_price(int cost, bool isGood) {
        popup.enabled = true;
        costString.text = cost.ToString();
        costString.color = isGood ? Color.black : Color.red;
    }

    // to call from script: Cost_Popup.hide_price()
    // turns off price
    public static void hide_price() {
        popup.enabled = false;
        costString.text = "";
    }

    void Awake()
    {
        popup = transform.GetChild(0).GetComponent<Image>();
        costString = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        costString.text = "";
        popup.enabled = false;
    }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
