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
    public float easyAttackCooldown;
    public int easyAttackDamage;
    public float easyMovementSpeed ;
    // MEDIUM
    public float mediumAttackCooldown;
    public int mediumAttackDamage;
    public float mediumMovementSpeed;
    // HARD
    public float hardAttackCooldown;
    public int hardAttackDamage;
    public float hardMovementSpeed;

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
