using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 50;
    
    void OnEnable()
    {
        UserInput.OnMove += Move;
        UserInput.OnStopMoving += StopMove;
    }

    void OnDisable()
    {
        UserInput.OnMove -= Move;
        UserInput.OnStopMoving -= StopMove;
    }

    private void Move(Vector2 input) => rb.linearVelocityX = input.x * movementSpeed;
    private void StopMove() => rb.linearVelocityX = 0;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IPickable pickable)) pickable.OnPickup();
    }
}