using UnityEngine;

public class SimpleAudioManager : Singleton<SimpleAudioManager>
{
    [SerializeField] private AudioSource audioSource;

    public static void Play(AudioClip clip, float volume = 1, bool pitch = false)
    {
        if (pitch) Instance.audioSource.pitch = Random.Range(0.95f, 1.05f);
        Instance.audioSource.volume = volume;

        Instance.audioSource.clip = clip;
        Instance.audioSource.Play();
    }
}
