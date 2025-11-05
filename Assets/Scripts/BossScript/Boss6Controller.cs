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

    #region // --- BIẾN SKILL ---
    [Header("Skill 1: Celestial Orbs (Quả Cầu Bám)")]
    public GameObject skill1_OrbPrefab;
    public int skill1_OrbCount_P1 = 6;
    public int skill1_OrbCount_P2 = 10;
    public float skill1_SpawnDelay = 0.1f;
    public float skill1_FireDelay = 0.5f;
    public float skill1_OrbitRadius = 2f;
    public float skill1_OrbitSpeed = 90f;
    public float skill1_OrbitDuration = 4f;

    [Header("Skill 2: Holy Ground (Đất Thánh Nổ)")]
    public GameObject skill2_GroundPrefab;
    public int skill2_ZoneCount_P1 = 3;
    public int skill2_ZoneCount_P2 = 5;
    public float skill2_SpawnDelay = 0.3f;

    [Header("Skill 3: Seraphic Rain (Mưa Lông Vũ)")]
    public GameObject skill3_RainBulletPrefab;
    public float skill3_Duration_P1 = 4f;
    public float skill3_Duration_P2 = 6f;
    public int skill3_BulletsPerWave = 10;
    public float skill3_WaveDelay = 0.2f;

    // --- (SKILL 4 ĐÃ THAY THẾ) ---
    [Header("Skill 4 (Ultimate): Orbital Bombardment")]
    [Tooltip("PREFAB 4 (Gắn script SuperBoss_TargetedGround)")]
    public GameObject skill4_BombardmentPrefab;
    public int skill4_BombCount_P1 = 3; // Thả 3 quả bom
    public int skill4_BombCount_P2 = 5; // Thả 5 quả bom
    public float skill4_BombDelay = 1.0f; // Thời gian nghỉ giữa mỗi quả bom
    public float skill4_FollowTime_P1 = 3f; // Thời gian bám theo P1
    public float skill4_FollowTime_P2 = 2f; // Thời gian bám theo P2 (khó hơn)
    // ----------------------------
    #endregion

    // --- HÀM TỪ INTERFACE ---
    public void Die()
    {
        StopAllCoroutines();
        this.enabled = false;
        Debug.Log("SIÊU BOSS AI ĐÃ DỪNG");
    }

    public void ActivateEnrage()
    {
        if (isEnraged) return;
        isEnraged = true;
        Debug.Log("SIÊU BOSS ENRAGED!");
        moveSpeed_P1 = moveSpeed_P2;
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.color = phase2Color;
        }
    }

    // --- (HÀM START ĐÃ SỬA) ---
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
            Skill_OrbitalBombardment // <<< (ĐÃ THAY THẾ)
        };

        StartCoroutine(BossAI_Pattern());
    }

    void Update()
    {
        if (playerTarget == null) { return; }
        LookAtPlayer();
        if (!isAttacking) { BossMove(); }
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

            isAttacking = true;
            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());
            isAttacking = false;

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

    // (Skill 1: Celestial Orbs - Giữ nguyên)
    private IEnumerator Skill_CelestialOrbs()
    {
        Debug.Log("Boss Cuối: Dùng Skill Celestial Orbs (Quay quanh boss)");
        if (skill1_OrbPrefab == null) yield break;
        int orbCount = isEnraged ? skill1_OrbCount_P2 : skill1_OrbCount_P1;
        List<GameObject> spawnedOrbs = new List<GameObject>();
        List<float> initialAngles = new List<float>();

        for (int i = 0; i < orbCount; i++)
        {
            float angle = i * (360f / orbCount);
            initialAngles.Add(angle);
            Vector2 offset = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * skill1_OrbitRadius;
            Vector2 spawnPos = (Vector2)transform.position + offset;
            GameObject orbGO = Instantiate(skill1_OrbPrefab, spawnPos, Quaternion.identity);
            spawnedOrbs.Add(orbGO);
            yield return GetSlowedWait(skill1_SpawnDelay);
        }

        Debug.Log("Các quả cầu bắt đầu quay quanh boss...");
        float orbitTime = 0f;
        while (orbitTime < skill1_OrbitDuration)
        {
            for (int i = 0; i < spawnedOrbs.Count; i++)
            {
                if (spawnedOrbs[i] != null)
                {
                    float currentAngle = initialAngles[i] + (skill1_OrbitSpeed * orbitTime);
                    Vector2 offset = new Vector2(Mathf.Cos(currentAngle * Mathf.Deg2Rad), Mathf.Sin(currentAngle * Mathf.Deg2Rad)) * skill1_OrbitRadius;
                    spawnedOrbs[i].transform.position = (Vector2)transform.position + offset;
                }
            }
            orbitTime += Time.deltaTime; // (SỬA LỖI: Dùng Time.deltaTime)
            yield return null;
        }

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
                yield return GetSlowedWait(skill1_FireDelay);
            }
        }
    }

    // (Skill 2: Holy Ground - Giữ nguyên)
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

    // (Skill 3: Seraphic Rain - Giữ nguyên)
    private IEnumerator Skill_SeraphicRain()
    {
        Debug.Log("Boss Cuối: Dùng Skill Seraphic Rain");
        if (skill3_RainBulletPrefab == null) yield break;
        float duration = isEnraged ? skill3_Duration_P2 : skill3_Duration_P1;
        float timer = 0f;
        isAttacking = false;
        while (timer < duration)
        {
            for (int i = 0; i < skill3_BulletsPerWave; i++)
            {
                Vector2 spawnPos = GetRandomTopTarget();
                Instantiate(skill3_RainBulletPrefab, spawnPos, Quaternion.Euler(0, 0, -65f)); // (Giữ nguyên góc -65f của bạn)
            }
            yield return GetSlowedWait(skill3_WaveDelay);
            timer += skill3_WaveDelay;
        }
        isAttacking = true;
    }

    // --- (SKILL 4 ĐÃ THAY THẾ) ---
    private IEnumerator Skill_OrbitalBombardment()
    {
        Debug.Log("Boss Cuối: Dùng Skill Orbital Bombardment");
        if (skill4_BombardmentPrefab == null || playerTarget == null) yield break;

        int bombCount = isEnraged ? skill4_BombCount_P2 : skill4_BombCount_P1;
        float followTime = isEnraged ? skill4_FollowTime_P2 : skill4_FollowTime_P1;

        // Cho phép boss di chuyển trong khi thả bom
        isAttacking = false;

        for (int i = 0; i < bombCount; i++)
        {
            // Spawn quả bom ngay trên đầu boss
            Vector2 spawnPos = (Vector2)transform.position + new Vector2(0, 3f);

            GameObject bombGO = Instantiate(skill4_BombardmentPrefab, spawnPos, Quaternion.identity);

            // Lấy script và ra lệnh cho nó bám theo
            SuperBoss_TargetedGround bombScript = bombGO.GetComponent<SuperBoss_TargetedGround>();
            if (bombScript != null)
            {
                bombScript.Activate(playerTarget, followTime);
            }

            // Chờ trước khi thả quả tiếp theo
            yield return GetSlowedWait(skill4_BombDelay);
        }

        // Chờ thêm một chút để quả bom cuối cùng nổ (3s bám + 2s cảnh báo)
        yield return GetSlowedWait(followTime + 2.0f);

        isAttacking = true; // Báo là skill đã xong
    }

    #region // --- HÀM HỖ TRỢ (GIỮ NGUYÊN) ---
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