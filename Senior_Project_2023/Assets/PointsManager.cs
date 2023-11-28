using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    public static int PointValue = 0;
    public TMP_Text Points;
    // Start is called before the first frame update
    void Start()
    {
        Points.text = "Balls";
        PointValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Points.text = "Points: " + PointValue;
    }

    public static void increase(int multiplier)
    {
        PointValue += 10 * multiplier;
    }
}
