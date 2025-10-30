using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class HomingMissile2D : MonoBehaviour
{
    private Transform target;
    private float lifetime;
    private float speed;
    private float turnSpeed;
    private PlayerController player;
    private Rigidbody2D rb;

    [Tooltip("Gán prefab 'hieuungno' của bạn vào đây")]
    public GameObject explosionEffectPrefab;

    private Camera mainCamera;
    private float offScreenBuffer = 0.4f;

    // --- BIẾN MỚI ĐỂ QUẢN LÝ CHẾ ĐỘ ---
    private bool isHomingMode = false;
    // ----------------------------------

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        mainCamera = Camera.main;
    }

    private void Start()
    {
        GameObject playerobj = GameObject.FindGameObjectWithTag("Player");

        if (playerobj != null)
        {
            player = playerobj.GetComponent<PlayerController>();
        }
    }

    // Hàm cho Skill 1 (Rượt đuổi)
    public void SetTarget(Transform newTarget, float life, float spd, float turn)
    {
        target = newTarget;
        lifetime = life;
        speed = spd;
        turnSpeed = turn;
        isHomingMode = true; // <-- Bật chế độ rượt đuổi

        // Tự hủy sau 'lifetime' giây (kích hoạt OnDestroy)
        Destroy(gameObject, lifetime);
    }

    // --- HÀM MỚI CHO SKILL 2 (Bắn thẳng) ---
    public void SetBarrage(float life)
    {
        lifetime = life;
        isHomingMode = false; // <-- Tắt chế độ rượt đuổi

        // Vẫn cần tự hủy sau 1 thời gian (nếu nó không bay ra ngoài màn hình)
        Destroy(gameObject, lifetime);
    }
    // ---------------------------------------

    void Update()
    {
        // Hàm này bây giờ sẽ chạy cho CẢ 2 chế độ
        CheckIfOffScreen();
    }

    void FixedUpdate()
    {
        // --- SỬA LỖI: CHỈ CHẠY LOGIC NẾU LÀ TÊN LỬA RƯỢT ĐUỔI ---
        if (!isHomingMode)
        {
            // Nếu là đạn bắn thẳng (Barrage), không làm gì cả.
            // Rigidbody đã được gán vận tốc bởi Boss 2.
            return;
        }
        // ----------------------------------------------------

        // (Code rượt đuổi cũ)
        if (target == null)
        {
            // Nếu mất mục tiêu, bay thẳng
            rb.linearVelocity = (Vector2)transform.right * speed;
            return;
        }

        Vector2 directionToTarget = (Vector2)target.position - rb.position;
        directionToTarget.Normalize();

        float rotateAmount = Vector3.Cross(directionToTarget, transform.right).z;
        rb.angularVelocity = -rotateAmount * turnSpeed;

        // Luôn di chuyển về phía trước (hướng .right của sprite)
        rb.linearVelocity = transform.right * speed;
    }

    void CheckIfOffScreen()
    {
        if (mainCamera == null) return;

        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // Kiểm tra nếu bay ra ngoài màn hình
        if (viewportPos.x < (0 - offScreenBuffer) || viewportPos.x > (1 + offScreenBuffer) ||
            viewportPos.y < (0 - offScreenBuffer) || viewportPos.y > (1 + offScreenBuffer))
        {
            // Tự hủy (sẽ kích hoạt OnDestroy)
            Destroy(gameObject);
        }
    }

    // Hàm này bây giờ sẽ chạy cho CẢ 2 chế độ
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Tên lửa trúng Player!");
            if (player != null)
            {
                player.TakeDame(15f);
            }
            Destroy(gameObject); // (sẽ kích hoạt OnDestroy)
        }
        else if (other.CompareTag("tenLuaRuot"))
        {
            // (Tên lửa tự hủy khi va chạm tên lửa khác)
            Destroy(gameObject); // (sẽ kích hoạt OnDestroy)
        }
    }

    // Hàm này bây giờ sẽ chạy cho CẢ 2 chế độ
    void OnDestroy()
    {
        // Lỗi "hieuungno(Clone)" của bạn là do prefab nổ không tự hủy.
        // Bạn PHẢI thêm script AutoDestroyEffect.cs (ở file dưới)
        // vào prefab "hieuungno" của bạn.

        if (explosionEffectPrefab != null)
        {
            // Tạo ra hiệu ứng nổ
            GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

            // (Quan trọng) Tự động phá hủy hiệu ứng nổ sau một thời gian ngắn
            // *** HÃY GẮN SCRIPT AUTODESTROYEFFECT.CS VÀO PREFAB "hieuungno" ***
            // *** VÀ XÓA DÒNG "Destroy(explosion, 0.5f);" NÀY ĐI ***
            Destroy(explosion, 0.5f);
        }
    }
}

