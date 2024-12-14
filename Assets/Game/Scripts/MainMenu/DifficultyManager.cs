using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance;

    // Difficulty settings
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty currentDifficulty;

    // Enemy parameters based on difficulty
    // EASY
    public float easyAttackCooldown = 1.5f;
    public int easyAttackDamage = 3;
    public float easyMovementSpeed = 1.0f;
    // MEDIUM
    public float mediumAttackCooldown = 1.0f;
    public int mediumAttackDamage = 5;
    public float mediumMovementSpeed = 2.0f;
    // HARD
    public float hardAttackCooldown = 0.5f;
    public int hardAttackDamage = 8;
    public float hardMovementSpeed = 3.0f;

    void Awake()
    {
        // Ensure this manager persists across scenes
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

    public void SetDifficulty(string difficulty)
    {
        switch (difficulty.ToLower())
        {
            case "easy":
                currentDifficulty = Difficulty.Easy;
                break;
            case "medium":
                currentDifficulty = Difficulty.Medium;
                break;
            case "hard":
                currentDifficulty = Difficulty.Hard;
                break;
        }
    }
}