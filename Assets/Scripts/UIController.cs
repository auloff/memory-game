using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MemoryGame;

public class UIController : MonoBehaviour
{
    public Text pointsField;
    public GameController gameController;
    public string template;

    private void Start()
    {
        pointsField.text = template + " " + 0;
        gameController.ScoreChanged += GameController_ScoreChanged;
    }

    private void GameController_ScoreChanged(int obj)
    {
        pointsField.text = template + " " + obj.ToString();
    }
}
