using System;
using UnityEngine;

public class WaitingToStartWindow : MonoBehaviour
{
    private void Start()
    {
        Bird.GetInstance().StartedPlaying += WaitingToStartWindow_OnStartedPlaying;
    }

    private void WaitingToStartWindow_OnStartedPlaying(object sender, EventArgs e)
    {
        Hide();
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
