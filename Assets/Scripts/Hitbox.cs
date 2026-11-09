using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public event Action<Hurtbox> OnHit;

    [SerializeField] private Vector2 knockbackForce = Vector2.right;
    public Vector2 KnockbackForce => knockbackForce;

    [SerializeField] private float damage = 5;
    public float Damage => damage;

    public void OnHitboxHit(Hurtbox hurtbox) => OnHit?.Invoke(hurtbox);
}