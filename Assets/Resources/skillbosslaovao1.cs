using UnityEngine;

public class skillbosslaovao1 : MonoBehaviour
{
    public GameObject bulletPrefab;      // Prefab của viên đạn
    public Transform firePoint;          // Vị trí bắn đạn (empty object đặt ở miệng boss)
    public float bulletSpeed = 10f;      // Tốc độ đạn
    public float fireRate = 1.5f;        // Khoảng thời gian giữa các lần bắn
    public Transform player;             // Tham chiếu đến Player

    private float nextFireTime = 0f;

    void Update()
    {
        if (player == null) return; // Nếu chưa có Player, dừng lại

        // Nếu đủ thời gian thì bắn
        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        // Tạo viên đạn ở vị trí bắn
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Tính hướng bắn về phía Player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Gắn vận tốc cho đạn
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bulletSpeed;
        }

        // Xoay đạn theo hướng bắn (tùy vào sprite)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
