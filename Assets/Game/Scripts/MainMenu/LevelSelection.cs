using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Button[] stageButtons; // Buttons for stages, set in Inspector

    void Start()
    {
        UpdateStageButtons();
    }

void UpdateStageButtons()
{
    for (int i = 0; i < stageButtons.Length; i++)
    {
        bool isUnlocked = GameManager.Instance.levelUnlocked[i];
        
        // Set interactable state
        stageButtons[i].interactable = isUnlocked;

        // Adjust button opacity
        Image buttonImage = stageButtons[i].GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, isUnlocked ? 1f : 0.15f);
        }
    }
}

    // Call this method when a stage button is clicked
    public void LoadStages(int level, string stageName)
    {
        GameManager.Instance.LoadStage(level, stageName);
    }
}

