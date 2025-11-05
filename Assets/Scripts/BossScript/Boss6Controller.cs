using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO "SIÊU BOSS" (Cùng với BossController)
public class Boss6Controller : MonoBehaviour, IBossAI
{
    #region // --- BIẾN CƠ BẢN ---
    [Header("Movement")]
    public float moveSpeed_P1 = 1.0f; // Tốc độ Phase 1
    public float moveSpeed_P2 = 2.0f; // Tốc độ Phase 2
    public float leftPoint = -3f;
    public float rightPoint = 3f;
    private bool movingRight = true;
    private bool isAttacking = false; // Cờ cấm di chuyển khi đang dùng skill

    [Header("Targeting")]
    public Transform playerTarget;
    public string playerTag = "Player";
    public PlayerController playerController;
    private SeraphMKII skillMKII;
    private Camera mainCamera;

    [Header("Balance")]
    public float slowDownFactor = 5f;

    [Header("AI Settings")]
    public float skillRestTime_P1 = 4.0f;
    public float skillRestTime_P2 = 2.5f;
    private bool isEnraged = false;
    private List<System.Func<IEnumerator>> skillList;

    [Header("Phase 2: Enrage")]
    public SpriteRenderer bossSpriteRenderer;
    public Color phase2Color = Color.red; // Màu khi nổi giận

    [Header("Tùy chọn: Con Mắt")]
    public Transform bossEye;
    #endregion

    #region // --- BIẾN SKILL MỚI ---
    [Header("Skill 1: Celestial Orbs (Quả Cầu Bám)")]
    // (SỬA ĐỔI) PREFAB 1 (Gắn script SuperBoss_TargetedOrb)
    public GameObject skill1_OrbPrefab;
    public int skill1_OrbCount_P1 = 6;
    public int skill1_OrbCount_P2 = 10;
    public float skill1_SpawnDelay = 0.1f;
    public float skill1_FireDelay = 0.5f; // Delay giữa mỗi lần bắn
    public float skill1_OrbitRadius = 2f; // Bán kính quỹ đạo quay
    public float skill1_OrbitSpeed = 90f; // Tốc độ quay (độ/giây)
    public float skill1_OrbitDuration = 4f; // Thời gian quay trước khi bắn

    [Header("Skill 2: Holy Ground (Đất Thánh Nổ)")]
    public GameObject skill2_GroundPrefab; // PREFAB 2 (Gắn script SuperBoss_HolyGround)
    public int skill2_ZoneCount_P1 = 3;
    public int skill2_ZoneCount_P2 = 5;
    public float skill2_SpawnDelay = 0.3f;

    [Header("Skill 3: Seraphic Rain (Mưa Lông Vũ)")]
    public GameObject skill3_RainBulletPrefab; // PREFAB 3 (Gắn script SuperBoss_RainBullet)
    public float skill3_Duration_P1 = 4f; // Mưa trong 4 giây
    public float skill3_Duration_P2 = 6f; // Mưa trong 6 giây
    public int skill3_BulletsPerWave = 10; // 10 viên mỗi đợt
    public float skill3_WaveDelay = 0.2f; // 0.2s bắn 1 đợt

    [Header("Skill 4 (Ultimate): Sanctuary (Thánh Địa Cấm)")]
    public GameObject skill4_DeathWavePrefab; // PREFAB 4 (Gắn script SuperBoss_DeathWave)
    public GameObject skill4_SafeZonePrefab; // PREFAB 5 (Gắn script SuperBoss_SafeZone)
    public int skill4_SafeZoneCount_P1 = 4;
    public int skill4_SafeZoneCount_P2 = 2; // Khó hơn
    public float skill4_ChargeTime = 3.0f; // Thời gian cho Player tìm chỗ núp
    public float skill4_WaveDuration = 7.0f; // Thời gian sóng tồn tại
    #endregion

    // --- HÀM TỪ INTERFACE ---
    public void Die()
    {
        StopAllCoroutines();
        this.enabled = false;
        Debug.Log("SIÊU BOSS AI ĐÃ DỪNG");
    }

