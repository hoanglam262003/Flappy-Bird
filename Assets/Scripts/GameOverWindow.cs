using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text highScoreText;

    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private Button mainMenuButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.SampleScene);
        });
        retryButton.AddButtonSounds();

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
        mainMenuButton.AddButtonSounds();
        Hide();
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show(int score)
    {
        scoreText.text = score.ToString();
        gameObject.SetActive(true);
    }

    public void NewHighScore()
    {
        if (Level.GetInstance().GetScore() >= HighScore.GetHighScore())
        {
            highScoreText.text = "NEW HIGH SCORE!";
        }
        else
        {
            highScoreText.text = "HIGH SCORE: " + HighScore.GetHighScore();
        }
    }
}
