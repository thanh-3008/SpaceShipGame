using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    public float damageRate = 4f; // Số lần gây sát thương mỗi giây (2f = 2 lần/giây)
    public PlayerController playerController;

    private float nextDamageTime = 0f; // Thời điểm được phép gây sát thương tiếp theo

    private void OnTriggerStay2D(Collider2D other)
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
                    thienthach.TakeDame(playerController.damehientai*100f);
                }
            }
        }
    }
}