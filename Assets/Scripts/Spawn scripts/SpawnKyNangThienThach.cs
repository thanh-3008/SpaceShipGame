// Gắn script này vào Player
using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnKyNangThienThach : MonoBehaviour
{
    // === CÀI ĐẶT SKILL (Kéo vào từ Inspector) ===
    [Header("Cài đặt")]
    public GameObject meteorPrefab; // 1. Prefab thiên thạch (Shuriken)
    public Transform firePoint;     // 2. Vị trí bắn (Empty Object con của Player)
    public float launchForce = 500f;// 3. Lực đẩy (Đã sửa giá trị mặc định, 5f quá nhỏ)
    public int soThienThach = 1;    // Số thiên thạch bắn ra mỗi lần dùng kỹ năng
    [Tooltip("Tổng góc tỏa ra tối đa của loạt đạn")]
    public float gocToaToiDa = 45f;
    [Tooltip("Mỗi viên đạn (sau viên đầu tiên) sẽ cộng thêm bao nhiêu độ vào tổng góc bắn")]
    public float gocTangMoiVienDan = 15f;
    public int soDem = 0;

    // === LOGIC HỒI CHIÊU (ĐÃ SỬA) ===
    [Header("Hồi Chiêu")]
    public float cooldown = 5f;       // Thời gian hồi chiêu (sẽ được nâng cấp)
    private float currentTimer;     // Thời gian đếm ngược

    // Biến cờ để kiểm tra nâng cấp cuối
    private bool isUpgradedCuoi = false;

    // Tham chiếu đến Player
    public PlayerController playerController;

    void Awake()
    { // <-- ĐÃ XÓA CHỮ 'D' BỊ LỖI
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("Thiếu PlayerController trên " + gameObject.name + "!");
        }
    }

    void Update()
    {
        // === LOGIC HỒI CHIÊU (ĐÃ SỬA) ===
        currentTimer -= Time.deltaTime;
        if (currentTimer <= 0)
        {
            CastSkill();
            currentTimer = cooldown; // Reset timer bằng thời gian hồi chiêu
        }

        // (Input để test nâng cấp cuối)
        if (Input.GetKeyDown(KeyCode.K))
        {
            NangCapCuoi();
        }
    }

    void CastSkill()
    {
        // 3. SET VỊ TRÍ BẮN
        Vector3 spawnPosition = (firePoint != null) ? firePoint.position : transform.position;

        // === KIỂM TRA ĐÃ NÂNG CẤP CUỐI CHƯA ===
        if (isUpgradedCuoi)
        {
            // *** KIỂU BẮN 1: BẮN 360 ĐỘ ***
            float buocNhayGoc = 360f / soThienThach;

            for (int i = 0; i < soThienThach; i++)
            {
                float gocHienTai = i * buocNhayGoc;
                Vector2 huongBan = Quaternion.Euler(0, 0, gocHienTai) * Vector2.up;
                GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
                SetupMeteor(meteor, huongBan);
            }
        }
        else
        {
            // *** KIỂU BẮN 2: BẮN VỀ PHÍA KẺ ĐỊCH ***
            Transform nearestEnemy = FindNearestEnemy();
            if (nearestEnemy == null)
            {
                Debug.Log("Không tìm thấy mục tiêu!");
                return;
            }

            Vector2 fireDirection = ((Vector2)nearestEnemy.position - (Vector2)spawnPosition).normalized;

            if (soThienThach == 1)
            {
                Debug.Log("tao ra thien thach 1");
                GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
                SetupMeteor(meteor, fireDirection);
            }
            else if (soThienThach >= 2)
            {
                float tongGocHienTai = Mathf.Min(gocToaToiDa, (soThienThach - 1) * gocTangMoiVienDan);
                float buocNhayGoc = (soThienThach > 1) ? tongGocHienTai / (soThienThach - 1) : 0;
                float gocBatDau = -tongGocHienTai / 2;

                for (int i = 0; i < soThienThach; i++)
                {
                    float gocHienTai = gocBatDau + i * buocNhayGoc;
                    Vector2 huongDaXoay = Quaternion.Euler(0, 0, gocHienTai) * fireDirection;
                    GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
                    SetupMeteor(meteor, huongDaXoay);
                }
            }
        }
    }

    // === HÀM HỖ TRỢ ===
    void SetupMeteor(GameObject meteor, Vector2 huongBan)
    {
        Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(huongBan * launchForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Prefab " + meteorPrefab.name + " thiếu Rigidbody2D!");
        }

        Shuriken shurikenScript = meteor.GetComponent<Shuriken>();
        if (shurikenScript != null && playerController != null)
        {
            shurikenScript.player = playerController;
            shurikenScript.isThienThach = true;
        }
    }


    // === HÀM TÌM KẺ ĐỊCH (3 TAG) ===
    Transform FindNearestEnemy()
    {
        List<GameObject> allTargets = new List<GameObject>();
        allTargets.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        allTargets.AddRange(GameObject.FindGameObjectsWithTag("Boss"));
        allTargets.AddRange(GameObject.FindGameObjectsWithTag("Monster"));

        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (GameObject target in allTargets)
        {
            if (target == null) continue;
            float distance = Vector3.Distance(target.transform.position, currentPosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = target;
            }
        }
        return (nearest != null) ? nearest.transform : null;
    }

    // === HÀM NÂNG CẤP ===
    public void NangCap()
    {
        soDem++;
        
        soThienThach += 1;
        
        // === LOGIC HỒI CHIÊU (ĐÃ SỬA) ===
        cooldown -= 0.2f; // Giảm thời gian hồi chiêu
        // Đảm bảo cooldown không bị âm
        if (cooldown < 0.5f) // Đặt một mức hồi chiêu tối thiểu
        {
            cooldown = 0.5f;
        }
    }

    public void NangCapCuoi()
    {
        isUpgradedCuoi = true;
        soThienThach = 10;
    }
}