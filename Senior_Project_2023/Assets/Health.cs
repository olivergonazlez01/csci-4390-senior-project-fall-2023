using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    public Player pawl;
    public int HeartCount = 2;
    public Image[] hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < hearts.Length; i++) {
            if (i < pawl.health) {
                hearts[i].sprite = FullHeart;
            } else {
                hearts[i].sprite = EmptyHeart;
            }
        }
    }
}
