using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    /// <summary>
    /// Activates the menu
    /// </summary>
    public void Activate()
    {
        // freezes time
        gameManager.TimeFrozen = true;
    }

    /// <summary>
    /// Reloads the main scene, restarting the game
    /// </summary>
    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    /// <summary>
    /// Returns to the main menu scene
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Resumes the game
    /// </summary>
    public void Resume()
    {
        // resumes time
        gameManager.TimeFrozen=false;

        // makes the menu invisible
        gameObject.SetActive(false);
    }

    private void Start()
    {
        // hides the menu
        Resume();
    }
}
