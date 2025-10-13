using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class SoundManager
{
    public enum Sound
    {
        BirdJump,
        Score,
        Lose,
        ButtonOver,
        ButtonClick,
    }
    public static void PlaySound(Sound sound)
    {
        AudioClip audio = GetAudioClip(sound);
        if (audio == null) return;

        GameObject soundGameObject = new GameObject("Sound", typeof(AudioSource));
        Object.DontDestroyOnLoad(soundGameObject);
        AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(audio);
        Object.Destroy(soundGameObject, audio.length);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        GameAssets instance = GameAssets.GetInstanceSafe();
        if (instance == null)
        {
            return null;
        }

        foreach (GameAssets.SoundAudioClip clip in instance.soundAudioClipArray)
        {
            if (clip.sound == sound)
                return clip.audioClip;
        }

        return null;
    }

    public static void AddButtonSounds(this Button button)
    {
        if (button == null) return;

        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = button.gameObject.AddComponent<EventTrigger>();

        var entryEnter = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };
        entryEnter.callback.AddListener(_ => PlaySound(Sound.ButtonOver));
        trigger.triggers.Add(entryEnter);

        button.onClick.AddListener(() => PlaySound(Sound.ButtonClick));
    }
}
