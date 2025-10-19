using UnityEngine;

public class OrbitingController : MonoBehaviour
{
    public GameObject projectilePrefab;  // Prefab phi tiêu
    public int projectileCount = 4;      // số phi tiêu xoay quanh
    public float orbitRadius = 2f;       // bán kính
    public float orbitSpeed = 90f;       // tốc độ quay

    private GameObject[] projectiles;

    void Start()
    {
        SpawnOrbitingProjectiles();
    }

    void SpawnOrbitingProjectiles()
    {
        projectiles = new GameObject[projectileCount];

        for (int i = 0; i < projectileCount; i++)
        {
            GameObject obj = Instantiate(projectilePrefab);
            obj.transform.position = transform.position;

            OrbitingProjectile orbit = obj.AddComponent<OrbitingProjectile>();
            orbit.center = transform;
            orbit.orbitRadius = orbitRadius;
            orbit.orbitSpeed = orbitSpeed;
            orbit.orbitAngle = i * (360f / projectileCount); // chia đều góc

            projectiles[i] = obj;
        }
    }

    // Nếu muốn bắn ra khi nhấn phím
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootAllProjectiles();
        }
    }

    void ShootAllProjectiles()
    {
        foreach (var obj in projectiles)
        {
            if (obj == null) continue;

            var rb = obj.GetComponent<Rigidbody2D>();
            if (rb == null) rb = obj.AddComponent<Rigidbody2D>();

            // ngắt kết nối
            var orbit = obj.GetComponent<OrbitingProjectile>();
            orbit.center = null;

            // bắn ra theo hướng hiện tại
            rb.velocity = obj.transform.up * 10f;

            // tự hủy sau 5s
            Destroy(obj, 5f);
        }

        // clear mảng
        projectiles = new GameObject[0];
    }
}
