using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

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
}
