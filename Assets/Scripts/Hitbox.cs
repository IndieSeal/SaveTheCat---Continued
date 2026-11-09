using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public event Action<Hurtbox> OnHit;

    public void OnHitboxHit(Hurtbox hurtbox) => OnHit?.Invoke(hurtbox);
}