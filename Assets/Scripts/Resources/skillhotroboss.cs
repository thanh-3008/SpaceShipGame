using UnityEngine;

public class CurvedProjectile : MonoBehaviour
{
    Rigidbody2D rb;
    float sideForce;
    float duration;
    float timer = 0f;

    // Tweak: bạn có thể dùng một hướng bên trái/phải ngẫu nhiên
    int sideSign = 1;

    public void Init(float sideForce, float duration)
    {
        this.sideForce = sideForce;
        this.duration = duration;
        rb = GetComponent<Rigidbody2D>();
        sideSign = Random.value > 0.5f ? 1 : -1;
        // Optional: giảm drag để chuyển động mượt
        if (rb) rb.drag = 0f;
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        if (timer < duration)
        {
            // Áp lực ngang vuông góc với vận tốc hiện tại để tạo cong
            Vector2 vel = rb.velocity;
            if (vel.sqrMagnitude > 0.001f)
            {
                Vector2 perp = new Vector2(-vel.y, vel.x).normalized * sideSign;
                rb.AddForce(perp * sideForce, ForceMode2D.Force);
            }
            timer += Time.fixedDeltaTime;
        }
        else
        {
            // Optionally: hủy component khi hết thời gian
            Destroy(this);
        }
    }

    // Optional: tự hủy sau 10s
    void Start() => Destroy(gameObject, 10f);
}
