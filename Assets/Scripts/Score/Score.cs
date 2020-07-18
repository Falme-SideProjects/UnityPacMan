using UnityEngine;

public class Score
{
    private const string HIGHSCORE = "highscore";

    private int score = 0;
    private int highscore = 0;

    public Score()
    {
        highscore = PlayerPrefs.GetInt(HIGHSCORE, 0);
    }
   
    public void AddScore(int value)
    {
        score += value;
        
        if(IsNewHighScore(score, highscore))
        {
            SetNewHighScore();
        }
    }

    public void AddScoreBasedOnItem(Items item)
    {
        AddScore((int)item);
    }

    public int GetScore()
    {
        return score;
    }

    public int GetHighScore()
    {
        return highscore;
    }

    public void ResetScore()
    {
        score = 0;
    }

    public bool IsNewHighScore(int currentScore, int lastHighScore)
    {
        return currentScore > lastHighScore;
    }

    private void SetNewHighScore()
    {
        highscore = score;
        PlayerPrefs.SetInt(HIGHSCORE, highscore);
    }

}
