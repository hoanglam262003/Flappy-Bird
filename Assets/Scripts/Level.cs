using UnityEngine;

public class Level : MonoBehaviour
{
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10f;
    private const float PIPE_HEAD_HEIGHT = 4f;
    private void Start()
    {
        //CreatePipe(40f, 20f, true);
        //CreatePipe(40f, 20f, false);
        CreateGapPipe(50f, 40f, 0f);
    }

    private void CreateGapPipe(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * 0.5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * 0.5f, xPosition, false);
    }

    private void CreatePipe(float height, float xPosition, bool createBottom)
    {
        Transform pipeHead = Instantiate(GameAssets.GetInstance().pipeHeadPrefab);
        float pipeHeadYPosition;
        if (createBottom)
        {
            pipeHeadYPosition = -CAMERA_ORTHO_SIZE + height - PIPE_HEAD_HEIGHT;
        }
        else
        {
            pipeHeadYPosition = CAMERA_ORTHO_SIZE - height + PIPE_HEAD_HEIGHT;
            pipeHead.eulerAngles = new Vector3(0f, 0f, 180f);
        }
        pipeHead.position = new Vector3(xPosition, pipeHeadYPosition);

        Transform pipeBody = Instantiate(GameAssets.GetInstance().pipeBodyPrefab);
        float pipeBodyYPosition;
        if (createBottom)
        {
            pipeBodyYPosition = -CAMERA_ORTHO_SIZE;
        }
        else
        {
            pipeBodyYPosition = CAMERA_ORTHO_SIZE;
            pipeBody.localScale = new Vector3(1f, -1f, 1f);
        }
        pipeBody.position = new Vector3(xPosition, pipeBodyYPosition);

        SpriteRenderer pipeBodySpriteRenderer = pipeBody.GetComponent<SpriteRenderer>();
        pipeBodySpriteRenderer.size = new Vector2(PIPE_WIDTH, height);

        BoxCollider2D pipeBodyBoxCollider2D = pipeBody.GetComponent<BoxCollider2D>();
        pipeBodyBoxCollider2D.size = new Vector2(PIPE_WIDTH, height);
        pipeBodyBoxCollider2D.offset = new Vector2(0f, height * 0.5f);
    }
}
