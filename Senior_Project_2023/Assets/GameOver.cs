using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void PlayGameAgain()
    {
        // Load the scene with the given name
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame() {
        // Quit game
        Application.Quit();
    }
}