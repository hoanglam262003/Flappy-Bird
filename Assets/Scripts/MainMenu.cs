using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    private void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.SampleScene);
        });
        playButton.AddButtonSounds();
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        quitButton.AddButtonSounds();
    }
}
