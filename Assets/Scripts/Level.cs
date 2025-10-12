using CodeMonkey;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private GameOverWindow gameOverWindow;
    private const float CAMERA_ORTHO_SIZE = 50f;
    private const float PIPE_WIDTH = 10f;
    private const float PIPE_HEAD_HEIGHT = 4f;
    private const float PIPE_MOVE_SPEED = 30f;
    private const float PIPE_DESTROY_X_POSITION = -100f;
    private const float PIPE_SPAWN_X_POSITION = 100f;
    private const float BIRD_X_POSITION = 0f;

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    private List<Pipe> pipeList;
    private int pipesPassedCount;
    private float pipeSpawnTime;
    private float pipeSpawnTimeMax;
    private float gapSize;
    private int pipesSpawned;
    private State state;
    private enum State
    {
        WaitingToStart,
        Playing,
        BirdDead
    }

    public enum Difficulty
    {
        Easy,
        Medium,
        Hard,
        Asian
    }

    private void Awake()
    {
        instance = this;
        pipeList = new List<Pipe>();
        SetDifficulty(Difficulty.Easy);
        state = State.WaitingToStart;
    }

    private void Start()
    {
        Bird.GetInstance().Died += Bird_Died;
        Bird.GetInstance().StartedPlaying += Bird_StartedPlaying;
    }

    private void Bird_StartedPlaying(object sender, System.EventArgs e)
    {
        state = State.Playing;
    }

    private void Bird_Died(object sender, System.EventArgs e)
    {
        CMDebug.TextPopupMouse("Dead!");
        state = State.BirdDead;
        int finalScore = GetScore();
        gameOverWindow.Show(finalScore);
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            PipeMovement();
            pipeSpawn();
        }
    }

    private void PipeMovement()
    {
        for (int i = 0; i < pipeList.Count; i++)
        {
            Pipe pipe = pipeList[i];
            bool isTheRightOfBird = pipe.GetXPosition() > BIRD_X_POSITION;
            pipe.Move();
            if (isTheRightOfBird && pipe.GetXPosition() <= BIRD_X_POSITION)
            {
                pipesPassedCount++;
            }
            if (pipe.GetXPosition() < PIPE_DESTROY_X_POSITION)
            {
                pipe.SelfDestroy();
                pipeList.Remove(pipe);
                i--;
            }
        }
    }

    private void SetDifficulty(Difficulty difficulty)
    {
        switch (difficulty)
        {
            case Difficulty.Easy:
                gapSize = 50f;
                pipeSpawnTimeMax = 2f;
                break;
            case Difficulty.Medium:
                gapSize = 40f;
                pipeSpawnTimeMax = 1.8f;
                break;
            case Difficulty.Hard:
                gapSize = 30f;
                pipeSpawnTimeMax = 1.5f;
                break;
            case Difficulty.Asian:
                gapSize = 20f;
                pipeSpawnTimeMax = 1.2f;
                break;
        }
    }

    private Difficulty GetDifficulty()
    {
        if (pipesSpawned >= 30) return Difficulty.Asian;
        if (pipesSpawned >= 20) return Difficulty.Hard;
        if (pipesSpawned >= 10) return Difficulty.Medium;
        return Difficulty.Easy;
    }

    private void pipeSpawn()
    {
        pipeSpawnTime -= Time.deltaTime;
        if (pipeSpawnTime < 0)
        {
            pipeSpawnTime += pipeSpawnTimeMax;
            float heightEdgeLimit = 10f;
            float minHeight = gapSize * 0.5f + heightEdgeLimit;
            float totalHeight = CAMERA_ORTHO_SIZE * 2f;
            float maxHeight = totalHeight - gapSize * 0.5f - heightEdgeLimit;
            float height = Random.Range(minHeight, maxHeight);
            CreateGapPipes(height, gapSize, PIPE_SPAWN_X_POSITION);
        }
    }

    private void CreateGapPipes(float gapY, float gapSize, float xPosition)
    {
        CreatePipe(gapY - gapSize * 0.5f, xPosition, true);
        CreatePipe(CAMERA_ORTHO_SIZE * 2f - gapY - gapSize * 0.5f, xPosition, false);
        pipesSpawned++;
        SetDifficulty(GetDifficulty());
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

        Pipe pipe = new Pipe(pipeHead, pipeBody);
        pipeList.Add(pipe);
    }

    public int GetPipesSpawned()
    {
        return pipesSpawned;
    }

    public int GetScore()
    {
        return pipesPassedCount / 2;
    }

    private class Pipe
    {
        public Transform pipeHeadTransform;
        public Transform pipeBodyTransform;

        public Pipe(Transform pipeHeadTransform, Transform pipeBodyTransform)
        {
            this.pipeHeadTransform = pipeHeadTransform;
            this.pipeBodyTransform = pipeBodyTransform;
        }

        public void Move()
        {
            pipeHeadTransform.position += new Vector3(-1f, 0f, 0f) * PIPE_MOVE_SPEED * Time.deltaTime;
            pipeBodyTransform.position += new Vector3(-1f, 0f, 0f) * PIPE_MOVE_SPEED * Time.deltaTime;
        }

        public float GetXPosition()
        {
            return pipeHeadTransform.position.x;
        }

        public void SelfDestroy()
        {
            Destroy(pipeHeadTransform.gameObject);
            Destroy(pipeBodyTransform.gameObject);
        }
    }
}
