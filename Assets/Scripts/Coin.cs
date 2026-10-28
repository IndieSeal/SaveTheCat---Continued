using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    [SerializeField] private AudioClip blingSound;
    
    public void OnPickup()
    {
        SimpleAudioManager.Play(blingSound, 0.8f, true);
        gameObject.SetActive(false);
    }
}