using Unity.VisualScripting;
using UnityEngine;

public class SunMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    private float speed = 1.5f;
    public float damageRate = 4f; // Số lần gây sát thương mỗi giây (2f = 2 lần/giây)
    public PlayerController playerController;
    public SpawnSun SpawnSun;

    private float nextDamageTime = 0f; // Thời điểm được phép gây sát thương tiếp theo
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    // Trong file SunMove.cs

    void Update()
    {
        // Code cũ: Di chuyển theo hướng nghiêng của object
        // transform.Translate(transform.up * speed * Time.deltaTime);

        // ✅ Code mới: Luôn di chuyển thẳng đứng lên trên theo trục Y của thế giới game
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        if (transform.position.y > 15f)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerStay2D(Collider2D other)
    {
        if (rb != null)
        {
            if (other.CompareTag("Enemy"))
            {
                // Chỉ gây sát thương nếu thời gian hiện tại đã vượt qua thời điểm cho phép
                if (Time.time >= nextDamageTime)
                {
                    Debug.Log("Laser deals damage to " + other.name);

                    // Cập nhật thời điểm gây sát thương tiếp theo
                    nextDamageTime = Time.time + 1f / damageRate;

                    // Lấy component và gây sát thương
                    thienthachdichuyen thienthach = other.GetComponent<thienthachdichuyen>();
                    if (thienthach != null)
                    {
                        // Gợi ý: Tên hàm nên là "TakeDamage" để dễ đọc hơn
                        thienthach.TakeDame(playerController.damehientai * 200f);
                    }
                }

            }
        }
    }
}
