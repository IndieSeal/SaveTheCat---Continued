using System;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public event Action<Hitbox, Vector2> OnHurt;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent(out Hitbox hitbox)) return;

        OnHurt?.Invoke(hitbox, hitbox.KnockbackForce);
    }
}