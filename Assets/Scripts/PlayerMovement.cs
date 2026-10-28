using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5;
    private bool right = true;

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
        CoyoteJump();
        BufferJump();

        anim.SetFloat("VelocityY", rb.linearVelocityY);
    }

    #region Movement

    private void Move(Vector2 input)
    {
        rb.linearVelocityX = input.x * movementSpeed;

        bool lastRight = right;
        if(input.x > 0) right = true;
        else right = false;
        
        float multiplier = lastRight == right ? 1 : -1;
        transform.localScale = new Vector2(transform.localScale.x * multiplier, transform.localScale.y);

        anim.SetBool("IsMoving", true);
    }
    private void StopMove()
    {
        rb.linearVelocityX = 0;
        anim.SetBool("IsMoving", false);
    }

    #endregion
    
    #region Jumping

    private void CoyoteJump()
    {
        if(!IsGrounded()) coyoteJumpTimer -= Time.deltaTime;
        else if(rb.linearVelocityY <= 0) coyoteJumpTimer = coyoteJump;
    }

    private void BufferJump()
    {
        if(jumpBufferTimer <= 0) return;

        jumpBufferTimer -= Time.deltaTime;
        Jump();
    }

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

    #endregion

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