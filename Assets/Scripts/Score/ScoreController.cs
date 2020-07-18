using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Text playerOneScoreText;
    [SerializeField] private Text highScoreText;

    private Score score;
    public Score Score { get => GetScore(); }

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        playerOneScoreText.text = Score.GetScore().ToString();
        highScoreText.text = Score.GetHighScore().ToString();
    }

    private Score GetScore()
    {
        if (score == null) score = new Score();
        return this.score;
    }
    
}
