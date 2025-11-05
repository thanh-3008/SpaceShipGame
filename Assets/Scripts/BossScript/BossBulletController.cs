using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    // Biến này sẽ được BossController gán giá trị khi tạo ra viên đạn
    public float normalSpeed;
    public float slowDownFactor = 5f; // Đặt hệ số làm chậm ở đây cho nhất quán

    // --- SỬA ĐỔI: THÊM BIẾN MỚI ---
    [Tooltip("Thời gian tự hủy tối đa (giây), đề phòng đạn kẹt hoặc sinh ra ngoài màn hình")]
    public float failsafeLifetime = 5f;
    // -------------------------------

    private Rigidbody2D rb;
    private SeraphMKII skillMKII;
    private PlayerController player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Tìm đối tượng Player và lấy script skill của nó
        skillMKII = FindObjectOfType<SeraphMKII>();
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");

        if (playerobj != null)
        {
            player = playerobj.GetComponent<PlayerController>();
        }

        // --- SỬA ĐỔI: THÊM TỰ HỦY DỰ PHÒNG ---
        // Đảm bảo đạn sẽ bị phá hủy sau một khoảng thời gian
        // ngay cả khi nó không bao giờ va chạm hoặc bay ra khỏi màn hình
        Destroy(gameObject, failsafeLifetime);
        // ---------------------------------------
    }

    void Update()
    {
        // 1. Tính toán tốc độ hiện tại dựa trên hiệu ứng làm chậm
        float currentSpeed = normalSpeed;
        if (skillMKII != null && skillMKII.lamchamthoigian)
        {
            currentSpeed /= slowDownFactor;
        }

        // 2. Cập nhật lại vận tốc của viên đạn
        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDame(10f);
            }
            Destroy(gameObject);
        }
        if (collision.CompareTag("TauMe"))
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Hàm này tự động được gọi bởi Unity khi
    /// Renderer (ví dụ SpriteRenderer) không còn
    /// hiển thị trên bất kỳ camera nào.
    /// </summary>
    void OnBecameInvisible()
    {
        // Đây là cách phá hủy nhanh và hiệu quả nhất
        Destroy(gameObject);
    }
}