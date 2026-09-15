using System.Collections;
using UnityEngine;

public sealed class TargetHealth : MonoBehaviour
{
    [SerializeField, Min(1f)] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    private Renderer targetRenderer;
    private Color originalColor = Color.white;
    private bool isDead;

    public float CurrentHealth => currentHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;
        targetRenderer = GetComponentInChildren<Renderer>();
        if (targetRenderer != null)
        {
            originalColor = GetComponent<EnemyAI>() != null
                ? new Color(0.72f, 0.08f, 0.06f)
                : targetRenderer.material.color;
            targetRenderer.material.color = originalColor;
        }
    }

    public void Configure(float healthMultiplier)
    {
        maxHealth = Mathf.Max(1f, maxHealth * Mathf.Max(0.1f, healthMultiplier));
        currentHealth = maxHealth;
    }

    public bool TakeDamage(float damage)
    {
        if (isDead || damage <= 0f) return false;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
        if (targetRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashOnHit());
        }

        if (currentHealth <= 0f)
        {
            isDead = true;
            Destroy(gameObject);
        }

        return true;
    }

    private IEnumerator FlashOnHit()
    {
        targetRenderer.material.color = Color.white;
        yield return new WaitForSeconds(0.06f);
        if (targetRenderer != null) targetRenderer.material.color = originalColor;
    }

    private void OnValidate()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
}
