using System;
using UnityEngine;

public class Health : Hurtbox
{
    public event Action<float> OnHealed;
    public event Action<float> OnDamaged;
    public event Action OnDeath;

    [Header("Variables")]
    [SerializeField] private float vulnerabilityTime = 0.4f;
    [SerializeField] private float maxHealth = 10;
    private float currentHealth;
    private float vulnerabilityTimer;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(IsInvulnerable()) vulnerabilityTimer -= Time.deltaTime;
    }

    public void AffectHealth(float health)
    {
        if(health == 0) return;
        
        if(health < 0)
        {
            vulnerabilityTimer = vulnerabilityTime;
            OnDamaged?.Invoke(health);
        }
        else OnHealed?.Invoke(health);
        
        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
        if(currentHealth == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }

    protected override void OnHit(Hitbox hitbox)
    {
        if(IsInvulnerable()) return;

        AffectHealth(-hitbox.Damage);
        base.OnHit(hitbox);
    }

    public bool IsInvulnerable() => vulnerabilityTimer > 0;
}