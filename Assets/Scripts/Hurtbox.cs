using System;
using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    public event Action OnHurt;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent(out Hitbox hitbox)) return;

        Debug.Log("I got hit");
    }
}