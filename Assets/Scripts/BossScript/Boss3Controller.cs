using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO BOSS 3
public class Boss3Controller : MonoBehaviour, IBossAI
{
    [Header("Movement (Giống Boss 2)")]
    public float moveSpeed = 2f;
    public float leftPoint = -4f;
    public float rightPoint = 4f;
    private bool movingRight = true;

    [Tooltip("Cờ báo boss đang lao, để dừng BossMove()")]
    private bool isDashing = false;

    [Header("Targeting (Giống Boss 2)")]
    public Transform playerTarget;
    public string playerTag = "Player";
    public PlayerController playerController;
    private SeraphMKII skillMKII;
    private Camera mainCamera;

    [Header("Balance (Giống Boss 2)")]
    public float slowDownFactor = 5f;

    [Header("AI Settings (Giống Boss 2)")]
    public float skillRestTimePhase1 = 3.0f;
    public float skillRestTimePhase2 = 1.5f;

    [Header("Minion System")]
    public GameObject minionPrefab;
    public Transform[] minionSpawnPoints;

    // --- SỬA ĐỔI: Đã xóa [Tooltip] và làm cho mảng này private ---
    // Nó sẽ được gán tự động trong hàm Start()
    private Transform[] minionOrbitPoints;
    // --------------------------------------------------------

    public float initialMinionSpawnDelay = 5f;
    public float minionRespawnTime = 20f;
    private Boss3_Minion[] activeMinions;
    private float[] respawnTimers;

    [Header("Skill 1: Wing Scatter")]
    public GameObject scatterBulletPrefab;
    public Transform scatterShotSpawnPoint;
    public int scatterBulletCount = 9;
    public float scatterSpreadAngle = 60f;

    [Header("Skill 2: Minion Dash Attack")]
    // (Không cần biến)

    [Header("Skill 3: Venom Dash")]
    public GameObject venomTrailPrefab;
    public float dashSpeed = 15f;
    public float distancePerPuddle = 1.0f;

    [Tooltip("Thời gian hồi chiêu CỦA RIÊNG SKILL DASH (tính bằng giây)")]
    public float venomDashSkillCooldown = 30f;
    private bool isVenomDashOnCooldown = false;

    [Header("Collision")]
    [Tooltip("Sát thương khi Player va chạm vào Boss")]
    public float bossTouchDamage = 80f;

