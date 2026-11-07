using Unity.Cinemachine;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const string ANIM_VELOCITY_Y = "VelocityY";
    private const string ANIM_IS_MOVING = "IsMoving";

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;
    
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5;
    private bool right = true;

    [Header("Looking")]
    [SerializeField] private float startLookSpeed = 2;
    [SerializeField] private float returnLookSpeed = 4;
    private Vector2 targetLookDirection;
    private bool isLooking;
    private CinemachinePositionComposer cinemachine;

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
        UserInput.OnStopJump += StopJump;
    }

    void OnDisable()
    {
        UserInput.OnMove -= Move;
        UserInput.OnStopMoving -= StopMove;

        UserInput.OnJump -= InputJump;
        UserInput.OnStopJump -= StopJump;
    }

    void Awake()
    {
        cinemachine = FindAnyObjectByType<CinemachinePositionComposer>();
    }

    void Update()
    {
        CoyoteJump();
        BufferJump();

        anim.SetFloat(ANIM_VELOCITY_Y, rb.linearVelocityY);
        cinemachine.Composition.ScreenPosition = Vector2.Lerp(cinemachine.Composition.ScreenPosition, targetLookDirection, (targetLookDirection == Vector2.zero ? returnLookSpeed : startLookSpeed) * Time.deltaTime);
    }

    #region Movement

    private void Move(Vector2 input)
    {
        if(isLooking && input.y != 0) return;
        if(input.x == 0)
        {
            StopMove();
            if(input.y != 0) Look(input.y);
            else Look(0);

            return;
        }
        else Look(0);

        rb.linearVelocityX = input.x * movementSpeed;

        bool lastRight = right;
        if(input.x > 0) right = true;
        else right = false;
        
        float multiplier = lastRight == right ? 1 : -1;
        transform.localScale = new Vector2(transform.localScale.x * multiplier, transform.localScale.y);

        anim.SetBool(ANIM_IS_MOVING, true);
    }
    private void StopMove()
    {
        rb.linearVelocityX = 0;
        anim.SetBool(ANIM_IS_MOVING, false);

        Look(0);
    }

    private void Look(float dir)
    {
        if (!IsGrounded()) dir = 0;

        targetLookDirection = new Vector2(0, dir * 0.25f);
        if(dir == 0)
        {
            isLooking = false;
            cinemachine.transform.position += new Vector3(0, 10, 0);
        }
        else isLooking = true;
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

    private void StopJump()
    {
        if(rb.linearVelocityY > 0) rb.linearVelocityY *= 0.5f;
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