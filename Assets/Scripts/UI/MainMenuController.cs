using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void StartGame()
    {
        // Load the main scene
        SceneManager.LoadScene("Main");
    }

    public void QuitGame()
    {
        // Quit the game
        Application.Quit();
    }
}
