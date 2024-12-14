using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Use TextMeshProUGUI for TextMeshPro objects
    public TextMeshProUGUI levelText;

    void Update()
    {
        if (GameManager.Instance != null)
        {
            scoreText.text = "Score: " + GameManager.Instance.currentScore;
            levelText.text = "Level: " + GameManager.Instance.currentLevel;
        }
    }
}

