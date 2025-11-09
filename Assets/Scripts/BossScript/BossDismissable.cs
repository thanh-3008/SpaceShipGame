using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB CỦA CẢ BOSS 5 VÀ BOSS 6
public class BossDismissable : MonoBehaviour
{
    private bool isDismissed = false;
    private Vector2 dismissDirection;
    private float dismissSpeed;
    private IBossAI aiScript;
    private Collider2D mainCollider;

    void Start()
    {
        // Tự động tìm các component
        aiScript = GetComponent<IBossAI>();
        mainCollider = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Hàm này được gọi bởi FinalBossManager
    /// </summary>
    public void Dismiss(Vector2 direction, float speed)
    {
        Debug.Log($"{gameObject.name} đang biến mất!");
        isDismissed = true;
        dismissDirection = direction;
        dismissSpeed = speed;

        // Tắt AI
        if (aiScript != null)
        {
            (aiScript as MonoBehaviour).enabled = false;
        }
        // Tắt va chạm
        if (mainCollider != null)
        {
            mainCollider.enabled = false;
        }
        // Tắt script máu (BossController) để nó không báo "chết"
        BossController bc = GetComponent<BossController>();
        if (bc != null) { bc.enabled = false; }

        // Failsafe: Tự hủy sau 10 giây
        Destroy(gameObject, 10f);
    }

    void Update()
    {
        if (isDismissed)
        {
            // Di chuyển liên tục theo hướng đã định
            transform.Translate(dismissDirection * dismissSpeed * Time.deltaTime);
        }
    }
}