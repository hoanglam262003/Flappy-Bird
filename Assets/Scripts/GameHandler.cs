using UnityEngine;

public class GameHandler : MonoBehaviour
{
    private void Start()
    {
        GameObject gameObject = new GameObject("Pipe", typeof(SpriteRenderer));
        //gameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.GetInstance().pipeHead;
    }
}