    // (Hàm này được gọi bởi BossController.cs)
    public void ActivateEnrage()
    {
        if (isEnraged) return;
        isEnraged = true;
        Debug.Log("SIÊU BOSS ENRAGED!");

        moveSpeed_P1 = moveSpeed_P2; // Tăng tốc độ di chuyển

        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.color = phase2Color;
        }
    }

    void Start()
    {
        mainCamera = Camera.main;
        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();
            skillMKII = playerObject.GetComponent<SeraphMKII>();
        }

        skillList = new List<System.Func<IEnumerator>>()
        {
            Skill_CelestialOrbs,
            Skill_HolyGround,
            Skill_SeraphicRain,
            Skill_Sanctuary
        };

        StartCoroutine(BossAI_Pattern());
    }

    void Update()
    {
        if (playerTarget == null) { return; }
        LookAtPlayer();
        if (!isAttacking) { BossMove(); } // Chỉ di chuyển khi không đang dùng skill
    }

    #region // --- HÀM CƠ BẢN ---
    IEnumerator BossAI_Pattern()
    {
        yield return GetSlowedWait(3.0f);
        int lastSkillIndex = -1;
        while (true)
        {
            int currentSkillIndex;
            do
            {
                currentSkillIndex = Random.Range(0, skillList.Count);
            } while (skillList.Count > 1 && currentSkillIndex == lastSkillIndex);

            lastSkillIndex = currentSkillIndex;

            isAttacking = true; // Dừng di chuyển
            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());
            isAttacking = false; // Di chuyển lại

            float restTime = isEnraged ? skillRestTime_P2 : skillRestTime_P1;
            yield return GetSlowedWait(restTime);
        }
    }

    void LookAtPlayer()
    {
        if (bossEye != null && playerTarget != null)
        {
            Vector2 direction = (playerTarget.position - bossEye.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bossEye.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
    #endregion

    // --- CÁC SKILL ---

    // --- (SKILL 1 ĐÃ SỬA - QUAY QUANH BOSS) ---
    private IEnumerator Skill_CelestialOrbs()
    {
        Debug.Log("Boss Cuối: Dùng Skill Celestial Orbs (Quay quanh boss)");
        if (skill1_OrbPrefab == null) yield break;

        int orbCount = isEnraged ? skill1_OrbCount_P2 : skill1_OrbCount_P1;

        // Tạo danh sách để chứa các quả cầu và góc ban đầu
        List<GameObject> spawnedOrbs = new List<GameObject>();
        List<float> initialAngles = new List<float>();

        // --- Giai đoạn 1: Tạo quả cầu theo vòng tròn ---
        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * (360f / orbCount);
            initialAngles.Add(angle);

            Vector2 offset = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            ) * skill1_OrbitRadius;

            Vector2 spawnPos = (Vector2)transform.position + offset;

            // Spawn quả cầu KHÔNG gán làm con (để có thể tự do di chuyển)
            GameObject orbGO = Instantiate(skill1_OrbPrefab, spawnPos, Quaternion.identity);
            spawnedOrbs.Add(orbGO);

            yield return GetSlowedWait(skill1_SpawnDelay);
        }

        // --- Giai đoạn 2: Quay quanh boss ---
        Debug.Log("Các quả cầu bắt đầu quay quanh boss...");
        float orbitTime = 0f;

        while (orbitTime < skill1_OrbitDuration)
        {
            for (int i = 0; i < spawnedOrbs.Count; i++)
            {
                if (spawnedOrbs[i] != null)
                {
                    // Tính góc hiện tại (góc ban đầu + góc đã quay)
                    float currentAngle = initialAngles[i] + (skill1_OrbitSpeed * orbitTime);

                    // Tính vị trí mới dựa trên góc hiện tại
                    Vector2 offset = new Vector2(
                        Mathf.Cos(currentAngle * Mathf.Deg2Rad),
                        Mathf.Sin(currentAngle * Mathf.Deg2Rad)
                    ) * skill1_OrbitRadius;

                    // Cập nhật vị trí (theo boss)
                    spawnedOrbs[i].transform.position = (Vector2)transform.position + offset;
                }
            }

            orbitTime += Time.deltaTime;
            yield return null; // Chờ frame tiếp theo
        }

        // --- Giai đoạn 3: Bắn lần lượt ---
        Debug.Log("Bắt đầu bắn!");
        if (playerTarget == null) yield break;

        foreach (GameObject orbGO in spawnedOrbs)
        {
            if (orbGO != null)
            {
                SuperBoss_TargetedOrb orbScript = orbGO.GetComponent<SuperBoss_TargetedOrb>();
                if (orbScript != null)
                {
                    Vector2 targetPos = playerTarget.position;
                    orbScript.Launch(targetPos);
                }

                // Chờ 0.5s cho quả tiếp theo
                yield return GetSlowedWait(skill1_FireDelay);
            }
        }
    }
    // ----------------------

    private IEnumerator Skill_HolyGround()
    {
        Debug.Log("Boss Cuối: Dùng Skill Holy Ground");
        if (skill2_GroundPrefab == null) yield break;

        int zoneCount = isEnraged ? skill2_ZoneCount_P2 : skill2_ZoneCount_P1;

        for (int i = 0; i < zoneCount; i++)
        {
            Vector2 randomPos = GetRandomScreenPos();
            Instantiate(skill2_GroundPrefab, randomPos, Quaternion.identity);
            yield return GetSlowedWait(skill2_SpawnDelay);
        }
    }

    private IEnumerator Skill_SeraphicRain()
    {
        Debug.Log("Boss Cuối: Dùng Skill Seraphic Rain");
        if (skill3_RainBulletPrefab == null) yield break;

        float duration = isEnraged ? skill3_Duration_P2 : skill3_Duration_P1;
        float timer = 0f;

        // (Cho phép boss di chuyển trong khi mưa)
        isAttacking = false;

        while (timer < duration)
        {
            for (int i = 0; i < skill3_BulletsPerWave; i++)
            {
                Vector2 spawnPos = GetRandomTopTarget();
                Instantiate(skill3_RainBulletPrefab, spawnPos, Quaternion.identity);
            }

            yield return GetSlowedWait(skill3_WaveDelay);
            timer += skill3_WaveDelay;
        }

        isAttacking = true; // (Báo cho AI biết là skill đã xong)
    }

    private IEnumerator Skill_Sanctuary()
    {
        Debug.Log("Boss Cuối: Dùng Skill SANCTUARY");
        if (skill4_DeathWavePrefab == null || skill4_SafeZonePrefab == null) yield break;

        int zoneCount = isEnraged ? skill4_SafeZoneCount_P2 : skill4_SafeZoneCount_P1;
        List<GameObject> safeZones = new List<GameObject>();

        for (int i = 0; i < zoneCount; i++)
        {
            Vector2 randomPos = GetRandomScreenPos();
            GameObject zone = Instantiate(skill4_SafeZonePrefab, randomPos, Quaternion.identity);
            safeZones.Add(zone);
        }

        yield return GetSlowedWait(skill4_ChargeTime);

        Instantiate(skill4_DeathWavePrefab, transform.position, Quaternion.identity);

        yield return GetSlowedWait(skill4_WaveDuration);

        foreach (GameObject zone in safeZones)
        {
            if (zone != null) { Destroy(zone); }
        }
    }

    #region // --- HÀM HỖ TRỢ ---

    Vector2 GetRandomScreenPos()
    {
        if (mainCamera == null) return Vector2.zero;
        float z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 viewportPoint = new Vector3(Random.Range(0.1f, 0.9f), Random.Range(0.1f, 0.9f), z);
        return mainCamera.ViewportToWorldPoint(viewportPoint);
    }

    Vector2 GetRandomTopTarget()
    {
        if (mainCamera == null) return Vector2.zero;
        float z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 viewportPoint = new Vector3(Random.value, 1.1f, z);
        return mainCamera.ViewportToWorldPoint(viewportPoint);
    }

    private WaitForSeconds GetSlowedWait(float normalDuration)
    {
        float waitTime = normalDuration;
        if (skillMKII != null && skillMKII.lamchamthoigian)
        {
            waitTime *= slowDownFactor;
        }
        return new WaitForSeconds(waitTime);
    }

    public void BossMove()
    {
        float currentMoveSpeed = isEnraged ? moveSpeed_P2 : moveSpeed_P1;
        if (skillMKII != null && skillMKII.lamchamthoigian)
        {
            currentMoveSpeed /= slowDownFactor;
        }
        if (movingRight)
        {
            transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
            if (transform.position.x >= rightPoint) { movingRight = false; }
        }
        else
        {
            transform.Translate(Vector2.left * currentMoveSpeed * Time.deltaTime);
            if (transform.position.x <= leftPoint) { movingRight = true; }
        }
    }
    #endregion
}