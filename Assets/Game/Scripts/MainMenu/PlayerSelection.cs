using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSelection : MonoBehaviour
{
    public GameObject playerCharacters;
    private GameObject[] allCharacters;
    private int currentIndex = 0;

    void Start()
    {
        allCharacters = new GameObject[playerCharacters.transform.childCount];

        for (int i = 0; i < playerCharacters.transform.childCount; i++)
        {
            allCharacters[i] = playerCharacters.transform.GetChild(i).gameObject;
            allCharacters[i].SetActive(false);
        }
        if (PlayerPrefs.HasKey("SelectedCharacterIndex"))
        {
            currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex");
        }

        ShowCurrentCharacter();

    }
    

    void ShowCurrentCharacter()
    {
        foreach (GameObject character in allCharacters)
        {
            character.SetActive(false);
        }
        allCharacters[currentIndex].SetActive(true);
    }

    public void NextCharacter()
    {
        currentIndex = (currentIndex + 1) % allCharacters.Length;

        ShowCurrentCharacter();
    }

    public void PreviousCharacter()
    {
        currentIndex = (currentIndex - 1 + allCharacters.Length) % allCharacters.Length;
        
        ShowCurrentCharacter();
    }

    public void OnyesButtonClick(string sceneName)
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);
        PlayerPrefs.Save();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

       // Callback method for sceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're in the correct scene
        if (scene.name == "MainMenu")
        {
            GameObject objToDisable = GameObject.FindWithTag("MainMenuGameObj");
            GameObject objToEnable = GameObject.FindWithTag("SelectCharacter&StageMenu");

            if (objToDisable != null) objToDisable.SetActive(false);
            if (objToEnable != null) objToEnable.SetActive(true);

            // Unsubscribe from the event to prevent multiple calls
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

}
