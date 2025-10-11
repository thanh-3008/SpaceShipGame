using UnityEngine;

public class BossBulletController : MonoBehaviour
{
    // Biến này sẽ được BossController gán giá trị khi tạo ra viên đạn
    public float normalSpeed;
    public float slowDownFactor = 5f; // Đặt hệ số làm chậm ở đây cho nhất quán

    private Rigidbody2D rb;
    private SeraphMKII skillMKII;
    private PlayerController player;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Tìm đối tượng Player và lấy script skill của nó
        // FindObjectOfType an toàn hơn trong trường hợp chỉ có 1 Player
        skillMKII = FindObjectOfType<SeraphMKII>();
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");
        player = playerobj.GetComponent<PlayerController>();
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
        // Luôn giữ hướng bay cũ (rb.velocity.normalized) và chỉ thay đổi độ lớn (tốc độ)
        rb.linearVelocity = rb.linearVelocity.normalized * currentSpeed;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDame(20f);
            DamePopUpGenerator.Instance.CreateHealthLossPopUp(transform.position, 20f);

            Destroy(gameObject);
        }
        if(collision.CompareTag("TauMe"))
        {
            Destroy(gameObject);
        }
    }
}