using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO BOSS 6 (Cùng với BossController)
public class Boss5Controller : MonoBehaviour, IBossAI
{
    #region // --- BIẾN ---
    [Header("Movement")]
    public float moveSpeed = 1.5f;
    public float leftPoint = -3f;
    public float rightPoint = 3f;
    private bool movingRight = true;
    private bool isDashing = false;

    [Header("Targeting")]
    public Transform playerTarget;
    public string playerTag = "Player";
    public PlayerController playerController;
    private SeraphMKII skillMKII;
    private Camera mainCamera;

    [Header("Balance")]
    public float slowDownFactor = 5f;

    [Header("AI Settings")]
    public float skillRestTimePhase1 = 3.0f;
    public float skillRestTimePhase2 = 2.0f;
    private bool isEnraged = false;
    private bool isEnrageTriggered = false; // Cờ để tránh trigger nhiều lần
    private List<System.Func<IEnumerator>> skillList;

    [Header("Nội tại: Pod (Đứng yên)")]
    public Transform podContainer;

    [Header("Phase 2: Phân Thân (Clone)")]
    public SpriteRenderer bossSpriteRenderer;
    public GameObject clonePrefab;
    private Transform[] phase2Positions = new Transform[2];
    private Boss5_Illusion myCloneInstance;

    [Header("Skill 1: Random Burst (Bắn ngẫu nhiên)")]
    public GameObject skill1_BulletPrefab;
    public Transform[] podFirePoints;
    public int skill1_BurstCount = 3;
    public int skill1_BurstCount_P2 = 5;
    public float skill1_BurstDelay = 0.1f;

    [Header("Skill 3: Random Barrage (Bắn ngẫu nhiên)")]
    public GameObject skill3_BulletPrefab;
    public int skill3_ShotCount = 20;
    public int skill3_ShotCount_P2 = 40;
    public float skill3_SpinDelay = 0.1f;

    [Header("Skill 4: Void Dash (Lao Chữ X)")]
    public GameObject skill4_TrailPrefab;
    public float dashSpeed = 20f;
    public float distancePerTrail = 0.5f;

    [Header("Skill 5: Dark Matter Eruption")]
    public GameObject skill5_DarkMatterPrefab;
    public int skill5_EruptionCount = 12;
    public int skill5_EruptionCount_P2 = 20;
    public float skill5_EruptionRadius = 1.0f;
    public float skill5_EruptionForce = 500f;
    public float skill5_PrepareTime = 1.0f;
    public float skill5_ActiveTime = 2.0f;

    [Header("Tùy chọn: Con Mắt")]
    public Transform bossEye;
    #endregion

    public void Die()
    {
        StopAllCoroutines();
        this.enabled = false;
        Debug.Log("Boss 6 AI Đã Dừng");
        if (myCloneInstance != null)
        {
            myCloneInstance.Vanish();
        }
    }

    // --- HÀM NÀY ĐƯỢC GỌI TỪ BossController KHI HP <= 50% ---
    public void ActivateEnrage()
    {
        if (isEnrageTriggered) return;
        isEnrageTriggered = true; // Đánh dấu đã trigger

        Debug.Log("Boss 6 ENRAGED! Dừng tất cả skill và bay đến vị trí phân thân!");

        // DỪNG TẤT CẢ COROUTINE (Skill đang chạy)
        StopAllCoroutines();

        // Bắt đầu sequence phân thân
        StartCoroutine(EnrageSequence());
    }

