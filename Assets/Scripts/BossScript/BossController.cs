using UnityEngine;

// GẮN SCRIPT NÀY LÊN CẢ 2 BOSS (BOSS 1 VÀ BOSS 2)
// Script "Dan.cs" sẽ tìm thấy script này
public class BossController : MonoBehaviour
{
    [Header("Health Stats (Riêng biệt)")]
    [Tooltip("Kéo thanh máu của boss NÀY vào đây")]
    public ThanhMauThienThach thanhMau;
    [Tooltip("Máu riêng của boss này")]
    public float maxHealth = 100000f;
    public float currentHealth;

    // Tham chiếu đến script AI (skill) của chính boss này
    private IBossAI aiScript;

    void Start()
    {
        currentHealth = maxHealth;

        // Tự động tìm script AI (Boss1_AI hoặc BossWardenGoliath2D)
        // Miễn là chúng được gắn trên cùng 1 GameObject
        aiScript = GetComponent<IBossAI>();
        if (aiScript == null)
        {
            Debug.LogError("GameObject " + name + " có BossController nhưng thiếu script AI (Boss1_AI hoặc BossWardenGoliath2D)!");
        }

        if (thanhMau != null)
        {
            thanhMau.capnhatthanhmau(currentHealth, maxHealth);
        }
    }

    // Dan.cs sẽ gọi hàm NÀY
    public void TakeDame(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (thanhMau != null)
        {
            thanhMau.capnhatthanhmau(currentHealth, maxHealth);
        }

        // *** KIỂM TRA CHUYỂN GIAI ĐOẠN (Chỉ Boss 2) ***
        if (currentHealth <= maxHealth * 0.5f)
        {
            // Thử lấy script AI của Boss 2
            BossWardenGoliath2D boss2AI = GetComponent<BossWardenGoliath2D>();

            // Nếu tìm thấy (tức là đây là Boss 2)
            if (boss2AI != null)
            {
                // Ra lệnh cho Boss 2 kích hoạt
                boss2AI.ActivateEnrage();
            }
        }

        // *** KIỂM TRA CHẾT ***
        if (currentHealth <= 0)
        {
            if (aiScript != null)
            {
                // Ra lệnh cho script AI dừng lại
                aiScript.Die();
            }

            // (Kiểm tra nếu là Boss 2 thì gọi RemoveSlow)
            if (gameObject.GetComponent<BossWardenGoliath2D>() != null)
            {
                // Tìm PlayerController và gọi RemoveSlow
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.RemoveSlow();
                }
            }

            Debug.Log(gameObject.name + " đã bị tiêu diệt!");
            Destroy(gameObject, 0.5f); // Hủy boss này
        }
    }
}