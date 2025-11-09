using UnityEngine;
using UnityEngine.Events;

// GẮN SCRIPT NÀY LÊN CẢ BOSS VÀ PREFAB ONG CON
public class BossController : MonoBehaviour
{
    [Header("Health Stats (Riêng biệt)")]
    [Tooltip("Kéo thanh máu của boss NÀY vào đây (Có thể bỏ trống cho Ong Con)")]
    public ThanhMauThienThach thanhMau;
    [Tooltip("Máu riêng của boss hoặc ong con")]
    public float maxHealth = 100000f;
    public float currentHealth;

    // Tham chiếu đến script AI (skill) của chính boss này
    private IBossAI aiScript;

    [Header("Sự kiện (Event)")]
    [Tooltip("Phát tín hiệu khi boss này chết")]
    public UnityEvent OnBossDieEvent;
    void Start()
    {
        currentHealth = maxHealth;

        // Tự động tìm script AI (cho Boss)
        aiScript = GetComponent<IBossAI>();
        if (aiScript == null)
        {
            // Đây có thể là Ong Con, không cần log lỗi
            // Debug.Log("GameObject " + name + " không có IBossAI (Có thể là Minion).");
        }

        if (thanhMau == null)
        {
            thanhMau = GetComponentInChildren<ThanhMauThienThach>();
        }

        if (thanhMau != null)
        {
            thanhMau.capnhatthanhmau(currentHealth, maxHealth);
        }
    }

    // Dan.cs của Player sẽ gọi hàm NÀY
    public void TakeDame(float damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Đảm bảo không âm

        if (thanhMau != null)
        {
            thanhMau.capnhatthanhmau(currentHealth, maxHealth);
        }

        // *** KIỂM TRA CHUYỂN GIAI ĐOẠN (50% MÁU) ***
        if (currentHealth <= maxHealth * 0.5f)
        {
            // Thử lấy script AI của Boss 2
            BossWardenGoliath2D boss2AI = GetComponent<BossWardenGoliath2D>();
            if (boss2AI != null)
            {
                boss2AI.ActivateEnrage();
            }

            // --- (SỬA LỖI) ---
            // Thử lấy script AI của Boss 3 (ĐÃ DI CHUYỂN VÀO ĐÂY)
            Boss3Controller boss3AI = GetComponent<Boss3Controller>();
            if (boss3AI != null)
            {
                boss3AI.ActivateEnrage();
            }
            Boss4Controller boss4AI = GetComponent<Boss4Controller>();
            if (boss4AI != null)
            {
                boss4AI.ActivateEnrage();
            }
            Boss5Controller boss5AI = GetComponent<Boss5Controller>();
            if (boss5AI != null)
            {
                boss5AI.ActivateEnrage();
            }
            Boss6Controller boss6AI = GetComponent<Boss6Controller>();
            if (boss6AI != null)
            {
                boss6AI.ActivateEnrage();
            }
            // --- (KẾT THÚC SỬA LỖI) ---
        }

        // *** KIỂM TRA CHẾT ***
        if (currentHealth <= 0)
        {
            OnBossDieEvent.Invoke();
            // --- (CẬP NHẬT LOGIC CHẾT) ---
            if (aiScript != null)
            {
                // Nếu là BOSS (vì có IBossAI)
                aiScript.Die();
            }
           
            if (gameObject.GetComponent<BossWardenGoliath2D>() != null)
            {
                PlayerController player = FindObjectOfType<PlayerController>();
                if (player != null)
                {
                    player.RemoveSlow();
                }
            }

            Debug.Log(gameObject.name + " đã bị tiêu diệt!");
            Destroy(gameObject, 0.5f); // Hủy GameObject (Boss hoặc Ong Con)
        }
    }
}