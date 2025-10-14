using UnityEngine;

public static class HighScore
{
    public static void Start()
    {
        Bird.GetInstance().Died += Bird_OnDied;
    }

    private static void Bird_OnDied(object sender, System.EventArgs e)
    {
        int score = Level.GetInstance().GetScore();
        SetHighScore(score);
    }
    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    public static bool SetHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            return true;
        }
        else
        {
            return false;
        }
    }
}
