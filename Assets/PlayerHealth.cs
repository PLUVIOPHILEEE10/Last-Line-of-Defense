using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField, Min(1f)] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("User Interface")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject restartButton;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead { get; private set; }

    private void Start()
    {
        currentHealth = maxHealth;
        IsDead = false;
        SetEndScreen(false);
        UpdateHealthUI();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f) return;

        currentHealth = Mathf.Max(0f, currentHealth - damage);
        ProceduralAudio.PlayPlayerHit();
        UpdateHealthUI();
        if (currentHealth <= 0f) Die();
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0f) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText == null) return;

        healthText.text = $"HP: {Mathf.CeilToInt(currentHealth)} / {Mathf.CeilToInt(maxHealth)}";
        healthText.color = currentHealth <= maxHealth * 0.3f
            ? new Color(1f, 0.25f, 0.2f)
            : Color.white;
    }

    private void Die()
    {
        IsDead = true;
        ProceduralAudio.PlayDefeat();
        SetEndScreen(true);

        FpsPlayerController controller = GetComponent<FpsPlayerController>();
        if (controller != null)
        {
            controller.SetInputEnabled(false);
            controller.enabled = false;
        }

        WeaponShooter weapon = GetComponentInChildren<WeaponShooter>();
        if (weapon != null) weapon.enabled = false;
        GameRuntime.ReleaseCursor();
    }

    private void SetEndScreen(bool visible)
    {
        if (gameOverText != null) gameOverText.SetActive(visible);
        if (restartButton != null) restartButton.SetActive(visible);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }

    private void OnValidate()
    {
        maxHealth = Mathf.Max(1f, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }
}
