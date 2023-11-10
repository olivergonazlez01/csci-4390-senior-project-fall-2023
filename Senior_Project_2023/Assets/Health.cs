using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    // Get references to the player, the set of images, and the full heart and empty heart images
    public Player pawl;
    public Image[] hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    
    // Initiate the number of hearts
    public int HeartCount = 2;

    void Start()
    {
        
    }

    void Update()
    {
        // For all the possible hearts, check against the health of the player, and assign the heart images
        for (int i = 0; i < hearts.Length; i++) {
            if (i < pawl.health) {
                hearts[i].sprite = FullHeart;
            } else {
                hearts[i].sprite = EmptyHeart;
            }
        }
    }
}
