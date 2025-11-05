using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB "skill1_OrbPrefab"
[RequireComponent(typeof(Rigidbody2D))]
public class SuperBoss_TargetedOrb : MonoBehaviour
{
    [Tooltip("Tốc độ bay (sau khi được kích hoạt)")]
    public float speed = 15f;
    [Tooltip("Thời gian tồn tại (sau khi bắn)")]
    public float lifetime = 3f;
    public float damage = 15f;

    private Rigidbody2D rb;
    private bool isLaunched = false;
    private Vector2 targetDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // Tắt vật lý khi nó đang "dính" vào boss
        rb.isKinematic = true;
        rb.linearVelocity = Vector2.zero;
    }

    /// <summary>
    /// Hàm này được gọi bởi Boss 6
    /// </summary>
    public void Launch(Vector2 targetPosition)
    {
        // 1. TÁCH khỏi Boss (không còn là con nữa)
        transform.SetParent(null);

        // 2. Tính hướng bay (chỉ 1 lần)
        targetDirection = (targetPosition - (Vector2)transform.position).normalized;

        // 3. Kích hoạt vật lý và bay
        rb.isKinematic = false;
        rb.linearVelocity = targetDirection * speed;
        isLaunched = true;

        // 4. Tự hủy sau 3 giây
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Nếu nó chưa được bắn, nó sẽ đi theo boss (vì nó là con của boss)
        // Nếu nó đã được bắn, Rigidbody2D sẽ lo phần di chuyển
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Chỉ gây sát thương NẾU ĐÃ ĐƯỢC BẮN ĐI
        if (!isLaunched) return;

        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) { player.TakeDame(damage); }
            Destroy(gameObject);
        }
        // (Tùy chọn) Thêm va chạm với "Shield"
        if (other.CompareTag("Shield") || other.CompareTag("Khien"))
        {
            Destroy(gameObject);
        }
    }
}