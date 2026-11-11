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
        if(force == Vector2.zero) return;
        
        Vector2 knockback = ((Vector2)transform.position - direction).normalized;
        Vector2 directionMultiplier = new Vector2(knockback.x >= 0 ? 1 : -1, knockback.y >= -0.2f ? 1 : -1);

        rb.AddForce(directionMultiplier * force * knockBackMultiplier, ForceMode2D.Impulse);
    }
}