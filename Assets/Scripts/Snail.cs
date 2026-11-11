using UnityEngine;

public class Snail : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Health health;

    [Header("Variables")]
    [SerializeField] private float movementSpeed = 5;
    private float movedDistance;

    void OnEnable()
    {
        health.OnDeath += OnDeath;
    }

    void OnDisable()
    {
        health.OnDeath -= OnDeath;
    }

    void Update()
    {
        if(rb.linearVelocityX < movementSpeed) rb.linearVelocityX = movementSpeed;
    }

    private void OnDeath()
    {
        gameObject.SetActive(false);
    }
}