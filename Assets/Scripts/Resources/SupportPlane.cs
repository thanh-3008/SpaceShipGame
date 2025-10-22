using UnityEngine;

public class SupportPlane : MonoBehaviour
{
    public Transform player;          // Player mà trợ thủ sẽ đi theo
    public Vector2 offset = new Vector2(1f, 0f); // Vị trí tương đối so với Player
    public float followSpeed = 5f;    // Tốc độ di chuyển theo
    public GameObject bulletPrefab;   // Prefab đạn
    public float fireRate = 1f;       // Tốc độ bắn (viên/giây)
    public float bulletSpeed = 10f;   // Tốc độ bay của đạn

    private float nextFireTime = 0f;

    void Update()
    {
        if (player == null) return;

        // Tính vị trí đích của trợ thủ (theo offset so với Player)
        Vector3 targetPos = player.position + (Vector3)offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);

        // Tự động bắn
        if (Time.time >= nextFireTime)
        {
            Fire();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void Fire()
    {
        if (bulletPrefab == null) return;

        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = transform.up * bulletSpeed;
        }

        Destroy(bullet, 3f); // Hủy đạn sau 3s
    }
}
