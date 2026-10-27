using UnityEngine;

public class Coin : MonoBehaviour, IPickable
{
    [SerializeField] private AudioSource blingSound;
    
    public void OnPickup()
    {
        blingSound.Play();
        gameObject.SetActive(false);
    }
}