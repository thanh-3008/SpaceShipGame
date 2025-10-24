using UnityEngine;

public class DroneControllerso2 : MonoBehaviour
{
    [Header("Shooting Settings")]
    public GameObject bulletPrefab;       // Prefab đạn của drone
    public Transform firePoint;           // Vị trí bắn đạn
    public float fireRate = 1.5f;         // Tốc độ bắn
    public float bulletSpeed = 8f;        // Tốc độ bay của đạn

    [Header("Drone Settings")]
    public float rotationSpeed = 50f;     // Tốc độ quay quanh player
    public float orbitRadius = 2f;        // Khoảng cách với player
    public float lifetime = 30f;          // Drone tồn tại trong 30s

    private float fireTimer = 0f;
    private Transform player;

    void Start()
    {
        // Tìm player
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("DroneControllerso2: Không tìm thấy Player!");
        }

        // Tự hủy sau lifetime giây
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (player == null) return;

        // Drone quay quanh player theo vòng tròn
        transform.RotateAround(player.position, Vector3.forward, rotationSpeed * Time.deltaTime);

        // Giữ drone luôn hướng ra ngoài (quay theo trục Z)
        Vector3 direction = (transform.position - player.position).normalized;
        transform.up = direction;

        // Xử lý bắn đạn
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
        }
    }
}
