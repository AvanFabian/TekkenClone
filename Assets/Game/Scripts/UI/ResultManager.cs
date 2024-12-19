using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public GameObject resultPanel;
    public Text resultText;

    public FightingController[] fightingController; // Player Characters
    public OpponentAI[] opponentAI; // Opponent Characters

    private bool resultSet = false; // Flag to ensure result is set only once

    void Update()
    {
        if (resultSet) return; // If result already set, skip further checks

        // Check if all player characters are defeated
        foreach (FightingController fc in fightingController)
        {
            if (fc.gameObject.activeSelf && fc.currentHealth <= 0)
            {
                SetResultLose("YOU LOSE!");
                return;
            }
        }

        // Check if all opponent characters are defeated
        foreach (OpponentAI oa in opponentAI)
        {
            if (oa.gameObject.activeSelf && oa.currentHealth <= 0)
            {
                SetResultWin("YOU WIN!");
                return;
            }
        }

        // Load Main Menu using M
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMainMenu();
        }
    }

    void SetResultWin(string result)
    {
        if (resultSet) return; // Double-check to avoid multiple calls

        resultSet = true; // Set the flag to true to prevent further execution

        // Add score based on current difficulty
        GameManager.Instance.AddScoreByDifficulty(DifficultyManager.Instance.currentDifficulty);
        // Save progress
        GameManager.Instance.SaveProgress();

        resultText.text = result;
        resultPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    void SetResultLose(string result)
    {
        if (resultSet) return; // Double-check to avoid multiple calls

        resultSet = true; // Set the flag to true to prevent further execution

        // Add score based on current difficulty
        // GameManager.Instance.AddScoreByDifficulty(DifficultyManager.Instance.currentDifficulty);
        // Save progress
        GameManager.Instance.SaveProgress();

        resultText.text = result;
        resultPanel.SetActive(true);
        Time.timeScale = 0f; // Pause the game
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("MainMenu");
    }
}
