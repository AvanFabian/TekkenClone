using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text scoreText;
    public Text levelText;

    void Update()
    {
        scoreText.text = "Score: " + GameManager.Instance.currentScore;
        levelText.text = "Level: " + GameManager.Instance.currentLevel;
    }
}

