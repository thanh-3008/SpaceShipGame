using UnityEngine;
using System.Collections;

public class BossArcShooter : MonoBehaviour
{
    [Header("Projectile & Target")]
    public GameObject projectilePrefab; // prefab with Rigidbody2D
    public Transform player;            // assign player transform (optional, null -> use facingDir)

    [Header("Arc Settings")]
    public int bulletsPerArc = 13;      // số đạn trên 1 cung
    [Range(0f, 360f)]
    public float arcAngle = 90f;        // tổng góc cung (degree)
    public float startAngleOffset = -45f; // dịch chuyển bắt đầu (degree) — thường = -arcAngle/2

    [Header("Layers & Timing")]
    public int layers = 1;              // số lớp cung (ví dụ 2 lớp)
    public float layerAngleStep = 5f;   // xoay giữa các lớp
    public float fireInterval = 0.1f;   // delay giữa 2 viên trong 1 cung
    public float cooldown = 2f;         // cooldown giữa lần dùng skill

    [Header("Projectile Motion")]
    public float projectileSpeed = 6f;  // tốc độ ban đầu
    public float layerSpeedStep = 1f;   // tốc độ khác nhau giữa layer
    public bool useCurvedMotion = false; // bật uốn cong
    public float sideForce = 2f;         // lực bên để uốn cong (áp dụng liên tục)
    public float curveDuration = 1.2f;   // thời gian áp lực cong

    [Header("Misc")]
    public bool aimAtPlayer = true;
    public Vector2 fixedDirection = Vector2.right; // nếu not aimAtPlayer

    bool isCooling = false;

    public void UseArcSkill()
    {
        if (!isCooling)
            StartCoroutine(FireArcRoutine());
    }

    IEnumerator FireArcRoutine()
    {
        isCooling = true;

        // Tính góc hướng bắn gốc
        float baseAngle = 0f;
        if (aimAtPlayer && player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        }
        else
        {
            baseAngle = Mathf.Atan2(fixedDirection.y, fixedDirection.x) * Mathf.Rad2Deg;
        }

        // Start offset điều chỉnh theo mong muốn
        float half = arcAngle * 0.5f;
        float startAngle = baseAngle + startAngleOffset; // hoặc baseAngle - half

        // Nếu bạn muốn center = baseAngle: startAngle = baseAngle - half;
        startAngle = baseAngle - half + startAngleOffset;

        for (int layer = 0; layer < layers; layer++)
        {
            float layerAngleOffset = layer * layerAngleStep; // mỗi layer xoay 1 ít
            float currentSpeed = projectileSpeed + layer * layerSpeedStep;

            for (int i = 0; i < bulletsPerArc; i++)
            {
                float t = bulletsPerArc == 1 ? 0.5f : (float)i / (bulletsPerArc - 1); // 0..1
                float angle = startAngle + layerAngleOffset + t * arcAngle;
                SpawnProjectile(angle, currentSpeed);
                yield return new WaitForSeconds(fireInterval);
            }

            // Nếu muốn delay giữa các layer, uncomment:
            // yield return new WaitForSeconds(0.15f);
        }

        yield return new WaitForSeconds(cooldown);
        isCooling = false;
    }

    void SpawnProjectile(float angleDeg, float speed)
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = transform.position;
        GameObject p = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            float rad = angleDeg * Mathf.Deg2Rad;
            Vector2 vel = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
            rb.linearVelocity = vel;

            //if (useCurvedMotion)
            //{
            //    CurvedProjectile cp = p.AddComponent<CurvedProjectile>();
            //    cp.Init(sideForce, curveDuration);
            //}
        }

        // Optional: rotate sprite to velocity
        p.transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
}
