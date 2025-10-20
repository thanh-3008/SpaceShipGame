using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Settings")]
    public Slider healthBar;        // Thanh máu dạng slider
    public Gradient healthGradient; // Màu chuyển theo máu
    public Image fill;              // Ô màu fill của slider

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
            if (fill != null && healthGradient != null)
                fill.color = healthGradient.Evaluate(1f); // full máu = màu xanh
        }
    }

    /// <summary>
    /// Trừ máu player
    /// </summary>
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player HP: " + currentHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Hồi máu player
    /// </summary>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Player healed. Current HP: " + currentHealth);

        UpdateHealthUI();
    }

    /// <summary>
    /// Cập nhật thanh máu UI
    /// </summary>
    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;

            if (fill != null && healthGradient != null)
            {
                float t = (float)currentHealth / maxHealth;
                fill.color = healthGradient.Evaluate(t);
            }
        }
    }

    /// <summary>
    /// Xử lý khi player chết
    /// </summary>
    private void Die()
    {
        Debug.Log("Player chết!");
        // TODO: thêm xử lý GameOver UI, respawn, hoặc restart game
        gameObject.SetActive(false); // ẩn player (tạm thời)
    }
}
