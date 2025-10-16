using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        highScoreText.text = "HIGH SCORE: " + HighScore.GetHighScore().ToString();
        Bird.GetInstance().Died += Score_OnDied;
        Bird.GetInstance().StartedPlaying += Score_StartedPlaying;
        Hide();
    }

    private void Score_StartedPlaying(object sender, EventArgs e)
    {
        Show();
    }

    private void Score_OnDied(object sender, EventArgs e)
    {
        Hide();
    }

    private void Update()
    {
        scoreText.text = Level.GetInstance().GetScore().ToString();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
