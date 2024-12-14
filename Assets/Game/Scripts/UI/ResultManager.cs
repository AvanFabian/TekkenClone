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

    void Update(){
        foreach(FightingController fightingController in fightingController){
            if(fightingController.gameObject.activeSelf && fightingController.currentHealth <=0 )
            {
                SetResult("YOU LOSE!");
                return;
            }
        }

        foreach(OpponentAI opponentAI in opponentAI){
            if(opponentAI.gameObject.activeSelf && opponentAI.currentHealth <= 0)
            {
                SetResult("YOU WIN!");
                return;
            }
        }

         // Load Main Menu using M
        if (Input.GetKeyDown(KeyCode.M))
        {
            LoadMainMenu();
        }
    }
    void SetResult(string result){
        // Add score based on current difficulty
        GameManager.Instance.AddScoreByDifficulty(DifficultyManager.Instance.currentDifficulty);
        resultText.text = result;
        resultPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void LoadMainMenu(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
