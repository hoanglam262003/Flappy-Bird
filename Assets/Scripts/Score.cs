using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private void Update()
    {
        scoreText.text = Level.GetInstance().GetScore().ToString();
    }
}
