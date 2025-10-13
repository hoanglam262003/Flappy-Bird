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
        GameObject soundGameObject = new GameObject("Sound", typeof(AudioSource));
        AudioSource audioSource = soundGameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.GetInstance().soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
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
