using UnityEngine;

public class Knockback : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Optional")]
    [SerializeField] private Hurtbox hurtbox;

    [Header("Variables")]
    [SerializeField] private float knockBackMultiplier = 1;

    void OnEnable()
    {
        if(hurtbox != null) hurtbox.OnHurt += ApplyHurtbox;
    }

    void OnDisable()
    {
        if(hurtbox != null) hurtbox.OnHurt -= ApplyHurtbox;
    }

    private void ApplyHurtbox(Hitbox attacker, Vector2 force)
    {
        Apply(attacker.transform.parent.position, force);
    }

    public void Apply(Vector2 direction, Vector2 force)
    {
        Vector2 knockback = ((Vector2)transform.position - direction).normalized;
        rb.AddForce(knockback * force * knockBackMultiplier, ForceMode2D.Impulse);
    }
}