using System.Collections;
using System.Collections.Generic; // <<< THÊM DÒNG NÀY
using Unity.VisualScripting;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB ĐẠN PHÁO (FlakShell)
[RequireComponent(typeof(Rigidbody2D))]
public class FlakShell : MonoBehaviour
{
    [Header("Explosion Settings")]
    [Tooltip("Prefab của đạn nhỏ bắn ra khi nổ (ví dụ: slowBulletPrefab)")]
    public GameObject bulletPrefab;
    [Tooltip("Prefab hiệu ứng nổ (ví dụ: hieuungno)")]
    public GameObject explosionEffectPrefab;
    [Tooltip("Thời gian (giây) trước khi tự nổ")]
    public float timeToExplode = 2.5f;
    [Tooltip("Tốc độ của đạn nổ")]
    public float explosionBulletSpeed = 5f;

    [Header("Số lượng đạn nổ (Debug)")]
    [Tooltip("Số đạn nổ ở Giai đoạn 1")]
    public int bulletCountPhase1 = 4;
    [Tooltip("Số đạn nổ ở Giai đoạn 2")]
    public int bulletCountPhase2 = 8;

    private bool hasExploded = false;
    private bool isEnraged = false; // Cờ để biết GĐ 1 hay 2

    // --- SỬA ĐỔI (Hỗ trợ Req 3) ---
    // Danh sách này được trỏ tới từ BossWardenGoliath2D
    private List<GameObject> trackingList;
    // -----------------------------


    /// <summary>
    /// Hàm này được Boss 2 gọi ngay khi tạo ra đạn
    /// </summary>
    /// <param name="isEnragedPhase">True nếu boss đang ở Giai đoạn 2</param>
    public void Initialize(bool isEnragedPhase)
    {
        this.isEnraged = isEnragedPhase;
    }

    // --- HÀM MỚI (Hỗ trợ Req 3) ---
    /// <summary>
    /// Boss 2 sẽ gọi hàm này để gán danh sách
    /// mà nó muốn đạn pháo này báo cáo đạn con về.
    /// </summary>
    public void SetTrackingList(List<GameObject> list)
    {
        this.trackingList = list;
    }
    // -------------------------------

    void Start()
    {
        // Bắt đầu đếm ngược để nổ
        StartCoroutine(ExplosionTimer());

        // Hủy dự phòng, nếu nó không nổ vì lý do nào đó
        Destroy(gameObject, timeToExplode + 1f);
    }

    IEnumerator ExplosionTimer()
    {
        // Chờ
        yield return new WaitForSeconds(timeToExplode);

        // Nổ
        Explode();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Nếu va chạm với Player, nổ ngay lập tức
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            // --- SỬA ĐỔI: Thêm kiểm tra null ---
            if (player != null)
            {
                player.TakeDame(50f);
            }
            Explode();
        }
    }

    void Explode()
    {
        // Kiểm tra để đảm bảo chỉ nổ 1 lần
        if (hasExploded) return;
        hasExploded = true;

        if (bulletPrefab != null)
        {
            // --- SỬA ĐỔI: Chọn số lượng đạn dựa trên Giai đoạn ---
            int bulletCount = isEnraged ? bulletCountPhase2 : bulletCountPhase1;
            // ----------------------------------------------------

            float angleStep = 360f / bulletCount;
            for (int i = 0; i < bulletCount; i++)
            {
                float currentAngle = i * angleStep;
                Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);

                // Giả định đạn của bạn hướng Vector2.down (xuống)
                Vector2 direction = rotation * Vector2.down;

                // Tạo đạn
                GameObject bullet = Instantiate(bulletPrefab, transform.position, rotation);

                // --- SỬA ĐỔI (Req 1): Gán tốc độ cho đạn con ---
                BossBulletController bulletScript = bullet.GetComponent<BossBulletController>();
                if (bulletScript != null)
                {
                    bulletScript.normalSpeed = explosionBulletSpeed;
                }
                // ---------------------------------------------

                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * explosionBulletSpeed;
                }

                // --- SỬA ĐỔI (Req 3): Thêm đạn con vào danh sách theo dõi ---
                if (trackingList != null)
                {
                    trackingList.Add(bullet);
                }
                // -------------------------------------------------------
            }
        }

        // Tạo hiệu ứng nổ
        if (explosionEffectPrefab != null)
        {
            // (Đảm bảo prefab 'hieuungno' có script AutoDestroyEffect.cs)
            GameObject effect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 0.5f);
        }

        // Hủy đạn pháo
        Destroy(gameObject);
    }
}