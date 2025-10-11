using UnityEngine;

public class DroneControllerso2 : MonoBehaviour
{
    public Transform player;       // Vị trí spaceship chính
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 100f;
    public float fireRate = 1.2f;
    public GameObject bulletPrefab;
    public float bulletSpeed = 7f;

    private float angle;
    private float fireTimer;

    void Update()
    {
        if (player == null) return;

        // Tính toán vị trí quay quanh player
        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad)) * orbitRadius;
        transform.position = player.position + offset;

        // Tự động bắn
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * bulletSpeed;
    }
}
