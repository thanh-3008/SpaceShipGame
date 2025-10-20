using UnityEngine;

public class FanShoot : MonoBehaviour
{
    public GameObject bulletPrefab;   // Prefab viên đạn
    public float bulletSpeed = 10f;   // Tốc độ đạn
    public int bulletCount = 5;       // Số lượng đạn bắn ra
    public float spreadAngle = 60f;   // Góc tỏa (ví dụ 60 độ)
    public Transform firePoint;       // Vị trí bắn (Transform đầu súng)

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Nhấn Space để kích hoạt skill
        {
            ShootFan();
        }
    }

    void ShootFan()
    {
        float startAngle = -spreadAngle / 2f;   // Góc bắt đầu
        float angleStep = spreadAngle / (bulletCount - 1); // Khoảng cách giữa các viên

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = startAngle + (angleStep * i);
            Quaternion rot = firePoint.rotation * Quaternion.Euler(0, 0, angle);

            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rot);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.velocity = bullet.transform.up * bulletSpeed;
        }
    }
}
