using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Player progression
    public int currentLevel = 1; // Start at level 1
    public int currentScore = 0;
    public int[] scoreToUnlockLevel = { 0, 100, 300, 600 }; // Example thresholds for levels
    public bool[] levelUnlocked = { true, false, false, false }; // Level 1 unlocked by default

    // Score points for difficulty
    public int easyScore = 50;
    public int mediumScore = 100;
    public int hardScore = 200;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Call this method when a fight is completed
    public void AddScoreByDifficulty(DifficultyManager.Difficulty difficulty)
    {
        switch (difficulty)
        {
            case DifficultyManager.Difficulty.Easy:
                currentScore += easyScore;
                break;
            case DifficultyManager.Difficulty.Medium:
                currentScore += mediumScore;
                break;
            case DifficultyManager.Difficulty.Hard:
                currentScore += hardScore;
                break;
        }

        Debug.Log("Score Added! Current Score: " + currentScore);
        CheckLevelUnlock();
    }

    // Check if the player has enough score to unlock the next level
    void CheckLevelUnlock()
    {
        for (int i = 1; i < scoreToUnlockLevel.Length; i++)
        {
            if (currentScore >= scoreToUnlockLevel[i] && !levelUnlocked[i])
            {
                levelUnlocked[i] = true;
                Debug.Log("Level " + (i + 1) + " Unlocked!");
            }
        }
    }

    // Method to load a stage
    public void LoadStage(int level, string stageName)
    {
        if (levelUnlocked[level - 1])
        {
            SceneManager.LoadScene(stageName);
        }
        else
        {
            Debug.Log("Stage Locked. Earn more score to unlock this level!");
        }
    }
}

