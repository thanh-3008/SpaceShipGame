using UnityEngine;

// Gắn script này vào Prefab "Đạn Làm Chậm"
public class SlowBullet : MonoBehaviour
{
    [Tooltip("Tốc độ của người chơi sẽ bị nhân với số này (ví dụ: 0.667 là giảm 1/3)")]
    public float slowFactor = 0.667f;

    [Tooltip("Gắn hiệu ứng nổ/va chạm (nếu có)")]
    public GameObject hitEffect;

    [Tooltip("Đạn tự hủy sau bao lâu (nếu không trúng gì)")]
    public float lifetime = 5.0f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ va chạm với Player
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                // Gọi hàm làm chậm vĩnh viễn
                player.ApplySlow(slowFactor);
            }

            // Tạo hiệu ứng
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            // Hủy đạn
            Destroy(gameObject);
        }
    }
}