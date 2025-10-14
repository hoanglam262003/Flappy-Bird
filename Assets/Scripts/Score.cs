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
    }
    private void Update()
    {
        scoreText.text = Level.GetInstance().GetScore().ToString();
    }
}
