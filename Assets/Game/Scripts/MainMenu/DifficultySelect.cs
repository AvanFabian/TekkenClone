using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelect : MonoBehaviour
{
    public void SetEasyDifficulty()
    {
        DifficultyManager.Instance.SetDifficulty("easy");
        LoadGameScene();
    }

    public void SetMediumDifficulty()
    {
        DifficultyManager.Instance.SetDifficulty("medium");
        LoadGameScene();
    }

    public void SetHardDifficulty()
    {
        DifficultyManager.Instance.SetDifficulty("hard");
        LoadGameScene();
    }

    void LoadGameScene()
    {
        // Load your main game scene
        SceneManager.LoadScene("MainMenu");
    }
}

