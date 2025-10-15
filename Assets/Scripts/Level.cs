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
    private const float GROUND_DESTROY_X_POSITION = -200f;
    private const float CLOUD_DESTROY_X_POSITION = -160f;
    private const float CLOUD_SPAWN_X_POSITION = 160f;
    private const float CLOUD_SPAWN_Y_POSITION = 30f;
    private const float BIRD_X_POSITION = 0f;

    private static Level instance;

    public static Level GetInstance()
    {
        return instance;
    }

    private List<Transform> groundList;
    private List<Transform> cloudList;
    private float cloudSpawnTime;
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
        SpawnInitialGround();
        SpawnInitialClouds();
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
        state = State.BirdDead;
        int finalScore = GetScore();
        gameOverWindow.NewHighScore();
        gameOverWindow.Show(finalScore);
    }

    private void Update()
    {
        if (state == State.Playing)
        {
            PipeMovement();
            pipeSpawn();
            Ground();
            Cloud();
        }
    }

    private void SpawnInitialClouds()
    {
        cloudList = new List<Transform>();
        Transform cloud;
        cloud = Instantiate(GetCloudPrefabTransform(), new Vector3(0, CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
        cloudList.Add(cloud);
    }

    private Transform GetCloudPrefabTransform()
    {
        switch (Random.Range(0, 3))
        {
            default:
            case 0: return GameAssets.GetInstance().cloud_1;
            case 1: return GameAssets.GetInstance().cloud_2;
            case 2: return GameAssets.GetInstance().cloud_3;
        }
    }

    private void Cloud()
    {
        cloudSpawnTime -= Time.deltaTime;
        if (cloudSpawnTime < 0)
        {
            float cloudSpawnTimeMax = 6f;
            cloudSpawnTime = cloudSpawnTimeMax;
            Transform cloud = Instantiate(GetCloudPrefabTransform(), new Vector3(CLOUD_SPAWN_X_POSITION, CLOUD_SPAWN_Y_POSITION, 0), Quaternion.identity);
            cloudList.Add(cloud);
        }
        for (int i = 0; i < cloudList.Count; i++)
        {
            Transform cloud = cloudList[i];
            cloud.position += new Vector3(-1f, 0f, 0f) * PIPE_MOVE_SPEED * Time.deltaTime * 0.7f;

            if (cloud.position.x < CLOUD_DESTROY_X_POSITION)
            {
                Destroy(cloud.gameObject);
                cloudList.RemoveAt(i);
                i--;
            }

        }
    }

    private void SpawnInitialGround()
    {
        groundList = new List<Transform>();
        Transform ground;
        float groundY = -46f;
        float groundWidth = 183f;
        ground = Instantiate(GameAssets.GetInstance().ground, new Vector3(0, groundY, 0), Quaternion.identity);
        groundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().ground, new Vector3(groundWidth, groundY, 0), Quaternion.identity);
        groundList.Add(ground);
        ground = Instantiate(GameAssets.GetInstance().ground, new Vector3(groundWidth * 2f, groundY, 0), Quaternion.identity);
        groundList.Add(ground);
    }

    private void Ground()
    {
        foreach (Transform ground in groundList)
        {
            ground.position += new Vector3(-1f, 0f, 0f) * PIPE_MOVE_SPEED * Time.deltaTime;

            if (ground.position.x < GROUND_DESTROY_X_POSITION)
            {
                float rightMostXPosition = -100f;
                for (int i = 0; i < groundList.Count; i++)
                {
                    if (groundList[i].position.x > rightMostXPosition)
                    {
                        rightMostXPosition = groundList[i].position.x;
                    }
                }
                float groundWidth = 183f;
                ground.position = new Vector3(rightMostXPosition + groundWidth, ground.position.y, ground.position.z);
            }
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
                SoundManager.PlaySound(SoundManager.Sound.Score);
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
