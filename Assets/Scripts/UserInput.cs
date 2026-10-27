using System;
using UnityEngine;
using UnityEngine.InputSystem;

using static UnityEngine.InputSystem.InputAction;

public class UserInput : MonoBehaviour
{
    public const string MOVE_ACTION_NAME = "Move";
    public const string JUMP_ACTION_NAME = "Jump";
    public const string INTERACT_ACTION_NAME = "Interact";
    
    public static event Action<Vector2> OnMove;
    public static event Action OnStopMoving;
    private InputAction moveAction;
    private bool wasMoving = false;

    public static event Action OnJump;
    private InputAction jumpAction;

    public static event Action OnInteractStart;
    public static event Action OnInteractStop;
    private InputAction interactAction;

    [SerializeField] private PlayerInput playerInput;

    void Awake()
    {
        moveAction = playerInput.actions.FindAction(MOVE_ACTION_NAME);
        jumpAction = playerInput.actions.FindAction(JUMP_ACTION_NAME);
        interactAction = playerInput.actions.FindAction(INTERACT_ACTION_NAME);
    }

    void OnEnable()
    {
        jumpAction.started += OnJumpStarted;

        interactAction.started += OnInteractStarted;
        interactAction.canceled += OnInteractStopped;
    }

    void OnDisable()
    {
        jumpAction.started -= OnJumpStarted;

        interactAction.started -= OnInteractStarted;
        interactAction.canceled -= OnInteractStopped;
    }

    void Update()
    {
        if(moveAction.IsInProgress()) OnMoveTriggered();
        else if (wasMoving)
        {
            wasMoving = false;
            OnStopMoving?.Invoke();
        }
    }

    private void OnMoveTriggered()
    {
        wasMoving = true;
        OnMove?.Invoke(moveAction.ReadValue<Vector2>());
    }

    private void OnJumpStarted(CallbackContext context) => OnJump?.Invoke();

    private void OnInteractStarted(CallbackContext context) => OnInteractStart?.Invoke();
    private void OnInteractStopped(CallbackContext context) => OnInteractStop?.Invoke();
}