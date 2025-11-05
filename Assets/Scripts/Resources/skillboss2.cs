using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class BossCircularSkill : MonoBehaviour
{
    [Header("Bullet & Pool")]
    public GameObject bulletPrefab;
    public int poolSize = 100;

    [Header("Circular Pattern")]
    public int bulletsPerBurst = 24;   // số viên mỗi vòng
    public float bulletSpeed = 6f;
    public float burstInterval = 2f;   // thời gian giữa 2 lần bắn (giữa các vòng)
    public float burstDuration = 0f;   // nếu >0 sẽ bắn liên tục trong thời gian này (sau đó dừng)
    public bool clockwise = true;

    [Header("Advanced / Spiral")]
    public bool spiralMode = false;    // bật nếu muốn từng vòng lệch góc => spiral
    public float angleOffsetPerBurst = 10f; // độ lệch mỗi vòng khi spiralMode = true
    public float initialAngle = 0f;    // góc bắt đầu

    [Header("Randomness")]
    [Range(0f, 30f)] public float angleRandomSpread = 0f; // +- độ nhiễu góc trên từng viên

    
    private float nextBurstTime = 0f;
    private float burstEndTime = 0f;
    private float currentAngleOffset = 0f;

    void Start()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError("BossCircularSkill: bulletPrefab chưa gán!");
            enabled = false;
            return;
        }

    
        nextBurstTime = Time.time;
        if (burstDuration > 0f) burstEndTime = Time.time + burstDuration;
    }

    void Update()
    {
        // Nếu burstDuration > 0, dừng khi hết thời gian
        if (burstDuration > 0f && Time.time > burstEndTime) return;

        if (Time.time >= nextBurstTime)
        {
            FireBurst();
            nextBurstTime = Time.time + burstInterval;

            // cập nhật offset cho spiral
            if (spiralMode)
                currentAngleOffset += angleOffsetPerBurst * (clockwise ? -1f : 1f);
        }
    }

    void FireBurst()
    {
        // góc bước giữa các viên
        float step = 360f / bulletsPerBurst;
        float baseAngle = initialAngle + currentAngleOffset;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            float angle = baseAngle + i * step;
            // thêm nhiễu nhỏ nếu cần
            angle += Random.Range(-angleRandomSpread, angleRandomSpread);

            // hướng từ góc (đổi sang radian)
            float rad = angle * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            SpawnBullet(transform.position, dir * bulletSpeed, angle);
        }
    }

    void SpawnBullet(Vector3 pos, Vector2 velocity, float rotationAngle)
    {
       
        //b.transform.position = pos;
        //b.transform.rotation = Quaternion.Euler(0f, 0f, rotationAngle);
       
    }

    // Tùy: hiển thị vòng bắn trong Scene bằng Gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
