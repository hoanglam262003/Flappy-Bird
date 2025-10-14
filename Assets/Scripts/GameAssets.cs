using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets GetInstanceSafe()
    {
        if (instance != null)
            return instance;

        instance = FindFirstObjectByType<GameAssets>();
        if (instance != null)
            return instance;

        return null;
    }
    public static GameAssets GetInstance() => GetInstanceSafe();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public Sprite pipeHead;
    public Transform pipeHeadPrefab;
    public Transform pipeBodyPrefab;
    public Transform ground;

    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public AudioClip audioClip;
        public SoundManager.Sound sound;
    }
}
