using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 20;
    [SerializeField] private float groundedRadius = 0.2f;
    [SerializeField] private LayerMask groundedMask;
    
    void OnEnable()
    {
        UserInput.OnMove += Move;
        UserInput.OnStopMoving += StopMove;

        UserInput.OnJump += Jump;
    }

    void OnDisable()
    {
        UserInput.OnMove -= Move;
        UserInput.OnStopMoving -= StopMove;

        UserInput.OnJump -= Jump;
    }

    private void Move(Vector2 input) => rb.linearVelocityX = input.x * movementSpeed;
    private void StopMove() => rb.linearVelocityX = 0;

    private void Jump()
    {
        if(!IsGrounded()) return;

        rb.linearVelocityY = jumpForce;
    }

    private bool IsGrounded() => Physics2D.OverlapCircle(transform.position, groundedRadius, groundedMask) != null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IPickable pickable)) pickable.OnPickup();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = IsGrounded() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, groundedRadius);
    }
}