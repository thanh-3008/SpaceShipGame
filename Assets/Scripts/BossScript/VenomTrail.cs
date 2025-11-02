using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB VŨNG ĐỘC (PARTICLE)
public class VenomTrail : MonoBehaviour
{
    [Tooltip("Vũng độc tồn tại trong bao lâu (giây)")]
    public float lifetime = 10f;
    [Tooltip("Sát thương gây ra MỖI GIÂY")]
    public float damagePerSecond = 10f; // Bây giờ đây là sát thương thật

    // Timer để đếm ngược 1 giây
    private float damageTimer;

    void Start()
    {
        Destroy(gameObject, lifetime);

        // Đặt là 0 để gây sát thương ngay khi Player vừa chạm vào
        damageTimer = 0f;
    }

    // (Không cần OnTriggerEnter2D)

    // OnTriggerStay2D chạy trên vòng lặp FixedUpdate (vật lý)
    void OnTriggerStay2D(Collider2D other)
    {
        // Chỉ xử lý nếu va chạm với Player
        if (other.CompareTag("Player"))
        {
            // Đếm ngược timer bằng thời gian vật lý
            damageTimer -= Time.fixedDeltaTime;

            // Nếu timer <= 0 (tức là đã đủ 1 giây)
            if (damageTimer <= 0f)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    // --- SỬA ĐỔI QUAN TRỌNG ---
                    // Gọi hàm "quản lý" sát thương
                    // Gây sát thương (damagePerSecond) 
                    // (KHÔNG nhân với Time.deltaTime)
                    player.TakePoisonDamage(damagePerSecond);
                }

                // Đặt lại timer về 1 giây
                damageTimer = 1.0f;
            }
        }
    }

    // (Không cần OnTriggerExit2D)
}