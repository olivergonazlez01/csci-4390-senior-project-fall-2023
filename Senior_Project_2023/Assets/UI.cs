using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UI : MonoBehaviour
{
    GameObject canvas;
    public GameObject panel;

    public Text round;
    public Text level;
    public Text zombCount;
    public Text points;
    public Text bulletInClip;
    public Text bulletTotal;

    byte roundCount = 1;
    byte levelCount = 1;
    ushort pointsCount = 500;
    byte bulletClipCount = 0;
    ushort bulletTotalCount = 0;

    public MainController controller;
    public Player playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas Full UI");
        
        
        // panel = canvas.transform.Find("Item Tab").gameObject;
        // round = canvas.transform.Find("Round").GetComponentInChildren<Text>();
        // level = canvas.transform.Find("Level").GetComponentInChildren<Text>();
        // zombCount = canvas.transform.Find("Zombie Count").GetComponentInChildren<Text>();
        // points = canvas.transform.Find("Points").GetComponentInChildren<Text>();
        // bulletInClip = canvas.transform.Find("Bullets (Clip)").GetComponentInChildren<Text>();
        // bulletTotal = canvas.transform.Find("Bullets (Total)").GetComponentInChildren<Text>();
        // controller = transform.Find("Game Controller").GetComponentInChildren<MainController>();
        // playerInfo = transform.Find("pawl").GetComponentInChildren<Player>();


        panel.SetActive(false);

        round.text = roundCount.ToString();
        if (controller.storyMode)
        {
            level.text = levelCount.ToString();
            level.gameObject.SetActive(true);
        }
        else
            level.gameObject.SetActive(false);
        zombCount.text = controller.zombiesLeft.ToString();
        points.text = pointsCount.ToString();
        bulletInClip.text = bulletClipCount.ToString();
        bulletTotal.text = bulletTotalCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        zombCount.text = controller.zombiesLeft.ToString();
        if (controller.zombiesLeft == 0)
        {
            roundCount++;
            round.text =  roundCount.ToString();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
            panel.SetActive(true);

        if (Input.GetKeyUp(KeyCode.Tab))
            panel.SetActive(false);
    }

    // public static void Change()
    // {
    //     bulletClipCount = gun.bulletCount;
    //     bulletTotalCount = gun.bulletCountTotal;
    // }
    public void Change(byte clipSize, ushort totalBullets)
    {
        bulletClipCount = clipSize;
        bulletTotalCount = totalBullets;

        bulletInClip.text = clipSize.ToString();
        bulletTotal.text = totalBullets.ToString();
    }

}