    private IEnumerator EnrageSequence()
    {
        // Đánh dấu đang trong trạng thái đặc biệt
        isDashing = true;

        // Kiểm tra vị trí Phase 2
        if (clonePrefab == null || phase2Positions[0] == null || phase2Positions[1] == null)
        {
            Debug.LogError("Boss 6: Không tìm thấy 'ViTri' hoặc 'ViTri2'. Không thể bắt đầu Phase 2!");
            isDashing = false;
            yield break;
        }

        // Đổi màu vàng
        if (bossSpriteRenderer != null)
        {
            bossSpriteRenderer.color = Color.yellow;
        }

        // 1. Bay nhanh đến vị trí Phase 2 (góc trái)
        float fastMoveSpeed = 8f; // Tốc độ bay nhanh
        while (Vector2.Distance(transform.position, phase2Positions[0].position) > 0.1f)
        {
            float step = fastMoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, phase2Positions[0].position, step);
            yield return null;
        }

        // 2. Spawn Clone
        GameObject cloneGO = Instantiate(clonePrefab, transform.position, Quaternion.identity);
        myCloneInstance = cloneGO.GetComponent<Boss5_Illusion>();

        yield return null; // Đợi 1 frame

        if (myCloneInstance != null)
        {
            // 3. Ra lệnh cho Clone bay đến góc phải
            myCloneInstance.Activate(playerTarget, phase2Positions[1], skillMKII);

            // 4. Sao chép máu
            BossController mainHealth = this.GetComponent<BossController>();
            BossController cloneHealth = cloneGO.GetComponent<BossController>();

            if (mainHealth != null && cloneHealth != null)
            {
                Debug.Log($"Sao chép máu: {mainHealth.currentHealth} / {mainHealth.maxHealth}");
                cloneHealth.currentHealth = mainHealth.currentHealth;
                cloneHealth.maxHealth = mainHealth.maxHealth;

                if (cloneHealth.thanhMau != null)
                {
                    cloneHealth.thanhMau.capnhatthanhmau(cloneHealth.currentHealth, cloneHealth.maxHealth);
                }
            }
        }

        // 5. Đợi Clone bay đến vị trí (khoảng 1-2 giây)
        yield return new WaitForSeconds(1.5f);

        // 6. HOÀN TẤT PHASE 2 - Cập nhật trạng thái
        isEnraged = true;
        skillRestTimePhase1 = skillRestTimePhase2;
        isDashing = false;

        Debug.Log("Boss 6: Phân thân hoàn tất! Tiếp tục dùng skill Phase 2!");

        // 7. KHỞI ĐỘNG LẠI AI PATTERN (Bắt đầu dùng skill Phase 2)
        StartCoroutine(BossAI_Pattern());
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

        // Tự động tìm vị trí
        GameObject viTri1 = GameObject.Find("ViTri");
        GameObject viTri2 = GameObject.Find("ViTri2");
        if (viTri1 != null) { phase2Positions[0] = viTri1.transform; }
        else { Debug.LogError("Boss 6: Không tìm thấy GameObject tên 'ViTri'!"); }
        if (viTri2 != null) { phase2Positions[1] = viTri2.transform; }
        else { Debug.LogError("Boss 6: Không tìm thấy GameObject tên 'ViTri2'!"); }

        skillList = new List<System.Func<IEnumerator>>()
        {
            Skill_FocusCannons,
            Skill_SpiralBarrage,
            Skill_VoidDash,
            Skill_DarkMatterEruption
        };

