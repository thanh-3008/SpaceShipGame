using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth22 : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Image healthBar; // gắn trong Inspector (tùy chọn)

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            // TODO: Gọi hàm chết, nổ, v.v.
        }
        UpdateHealthUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        // Có thể thêm âm thanh hoặc hiệu ứng hồi máu ở đây
        Debug.Log("Healed " + amount + " HP!");
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / maxHealth;
    }
}
