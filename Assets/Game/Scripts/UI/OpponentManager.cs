using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentManager : MonoBehaviour
{
    public GameObject[] opponentCharacters;

    void Start()
    {
        if (opponentCharacters.Length == 0)
        {
            Debug.LogError("No opponents assigned to the OpponentManager!");
            return;
        }

        ActivateRandomOpponent();
    }

    void ActivateRandomOpponent()
    {
        int randomIndex = Random.Range(0, opponentCharacters.Length);
        for (int i = 0; i < opponentCharacters.Length; i++)
        {
            if (i == randomIndex)
                opponentCharacters[i].SetActive(true);
            else
                opponentCharacters[i].SetActive(false);
        }
    }
}