        StartCoroutine(BossAI_Pattern());
    }

    #region // --- HÀM CƠ BẢN ---
    void Update()
    {
        if (playerTarget == null) { return; }
        LookAtPlayer();
        if (!isDashing && !isEnraged) { BossMove(); }
    }

    IEnumerator BossAI_Pattern()
    {
        yield return GetSlowedWait(3.0f);
        int lastSkillIndex = -1;

        while (true)
        {
            // Kiểm tra nếu đang trong quá trình Enrage thì dừng AI tạm thời
            if (isEnrageTriggered && !isEnraged)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            int currentSkillIndex;
            do
            {
                currentSkillIndex = Random.Range(0, skillList.Count);
            } while (skillList.Count > 1 && currentSkillIndex == lastSkillIndex);

            lastSkillIndex = currentSkillIndex;

            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());

            float restTime = isEnraged ? skillRestTimePhase2 : skillRestTimePhase1;
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

    #region // --- SKILLS ---
    private IEnumerator Skill_VoidDash()
    {
        Debug.Log("Boss 6: Dùng Skill Void Dash (X Shape)");
        if (skill4_TrailPrefab == null || mainCamera == null || playerTarget == null) yield break;

        isDashing = true;
        float currentMoveSpeed = 15f; // Tăng tốc độ bay giữa các điểm (8f -> 15f)
        float phase1ReturnY = transform.position.y;

        float z = Mathf.Abs(mainCamera.transform.position.z - playerTarget.position.z);
        Vector2 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, z));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, z));
        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, z));
        Vector2 bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, z));

        Vector2 startPos1, endPos1, startPos2, endPos2;
        if (Random.value > 0.5f) { startPos1 = topLeft; endPos1 = bottomRight; startPos2 = topRight; endPos2 = bottomLeft; }
        else { startPos1 = topRight; endPos1 = bottomLeft; startPos2 = topLeft; endPos2 = bottomRight; }

        if (myCloneInstance != null)
        {
            myCloneInstance.Start_Skill_VoidDash(startPos1, endPos1, startPos2, endPos2, currentMoveSpeed, dashSpeed, distancePerTrail, skill4_TrailPrefab);
        }

        while (Vector2.Distance(transform.position, startPos1) > 0.1f) { float step = currentMoveSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, startPos1, step); yield return null; }
        yield return GetSlowedWait(0.5f);
        Vector2 lastTrailPos = transform.position;
        while (Vector2.Distance(transform.position, endPos1) > 0.1f) { float step = dashSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, endPos1, step); if (Vector2.Distance(transform.position, lastTrailPos) >= distancePerTrail) { Instantiate(skill4_TrailPrefab, transform.position, Quaternion.identity); lastTrailPos = transform.position; } yield return null; }
        while (Vector2.Distance(transform.position, startPos2) > 0.1f) { float step = currentMoveSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, startPos2, step); yield return null; }
        yield return GetSlowedWait(0.5f);
        lastTrailPos = transform.position;
        while (Vector2.Distance(transform.position, endPos2) > 0.1f) { float step = dashSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, endPos2, step); if (Vector2.Distance(transform.position, lastTrailPos) >= distancePerTrail) { Instantiate(skill4_TrailPrefab, transform.position, Quaternion.identity); lastTrailPos = transform.position; } yield return null; }
        yield return GetSlowedWait(1.0f);

        Vector2 returnPosition;
        if (isEnraged && phase2Positions[0] != null)
        {
            returnPosition = phase2Positions[0].position;
        }
        else
        {
            returnPosition = new Vector2((leftPoint + rightPoint) / 2, phase1ReturnY);
        }

        while (Vector2.Distance(transform.position, returnPosition) > 0.1f)
        {
            float step = currentMoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, returnPosition, step);
            yield return null;
        }

        isDashing = false;
    }

    private IEnumerator Skill_FocusCannons()
    {
        Debug.Log("Boss 6: Dùng Skill Random Burst");
        if (skill1_BulletPrefab == null || podFirePoints.Length < 2 || playerTarget == null) yield break;
        int currentBurstCount = isEnraged ? skill1_BurstCount_P2 : skill1_BurstCount;
        if (myCloneInstance != null) { myCloneInstance.Start_Skill_FocusCannons(currentBurstCount, skill1_BurstDelay); }
        for (int i = 0; i < currentBurstCount; i++)
        {
            Vector2 targetPos1 = GetRandomTargetViewport(); FireBulletAtTarget(podFirePoints[0], targetPos1, skill1_BulletPrefab);
            Vector2 targetPos2 = GetRandomTargetViewport(); FireBulletAtTarget(podFirePoints[1], targetPos2, skill1_BulletPrefab);
            yield return GetSlowedWait(skill1_BurstDelay);
        }
    }

    private IEnumerator Skill_SpiralBarrage()
    {
        Debug.Log("Boss 6: Dùng Skill Random Barrage");
        if (skill3_BulletPrefab == null || podFirePoints.Length < 2) yield break;
        int currentShotCount = isEnraged ? skill3_ShotCount_P2 : skill3_ShotCount;
        if (myCloneInstance != null) { myCloneInstance.Start_Skill_SpiralBarrage(currentShotCount, skill3_SpinDelay); }
        for (int i = 0; i < currentShotCount; i++)
        {
            int podIndex = i % 2;
            Vector2 targetPos = GetRandomTargetViewport(); FireBulletAtTarget(podFirePoints[podIndex], targetPos, skill3_BulletPrefab);
            yield return GetSlowedWait(skill3_SpinDelay);
        }
    }

    private IEnumerator Skill_DarkMatterEruption()
    {
        Debug.Log("Boss 6: Dùng Skill Dark Matter Eruption");
        if (skill5_DarkMatterPrefab == null) yield break;
        int currentEruptionCount = isEnraged ? skill5_EruptionCount_P2 : skill5_EruptionCount;
        if (myCloneInstance != null) { myCloneInstance.Start_Skill_DarkMatterEruption(currentEruptionCount, skill5_EruptionRadius, skill5_EruptionForce, skill5_PrepareTime, skill5_ActiveTime); }
        yield return GetSlowedWait(skill5_PrepareTime);
        List<GameObject> spawnedMatter = new List<GameObject>();
        for (int i = 0; i < currentEruptionCount; i++)
        {
            float angle = i * (360f / currentEruptionCount);
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector2 spawnPosition = (Vector2)transform.position + (Vector2)(rotation * Vector2.up) * skill5_EruptionRadius;
            GameObject matter = Instantiate(skill5_DarkMatterPrefab, spawnPosition, rotation);
            spawnedMatter.Add(matter);
            Rigidbody2D rb = matter.GetComponent<Rigidbody2D>();
            if (rb != null) { rb.AddForce(matter.transform.up * skill5_EruptionForce, ForceMode2D.Impulse); }
        }
        yield return GetSlowedWait(skill5_ActiveTime);
        foreach (GameObject matter in spawnedMatter) { if (matter != null) { Destroy(matter); } }
        spawnedMatter.Clear();
    }
    #endregion

    #region // --- HÀM HỖ TRỢ ---
    IEnumerator MoveToPosition(Vector2 targetPos)
    {
        float currentMoveSpeed = (moveSpeed == 0) ? 3f : moveSpeed * 2f;
        while (Vector2.Distance(transform.position, targetPos) > 0.1f)
        {
            float step = currentMoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPos, step);
            yield return null;
        }
    }

    void FireBullet(Transform firePoint, GameObject prefab)
    {
        if (firePoint == null || prefab == null) return;
        Instantiate(prefab, firePoint.position, firePoint.rotation);
    }

    void FireBulletAtTarget(Transform firePoint, Vector2 targetPosition, GameObject prefab)
    {
        if (firePoint == null || prefab == null) return;
        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
        Instantiate(prefab, firePoint.position, bulletRotation);
    }

    Vector2 GetRandomTargetViewport()
    {
        if (mainCamera == null || playerTarget == null) return new Vector2(0, -10);
        float z = Mathf.Abs(mainCamera.transform.position.z - playerTarget.position.z);
        Vector3 viewportPoint = new Vector3();
        int edge = Random.Range(0, 3);
        if (edge == 0) { viewportPoint = new Vector3(0, Random.value, z); }
        else if (edge == 1) { viewportPoint = new Vector3(1, Random.value, z); }
        else { viewportPoint = new Vector3(Random.value, 0, z); }
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
        float currentMoveSpeed = moveSpeed;
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().TakeDame(80f);
        }
    }
    #endregion
}