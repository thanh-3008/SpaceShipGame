using UnityEngine;
using System.Collections.Generic;

public class TroThuController : MonoBehaviour
{
    [Header("Level Hien Tai")]
    [SerializeField]
    private int currentLevel = 1; // Level 1 = 1 đạn, Level 5 = 5 đạn

    [Header("Thong So Co Ban")]
    public GameObject projectilePrefab; // Prefab của viên đạn
    public float fireRate = 1f;       // Số lần bắn mỗi giây
    public float range = 15f;         // Tầm tìm kiếm kẻ địch
    public float projectileSpeed = 10f; // --- THÊM VÀO: Tốc độ/Lực bắn của đạn
    public string[] enemyTags = { "Enemy", "Monster", "Boss" }; // Tag của kẻ địch
    public Transform firePoint;       // Điểm bắn đạn (nếu có, không thì dùng transform.position)

    [Header("Thong So Nang Cap")]
    public int maxLevel = 5;          // Level tối đa (tương ứng 5 viên đạn)
    public float maxSpreadAngle = 45f; // Góc xòe đạn tối đa

    private Transform target;
    private float fireCooldown;

    void Update()
    {
        FindNearestEnemy();
        HandleShooting();
    }

    void FindNearestEnemy()
    {
        List<GameObject> allEnemies = new List<GameObject>();
        foreach (string tag in enemyTags)
        {
            GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag(tag);
            allEnemies.AddRange(foundEnemies);
        }

        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in allEnemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void HandleShooting()
    {
        if (target == null) return;
        fireCooldown -= Time.deltaTime;
        if (fireCooldown <= 0f)
        {
            Shoot();
            fireCooldown = 1f / fireRate;
        }
    }

    void Shoot()
    {
        Vector3 spawnPosition = (firePoint != null) ? firePoint.position : transform.position;
        Vector3 directionToTarget = (target.position - spawnPosition).normalized;
        int projectileCount = Mathf.Min(currentLevel, maxLevel);

        if (projectileCount == 1)
        {
            InstantiateProjectile(spawnPosition, directionToTarget);
        }
        else
        {
            float startAngle = -maxSpreadAngle / 2f;
            float angleStep = maxSpreadAngle / (projectileCount - 1);

            for (int i = 0; i < projectileCount; i++)
            {
                float currentAngle = startAngle + (i * angleStep);
                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
                Vector3 shotDirection = rotation * directionToTarget;
                InstantiateProjectile(spawnPosition, shotDirection);
            }
        }
    }

    // --- SỬA HÀM NÀY ĐỂ DÙNG AddForce ---
    void InstantiateProjectile(Vector3 position, Vector3 direction)
    {
        // 1. Tính toán góc xoay (giữ nguyên)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion lookRotation = Quaternion.Euler(0, 0, angle);

        // 2. Instantiate viên đạn
        GameObject projectile = Instantiate(projectilePrefab, position, lookRotation);

        // 3. Lấy Rigidbody2D của đạn
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        // 4. Thêm lực đẩy
        if (rb != null)
        {
            // Dùng direction (hướng) nhân với tốc độ
            rb.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Prefab đạn thiếu Rigidbody2D!");
        }
    }
    // ------------------------------------


    // --- Các hàm Public (giữ nguyên) ---
    public void NangCapTroThu()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
        }
    }
    public void SetLevel(int level)
    {
        currentLevel = Mathf.Clamp(level, 1, maxLevel);
    }
    public int GetCurrentLevel()
    {
        return currentLevel;
    }
}