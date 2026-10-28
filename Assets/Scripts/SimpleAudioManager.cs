using UnityEngine;

public class SimpleAudioManager : Singleton<SimpleAudioManager>
{
    [SerializeField] private AudioSource audioSource;

    public static void Play(AudioClip clip) => Instance.audioSource.PlayOneShot(clip);
}
