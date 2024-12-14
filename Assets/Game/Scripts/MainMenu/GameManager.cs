using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // Player progression
    public int currentLevel = 1; // Start at level 1
    public int currentScore = 0;
    public int[] scoreToUnlockLevel = { 0, 100, 300, 600 };
    public bool[] levelUnlocked = { true, false, false, false };

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
            LoadProgress(); // Load saved data
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        SaveProgress(); // Save progress after updating the score
    }

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

        // Update the current level to the highest unlocked level
        for (int i = levelUnlocked.Length - 1; i >= 0; i--)
        {
            if (levelUnlocked[i])
            {
                currentLevel = i + 1; // Levels are 1-based
                break;
            }
        }

        Debug.Log("Current Level: " + currentLevel);
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentScore", currentScore);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);

        // Save unlocked levels as a string
        for (int i = 0; i < levelUnlocked.Length; i++)
        {
            PlayerPrefs.SetInt("LevelUnlocked_" + i, levelUnlocked[i] ? 1 : 0);
        }

        PlayerPrefs.Save();
        Debug.Log("Progress Saved!");
    }

    public void LoadProgress()
    {
        if (PlayerPrefs.HasKey("CurrentScore"))
        {
            currentScore = PlayerPrefs.GetInt("CurrentScore");
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");

            for (int i = 0; i < levelUnlocked.Length; i++)
            {
                levelUnlocked[i] = PlayerPrefs.GetInt("LevelUnlocked_" + i) == 1;
            }

            Debug.Log("Progress Loaded!");
        }
        else
        {
            Debug.Log("No saved data found. Starting fresh.");
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        currentScore = 0;
        currentLevel = 1;
        levelUnlocked = new bool[] { true, false, false, false };

        SaveProgress();
        Debug.Log("Progress Reset!");
    }

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
