using System.Collections;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB "BOM HẸN GIỜ" (skill4_BombardmentPrefab)
public class SuperBoss_TargetedGround : MonoBehaviour
{
    [Header("Settings")]
    public float followSpeed = 8f; // Tốc độ bám theo
    public float warningTime = 1.5f; // Thời gian cảnh báo SAU KHI khóa

    [Header("Components (Kéo từ con)")]
    public GameObject warningVisual; // Sprite/Particle cảnh báo (Con)
    public GameObject explosionCollider; // GameObject chứa Collider + DameNo.cs (Con)

    private Transform playerTarget;
    private float followTimer = 0f;
    private float followDuration;
    private bool isActivated = false;
    private bool isLocked = false;

    void Start()
    {
        if (explosionCollider == null || warningVisual == null)
        {
            Debug.LogError("TargetedGround Prefab thiếu Visual hoặc Collider!");
            Destroy(gameObject);
            return;
        }
        // Tắt vùng nổ, bật cảnh báo
        explosionCollider.SetActive(false);
        warningVisual.SetActive(true);
    }

    /// <summary>
    /// Boss6Controller sẽ gọi hàm này
    /// </summary>
    public void Activate(Transform target, float duration)
    {
        this.playerTarget = target;
        this.followDuration = duration;
        this.isActivated = true;
    }

    void Update()
    {
        if (!isActivated || isLocked || playerTarget == null) return;

        // --- Giai đoạn 1: Bám theo Player ---
        if (followTimer < followDuration)
        {
            // Di chuyển prefab (Cha) đến vị trí Player
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, followSpeed * Time.deltaTime);
            followTimer += Time.deltaTime;
        }
        // --- Giai đoạn 2: Khóa và Kích nổ ---
        else
        {
            isLocked = true; // Dừng bám theo
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        Debug.Log("Bom đã khóa! Kích hoạt nổ...");
        // (Tùy chọn: Làm cho warningVisual nhấp nháy)

        // 1. Chờ hết thời gian cảnh báo (sau khi khóa)
        yield return new WaitForSeconds(warningTime);

        // 2. Ẩn cảnh báo
        warningVisual.SetActive(false);
        // 3. Bật vùng nổ (gắn DameNo.cs)
        explosionCollider.SetActive(true);

        // 4. Chờ 0.3s để DameNo.cs kịp gây sát thương
        yield return new WaitForSeconds(0.3f);

        // 5. Tự hủy
        Destroy(gameObject);
    }
}