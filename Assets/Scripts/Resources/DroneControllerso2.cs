using UnityEngine;

public class DroneControllerso2 : MonoBehaviour
{
    private GameObject bulletPrefab;
    private Transform firePoint;
    private float fireRate = 1.5f;
    private float lifetime = 30f; // tồn tại 30s

    private float fireTimer;
    private Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        Destroy(gameObject, lifetime); // tự hủy sau 30s
    }

    void Update()
    {
        // Quay quanh player
        if (player != null)
        {
            transform.RotateAround(player.position, Vector3.forward, 50 * Time.deltaTime);
        }

        // Bắn đạn
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }
    }

    private void Fire()
    {
        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
    }
}
