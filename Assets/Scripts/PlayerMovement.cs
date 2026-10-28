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
    [Space]
    [SerializeField] private float jumpBuffer = 0.1f;
    [SerializeField] private float coyoteJump = 0.2f;
    private float jumpBufferTimer;
    private float coyoteJumpTimer;
    
    void OnEnable()
    {
        UserInput.OnMove += Move;
        UserInput.OnStopMoving += StopMove;

        UserInput.OnJump += InputJump;
    }

    void OnDisable()
    {
        UserInput.OnMove -= Move;
        UserInput.OnStopMoving -= StopMove;

        UserInput.OnJump -= InputJump;
    }

    void Update()
    {
        if(!IsGrounded()) coyoteJumpTimer -= Time.deltaTime;
        else if(rb.linearVelocityY <= 0) coyoteJumpTimer = coyoteJump;

        if(jumpBufferTimer > 0)
        {
            jumpBufferTimer -= Time.deltaTime;
            Jump();
        }
    }

    private void Move(Vector2 input) => rb.linearVelocityX = input.x * movementSpeed;
    private void StopMove() => rb.linearVelocityX = 0;

    private void InputJump()
    {
        if(!Jump()) jumpBufferTimer = jumpBuffer;
    }

    private bool Jump()
    {
        if(!CanJump()) return false;

        rb.linearVelocityY = jumpForce;
        jumpBufferTimer = 0;
        coyoteJumpTimer = 0;
        return true;
    }

    private bool CanJump() => coyoteJumpTimer > 0 || !Application.isPlaying && IsGrounded();
    private bool IsGrounded() => Physics2D.OverlapCircle(transform.position, groundedRadius, groundedMask) != null;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IPickable pickable)) pickable.OnPickup();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = CanJump() ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, groundedRadius);
    }
}