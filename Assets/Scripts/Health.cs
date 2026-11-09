using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnDeath;

    [Header("Optional")]
    [SerializeField] private Hurtbox hurtbox;

    [Header("Variables")]
    [SerializeField] private float maxHealth = 10;
    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void OnEnable()
    {
        if(hurtbox != null) hurtbox.OnHurt += AffectHealthHurtbox;
    }

    void OnDisable()
    {
        if(hurtbox != null) hurtbox.OnHurt -= AffectHealthHurtbox;
    }

    private void AffectHealthHurtbox(Hitbox h, Vector2 f)
    {
        AffectHealth(-h.Damage);
    }

    public void AffectHealth(float health)
    {
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
        if(currentHealth == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke();
        gameObject.SetActive(false);
    }
}