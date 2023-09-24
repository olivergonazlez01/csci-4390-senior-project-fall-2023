using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
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


}
