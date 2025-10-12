using CodeMonkey.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Button retryButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        });
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
