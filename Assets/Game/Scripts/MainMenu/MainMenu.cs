using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject selectCharacterAndStageMenu;
    public GameObject optionsMenu;
    public GameObject ControlsMenu;
    public GameObject CreditsMenu;
    public GameObject difficultyMenu;

    void Start()
    {
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionsMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void PlayButtonClicked()
    {
        mainMenu.SetActive(false);
        selectCharacterAndStageMenu.SetActive(true);
    }

    public void OptionsButtonClicked()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void CreditsButtonClicked()
    {
        mainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void ControlsButtonClicked()
    {
        mainMenu.SetActive(false);
        ControlsMenu.SetActive(true);
        optionsMenu.SetActive(false);
        
 
    }

    public void BackButtonClicked()
    {
        mainMenu.SetActive(true);
        selectCharacterAndStageMenu.SetActive(false);
        optionsMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void ExitButtonClicked()
    {
        Application.Quit();
    }

    public void SelectCharacterClicked(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void SelectStageClicked(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
    public void SelectDifficultyClicked(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
