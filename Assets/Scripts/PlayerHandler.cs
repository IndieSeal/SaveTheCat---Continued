using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Health health;
    public Health Health => health;
    [SerializeField] private PlayerMovement playerMovement;
    public PlayerMovement Movement => playerMovement;
}