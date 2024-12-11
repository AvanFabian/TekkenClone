using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    void Update()
    {
        // Toggle Pause/Resume using Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }

        // Resume using Space
        if (Input.GetKeyDown(KeyCode.Space) && isPaused)
        {
            Resume();
        }

        // Load Main Menu using M
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMainMenu();
        }

        // Quit Game using Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            QuitGame();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenuUI.SetActive(true);
        // Cursor.lockState = CursorLockMode.None;
        // Cursor.visible = true;
    }

    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
