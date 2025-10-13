using System;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets instance;
    public static GameAssets GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        instance = this;
    }

    public Sprite pipeHead;
    public Transform pipeHeadPrefab;
    public Transform pipeBodyPrefab;

    public SoundAudioClip[] soundAudioClipArray;

    [Serializable]
    public class SoundAudioClip
    {
        public AudioClip audioClip;
        public SoundManager.Sound sound;
    }
}