    [Header("Phase 2 (Enraged)")]
    private SpriteRenderer spriteRenderer;
    private bool isEnraged = false;
    public bool isMove = true;
    // --- (Hàm Die, ActivateEnrage giữ nguyên) ---
    public void Die()
    {
        StopAllCoroutines();
        if (activeMinions != null)
        {
            foreach (Boss3_Minion minion in activeMinions)
            {
                if (minion != null) minion.Die();
            }
        }
        this.enabled = false;
        Debug.Log("Boss 3 AI Đã Dừng");
    }
    public void ActivateEnrage()
    {
        if (isEnraged) return;
        isEnraged = true;
        Debug.Log("Boss 3 ENRAGED!");
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.magenta;
        }
        moveSpeed *= 1.5f;
    }


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;

        GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
            playerController = playerObject.GetComponent<PlayerController>();
            skillMKII = playerObject.GetComponent<SeraphMKII>();
        }

        // --- SỬA ĐỔI LỚN: TỰ ĐỘNG GÁN CÁC ĐIỂM LƠ LỬNG ---

        // 1. Tự động tìm 4 điểm lơ lửng
        if (!AutoAssignOrbitPoints())
        {
            // Nếu không tìm thấy 1 trong 4 điểm, vô hiệu hóa script AI
            Debug.LogError("Boss3Controller: Không tìm thấy 1 trong 4 'transformMinion' points! Hãy kiểm tra tên trong Scene. AI sẽ bị tắt.");
            this.enabled = false;
            return;
        }

        // 2. Khởi tạo hệ thống ong con (Logic này giờ đã an toàn)
        // (minionOrbitPoints đã được gán ở hàm trên)
        if (minionOrbitPoints != null && minionOrbitPoints.Length > 0)
        {
            int slotCount = minionOrbitPoints.Length; // slotCount sẽ là 4
            activeMinions = new Boss3_Minion[slotCount];
            respawnTimers = new float[slotCount];
            for (int i = 0; i < slotCount; i++)
            {
                respawnTimers[i] = initialMinionSpawnDelay;
            }
        }
        // -------------------------------------------------

        StartCoroutine(BossAI_Pattern());
    }

    /// <summary>
    /// (HÀM MỚI) Tự động tìm 4 GameObject tên là "transformMinionX"
    /// và gán vào mảng minionOrbitPoints.
    /// </summary>
    /// <returns>Trả về True nếu TẤT CẢ 4 điểm được tìm thấy</returns>
    private bool AutoAssignOrbitPoints()
    {
        // Khởi tạo mảng (cứng 4 vị trí)
        minionOrbitPoints = new Transform[4];
        bool allFound = true;

        for (int i = 0; i < 4; i++)
        {
            // Tạo tên, ví dụ: "transformMinion1", "transformMinion2", v.v.
            string objectName = "transformMinion" + (i + 1);

            GameObject foundObject = GameObject.Find(objectName);

            if (foundObject != null)
            {
                minionOrbitPoints[i] = foundObject.transform;
                Debug.Log("Boss3Controller: Đã tìm thấy và gán " + objectName);
            }
            else
            {
                Debug.LogError("Boss3Controller: KHÔNG TÌM THẤY GameObject tên là '" + objectName + "'!");
                allFound = false;
            }
        }

        return allFound;
    }


    void Update()
    {
        // (Giữ nguyên)
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
                playerController = playerObject.GetComponent<PlayerController>();
                skillMKII = playerObject.GetComponent<SeraphMKII>();
            }
            else { return; }
        }
        if (isMove == true)
        {
            transform.Translate(Vector2.down * 1f * Time.deltaTime);
            if (transform.position.y <= 3)
            {
                isMove = false;
            }
        }
        HandleMinionRespawns();
        if (!isDashing)
        {
            BossMove();
        }
    }

    IEnumerator BossAI_Pattern()
    {
        // (Giữ nguyên)
        yield return GetSlowedWait(3.0f);

        var skillList = new List<System.Func<IEnumerator>>()
        {
            Skill_WingScatter,
            Skill_MinionDashAttack,
            Skill_VenomDash
        };

        int lastSkillIndex = -1;
        while (true)
        {
            int currentSkillIndex;
            do
            {
                currentSkillIndex = Random.Range(0, skillList.Count);
                if (skillList[currentSkillIndex] == Skill_VenomDash && isVenomDashOnCooldown)
                {
                    currentSkillIndex = -1;
                }
            } while (skillList.Count > 1 && currentSkillIndex == lastSkillIndex || currentSkillIndex == -1);

            lastSkillIndex = currentSkillIndex;

            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());

            if (skillList[currentSkillIndex] == Skill_VenomDash)
            {
                StartCoroutine(VenomDashCooldown());
            }

            float restTime = isEnraged ? skillRestTimePhase2 : skillRestTimePhase1;
            yield return GetSlowedWait(restTime);
        }
    }

    // --- (Tất cả các hàm còn lại: HandleMinionRespawns, RespawnMinion,
    // Skill_WingScatter, Skill_MinionDashAttack, Skill_VenomDash,
    // VenomDashCooldown, FireSingleScatterBullet, GetSlowedWait, BossMove,
    // và OnTriggerEnter2D đều được GIỮ NGUYÊN) ---

    #region // --- CÁC HÀM GIỮ NGUYÊN ---

    void HandleMinionRespawns()
    {
        if (activeMinions == null) return;
        for (int i = 0; i < activeMinions.Length; i++)
        {
            if (activeMinions[i] != null)
            {
                respawnTimers[i] = -1f;
                continue;
            }
            if (respawnTimers[i] == -1f)
            {
                Debug.Log($"Ong con ở vị trí {i} đã chết. Bắt đầu đếm 20s.");
                respawnTimers[i] = minionRespawnTime;
            }
            if (respawnTimers[i] > 0)
            {
                respawnTimers[i] -= Time.deltaTime;
            }
            if (respawnTimers[i] <= 0 && respawnTimers[i] != -1f)
            {
                RespawnMinion(i);
                respawnTimers[i] = -1f;
            }
        }
    }
    void RespawnMinion(int slotIndex)
    {
        Transform orbitPoint = minionOrbitPoints[slotIndex];
        Transform spawnPoint = minionSpawnPoints[Random.Range(0, minionSpawnPoints.Length)];
        GameObject minionGO = Instantiate(minionPrefab, spawnPoint.position, spawnPoint.rotation);
        Boss3_Minion minionScript = minionGO.GetComponent<Boss3_Minion>();
        if (minionScript != null)
        {
            minionScript.Initialize(orbitPoint);
            activeMinions[slotIndex] = minionScript;
        }
    }

    private IEnumerator Skill_WingScatter()
    {
        Debug.Log("Boss 3: Dùng Skill Wing Scatter");
        yield return GetSlowedWait(0.5f);
        if (scatterShotSpawnPoint == null || playerTarget == null) { yield break; }
        Vector2 directionToPlayer = (playerTarget.position - scatterShotSpawnPoint.position).normalized;
        float baseAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg - 90f;
        float startAngle = baseAngle - scatterSpreadAngle / 2;
        float angleStep = scatterSpreadAngle / (scatterBulletCount - 1);
        for (int i = 0; i < scatterBulletCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            FireSingleScatterBullet(currentAngle);
        }
    }
    private IEnumerator Skill_MinionDashAttack()
    {
        Debug.Log("Boss 3: Dùng Skill Minion Dash Attack");
        if (playerTarget == null) yield break;
        bool hasMinionsToLaunch = false;
        foreach (var minion in activeMinions)
        {
            if (minion != null && minion.IsHovering())
            {
                hasMinionsToLaunch = true;
                break;
            }
        }
        if (!hasMinionsToLaunch)
        {
            Debug.Log("Không có ong con (rảnh) để tấn công!");
            yield return GetSlowedWait(1.0f);
            yield break;
        }
        yield return GetSlowedWait(0.5f);
        for (int i = 0; i < activeMinions.Length; i++)
        {
            if (activeMinions[i] != null && activeMinions[i].IsHovering())
            {
                Vector2 currentTargetPosition = playerTarget.position;
                activeMinions[i].LaunchAttack(currentTargetPosition);
                yield return GetSlowedWait(1.0f);
            }
        }
    }

    private IEnumerator Skill_VenomDash()
    {
        Debug.Log("Boss 3: Dùng Skill Venom Dash (Diagonal)");
        isDashing = true;

        if (mainCamera == null || playerTarget == null)
        {
            isDashing = false;
            yield break;
        }

        Vector2 returnPosition = transform.position;

        float z = Mathf.Abs(mainCamera.transform.position.z - playerTarget.position.z);
        Vector2 startPos, endPos;
        if (Random.value > 0.5f)
        {
            startPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, z)); // Top-Right
            endPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, z)); // Bottom-Left
        }
        else
        {
            startPos = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, z)); // Top-Left
            endPos = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, z)); // Bottom-Right
        }

        while (Vector2.Distance(transform.position, startPos) > 0.1f)
        {
            float step = moveSpeed * Time.deltaTime;
            if (skillMKII != null && skillMKII.lamchamthoigian) { step /= slowDownFactor; }
            transform.position = Vector2.MoveTowards(transform.position, startPos, step);
            yield return null;
        }

        yield return GetSlowedWait(0.5f);

        Vector2 lastPuddlePosition = transform.position;
        if (venomTrailPrefab != null)
            Instantiate(venomTrailPrefab, transform.position, Quaternion.identity);

        while (Vector2.Distance(transform.position, endPos) > 0.1f)
        {
            float step = dashSpeed * Time.deltaTime;
            if (skillMKII != null && skillMKII.lamchamthoigian) { step /= slowDownFactor; }
            transform.position = Vector2.MoveTowards(transform.position, endPos, step);

            if (Vector2.Distance(transform.position, lastPuddlePosition) >= distancePerPuddle)
            {
                if (venomTrailPrefab != null)
                {
                    Instantiate(venomTrailPrefab, transform.position, Quaternion.identity);
                }
                lastPuddlePosition = transform.position;
            }
            yield return null;
        }

        yield return GetSlowedWait(2.0f);

        while (Vector2.Distance(transform.position, returnPosition) > 0.1f)
        {
            float step = moveSpeed * 2 * Time.deltaTime; // Tăng tốc độ quay về
            if (skillMKII != null && skillMKII.lamchamthoigian) { step /= slowDownFactor; }
            transform.position = Vector2.MoveTowards(transform.position, returnPosition, step);
            yield return null;
        }

        isDashing = false;
    }

    private IEnumerator VenomDashCooldown()
    {
        isVenomDashOnCooldown = true;
        Debug.Log("Skill Venom Dash BẮT ĐẦU hồi chiêu " + venomDashSkillCooldown + " giây.");

        yield return new WaitForSeconds(venomDashSkillCooldown);

        isVenomDashOnCooldown = false;
        Debug.Log("Skill Venom Dash đã SẴN SÀNG.");
    }

    void FireSingleScatterBullet(float angle)
    {
        if (scatterBulletPrefab == null) return;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject bullet = Instantiate(scatterBulletPrefab, scatterShotSpawnPoint.position, rotation);
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
        float currentMoveSpeed = moveSpeed;
        if (skillMKII != null && skillMKII.lamchamthoigian)
        {
            currentMoveSpeed /= slowDownFactor;
        }
        if (movingRight)
        {
            transform.Translate(Vector2.right * currentMoveSpeed * Time.deltaTime);
            if (transform.position.x >= rightPoint)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * currentMoveSpeed * Time.deltaTime);
            if (transform.position.x <= leftPoint)
            {
                movingRight = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDame(bossTouchDamage);
            }
        }
    }

    #endregion
}