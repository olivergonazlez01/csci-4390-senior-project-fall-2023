using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor.UI;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UI : MonoBehaviour
{
    GameObject canvas;
    public GameObject panel;

    GameObject powerupTab;
    GameObject ik_powerup;
    GameObject dp_powerup;

    GameObject BombTab;
    GameObject grenade1;
    GameObject grenade2;
    GameObject yarn1;
    GameObject yarn2;

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

        powerupTab = canvas.transform.Find("Perk Tab").gameObject;
        BombTab = canvas.transform.Find("Item Tab").gameObject;

        ik_powerup = powerupTab.transform.Find("ik_powerup").gameObject;
        dp_powerup = powerupTab.transform.Find("dp_powerup").gameObject;

        ik_powerup.SetActive(false);
        dp_powerup.SetActive(false);

        grenade1 = BombTab.transform.Find("Grenade1").gameObject;
        grenade2 = BombTab.transform.Find("Grenade2").gameObject;
        yarn1 = BombTab.transform.Find("Yarn1").gameObject;
        yarn2 = BombTab.transform.Find("Yarn2").gameObject;


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

    public void instaKillActive(bool set)
    {
        ik_powerup.SetActive(set);
    }

    public void douPoiActive(bool set)
    {
        dp_powerup.SetActive(set);
    }

    public void grenadeUI(bool add = false)
    {
        if (add)
        {
            if (!grenade1.activeSelf)   grenade1.SetActive(true);
            else    grenade2.SetActive(true);
        }
        else
        {
            if (grenade2.activeSelf)    grenade2.SetActive(false);
            else    grenade1.SetActive(false);
        }
    }

    public void yarnUI(bool add = false)
    {
        if (add)
        {
            if (!yarn1.activeSelf)  yarn1.SetActive(true);
            else    yarn2.SetActive(true);
        }
        else
        {
            if(yarn2.activeSelf)    yarn2.SetActive(false);
            else    yarn1.SetActive(false);
        }
    }
}
