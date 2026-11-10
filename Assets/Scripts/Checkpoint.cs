using System;
using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public static event Action OnCheckpointArrivedGlobal;
    
    public event Action OnArrived;
    public event Action OnReset;
    
    [SerializeField] private UnityEvent OnCheckpointArrivedEvents;
    [SerializeField] private UnityEvent OnCheckpointResetEvents;
    private PlayerHandler player;
    private bool dismissed;

    void OnDisable()
    {
        OnCheckpointArrivedGlobal -= ArriveAtNewCheckpoint;
        if(player != null) player.Health.OnDeath -= OnDeathReset;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryPlayerArriveAtCheckpoint(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        TryPlayerArriveAtCheckpoint(collision.gameObject);
    }

    private void TryPlayerArriveAtCheckpoint(GameObject collision)
    {
        if(this.player != null || !collision.TryGetComponent(out PlayerHandler player)) return;       

        // Arrived for the first time :]!
        this.player = player;
        player.Health.OnDeath += OnDeathReset;

        OnCheckpointArrivedGlobal?.Invoke();
        OnArrived?.Invoke();
        OnCheckpointArrivedEvents?.Invoke();

        OnCheckpointArrivedGlobal += ArriveAtNewCheckpoint; 
    }

    private void ArriveAtNewCheckpoint()
    {
        if(player != null) player.Health.OnDeath -= OnDeathReset;
    }

    private void OnDeathReset()
    {
        player.transform.position = transform.position;
        player.Movement.ResetCamera();

        OnReset?.Invoke();
        OnCheckpointResetEvents?.Invoke();
    }
}