using UnityEngine;

public class OrbitingProjectile : MonoBehaviour
{
    public Transform center;          // trung tâm quay (player hoặc boss)
    public float orbitRadius = 2f;    // bán kính quay
    public float orbitSpeed = 90f;    // tốc độ quay (độ / giây)
    public float orbitAngle = 0f;     // góc ban đầu
    public bool followCenter = true;  // có di chuyển theo nhân vật không

    void Update()
    {
        if (center == null) return;

        // cập nhật góc quay
        orbitAngle += orbitSpeed * Time.deltaTime;

        // tính vị trí mới theo góc
        float rad = orbitAngle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * orbitRadius;
        transform.position = center.position + offset;

        // hướng ra ngoài (nếu muốn phi tiêu xoay mặt)
        transform.up = (transform.position - center.position).normalized;
    }
}


