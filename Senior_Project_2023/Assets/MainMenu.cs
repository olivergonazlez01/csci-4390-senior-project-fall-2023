using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        // Load  the scene with the given name
        SceneManager.LoadScene("Final_Demo");
    }

    public void QuitGame() {
        // Quit game
        Application.Quit();
    }
}
