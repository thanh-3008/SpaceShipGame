using System.Collections;
using System.Collections.Generic; // Cần cho List
using UnityEngine;

// GẮN SCRIPT NÀY VÀO BOSS 2
public class BossWardenGoliath2D : MonoBehaviour, IBossAI
{
    // (Các biến từ Movement đến Skill 2 giữ nguyên)
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float leftPoint = -4f;
    public float rightPoint = 4f;
    private bool movingRight = true;
    public bool isMove = true;

    [Header("Targeting")]
    public Transform playerTarget;
    public string playerTag = "Player";
    public PlayerController playerController;
    private SeraphMKII skillMKII;
    private Camera mainCamera;

    [Header("Balance (From Boss 1)")]
    public float slowDownFactor = 5f;

    [Header("AI Settings (Quan trọng)")]
    [Tooltip("Thời gian nghỉ giữa các skill (GĐ 1)")]
    public float skillRestTimePhase1 = 3.0f;
    [Tooltip("Thời gian nghỉ giữa các skill (GĐ 2)")]
    public float skillRestTimePhase2 = 1.5f;

    [Header("Skill 1: Homing Missile")]
    public GameObject missilePrefab2D;
    public Transform[] firePoints; // Dùng chung cho cả 3 skill
    public float missileLifetime = 7.0f;
    [Tooltip("Tốc độ của tên lửa rượt đuổi")]
    public float missileSpeed = 10.0f;
    public float missileTurnSpeed = 120.0f;
    [Tooltip("Khoảng nghỉ giữa 5 đợt bắn rượt đuổi")]
    public float homingWaveDelay = 0.8f;

    [Header("Skill 2: Missile Barrage")]
    [Tooltip("Tốc độ của tên lửa bắn thẳng")]
    public float barrageMissileSpeed = 15f;
    [Tooltip("Số lượng tên lửa mỗi tay (GĐ 1)")]
    public int barrageAmountPhase1 = 10;
    [Tooltip("Số lượng tên lửa mỗi tay (GĐ 2)")]
    public int barrageAmountPhase2 = 20;
    [Tooltip("Khoảng nghỉ giữa mỗi loạt bắn (giây)")]
    public float barrageMissileDelay = 0.3f;

    // --- SKILL 3 ĐÃ SỬA ---
    [Header("Skill 3: Flak Shells")]
    [Tooltip("Prefab của đạn pháo (sẽ tự nổ)")]
    public GameObject flakShellPrefab;
    [Tooltip("Tốc độ của đạn pháo (nên bay chậm)")]
    public float flakShellSpeed = 3f;

    // --- SỬA ĐỔI (Req 3): Danh sách theo dõi đạn con ---
    private List<GameObject> activeFlakBullets = new List<GameObject>();
    // ----------------------------------------------------

    [Header("Phase 2 (Enraged)")]
    private SpriteRenderer spriteRenderer;

    [Header("Slow Spread Attack (Phase 2)")]
    public GameObject slowBulletPrefab;
    public Transform slowSpreadFirePoint;
    public float slowBulletSpeed = 5f;
    public float slowBulletCount = 12f;
    public float slowBulletAngle = 170f;

    private bool isEnraged = false; // Cờ Giai đoạn 2

    // --- HÀM TỪ INTERFACE ---
    public void Die()
    {
        StopAllCoroutines();
        this.enabled = false;
        Debug.Log("Boss 2 AI Đã Dừng");
    }
    // -----------------------

    // (Hàm ActivateEnrage, Start, Update, BossAI_Pattern,
    // Skill_HomingShot, Skill_MissileBarrage giữ nguyên
    // ...
    public void ActivateEnrage()
    {
        if (isEnraged) return;
        isEnraged = true;
        Debug.Log("Boss 2 ENRAGED!");

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        // Bắn đạn làm chậm 1 lần
        FireSlowSpread();
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

        // Bắt đầu AI chính (chỉ 1 vòng lặp)
        StartCoroutine(BossAI_Pattern());
    }

    void Update()
    {
        // (Hàm Update giữ nguyên... di chuyển và tìm player)
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
                playerController = playerObject.GetComponent<PlayerController>();
                skillMKII = playerObject.GetComponent<SeraphMKII>();
            }
            else
            {
                return;
            }
        }
        if (isMove == true)
        {
            transform.Translate(Vector2.down * 1f * Time.deltaTime);
            if (transform.position.y <= 3)
            {
                isMove = false;
            }
        }
        BossMove();
    }

    IEnumerator BossAI_Pattern()
    {
        yield return GetSlowedWait(3.0f);

        // --- SỬA ĐỔI: Thêm Skill 3 vào danh sách từ đầu
        var skillList = new List<System.Func<IEnumerator>>()
        {
            Skill_HomingShot,
            Skill_MissileBarrage,
            Skill_FlakShells      // <<< SKILL 3
        };

        int lastSkillIndex = -1;

        while (true) // Lặp vô hạn
        {
            int currentSkillIndex;
            do
            {
                currentSkillIndex = Random.Range(0, skillList.Count);
            } while (skillList.Count > 1 && currentSkillIndex == lastSkillIndex);

            lastSkillIndex = currentSkillIndex;

            // Chạy và chờ skill xong
            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());

            // Nghỉ ngơi (thời gian nghỉ dựa trên Giai đoạn)
            float restTime = isEnraged ? skillRestTimePhase2 : skillRestTimePhase1;
            yield return GetSlowedWait(restTime);
        }
    }

    private IEnumerator Skill_HomingShot()
    {
        Debug.Log("Boss 2: Dùng Skill Homing Shot (5 đợt)");
        for (int i = 0; i < 5; i++)
        {
            FireHomingMissile_Internal();
            yield return GetSlowedWait(homingWaveDelay);
        }
    }

    private IEnumerator Skill_MissileBarrage()
    {
        Debug.Log("Boss 2: Dùng Skill Missile Barrage");
        // Số lượng đạn dựa trên Giai đoạn
        int missileCount = isEnraged ? barrageAmountPhase2 : barrageAmountPhase1;

        for (int i = 0; i < missileCount; i++)
        {
            // Bắn từ TẤT CẢ các điểm bắn (theo yêu cầu là 2)
            foreach (Transform point in firePoints)
            {
                if (point == null) continue;
                FireSingleBarrageMissile(point);
            }
            yield return GetSlowedWait(barrageMissileDelay);
        }
    }

    // --- SỬA ĐỔI TOÀN BỘ HÀM NÀY (Req 3) ---
    /// <summary>
    /// SKILL 3 (ĐÃ SỬA): Bắn đạn pháo VÀ CHỜ TẤT CẢ ĐẠN CON BIẾN MẤT
    /// </summary>
    private IEnumerator Skill_FlakShells()
    {
        Debug.Log("Boss 2: Dùng Skill Flak Shells");

        // 1. Dọn dẹp danh sách, loại bỏ các đạn (null) từ đợt trước (nếu có)
        activeFlakBullets.RemoveAll(item => item == null);

        // 2. Bắn đạn pháo (FireSingleFlakShell sẽ thêm đạn con vào danh sách)
        foreach (Transform point in firePoints)
        {
            if (point == null) continue;
            FireSingleFlakShell(point);
        }

        // 3. Chờ cho đạn pháo có thời gian bay và NỔ
        // Phải chờ, nếu không vòng lặp while bên dưới sẽ chạy
        // và kết thúc ngay lập tức (vì đạn chưa kịp nổ để thêm vào list)
        float timeToWait = 3.0f; // Mặc định (2.5s nổ + 0.5s dự phòng)
        if (flakShellPrefab != null)
        {
            FlakShell prefabScript = flakShellPrefab.GetComponent<FlakShell>();
            if (prefabScript != null)
            {
                timeToWait = prefabScript.timeToExplode + 0.5f;
            }
        }
        yield return GetSlowedWait(timeToWait);

        // 4. Bắt đầu vòng lặp: Chờ cho đến khi tất cả đạn con bị phá hủy
        // (Bị phá hủy do va chạm, hoặc do bay ra khỏi camera)
        while (activeFlakBullets.Count > 0)
        {
            // Dọn dẹp danh sách liên tục
            activeFlakBullets.RemoveAll(item => item == null);

            if (activeFlakBullets.Count == 0)
            {
                break; // Thoát vòng lặp
            }

            // Chờ 1 chút (bị ảnh hưởng bởi slow-down) rồi kiểm tra lại
            yield return GetSlowedWait(0.1f);
        }

        Debug.Log("[Skill_FlakShells] Đã dọn dẹp hết đạn. Tiếp tục AI.");
    }
    // ---------------------------------------------


    // (Hàm FireHomingMissile_Internal, FireSingleBarrageMissile giữ nguyên)
    // ...
    void FireHomingMissile_Internal()
    {
        if (missilePrefab2D == null || firePoints == null || firePoints.Length == 0) return;
        if (playerTarget == null) return;
        foreach (Transform point in firePoints)
        {
            if (point == null) continue;
            Vector2 directionToPlayer = (playerTarget.position - point.position).normalized;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion initialRotation = Quaternion.Euler(0f, 0f, angle);
            GameObject missileGO = Instantiate(missilePrefab2D, point.position, initialRotation);
            HomingMissile2D missileScript = missileGO.GetComponent<HomingMissile2D>();
            if (missileScript != null)
            {
                missileScript.SetTarget(playerTarget, missileLifetime, missileSpeed, missileTurnSpeed);
            }
            else
            {
                Debug.LogError("Missile Prefab 2D không chứa script HomingMissile2D!");
            }
        }
    }

    void FireSingleBarrageMissile(Transform firePoint)
    {
        if (missilePrefab2D == null) return;
        Vector2 targetPos = GetRandomBottomTarget();
        Vector2 direction = (targetPos - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);
        GameObject missileGO = Instantiate(missilePrefab2D, firePoint.position, rotation);

        HomingMissile2D missileScript = missileGO.GetComponent<HomingMissile2D>();
        if (missileScript != null)
        {
            missileScript.SetBarrage(missileLifetime);
        }
        else
        {
            Debug.LogError("Missile Prefab 2D không chứa script HomingMissile2D!");
            Destroy(missileGO, 5f);
        }

        Rigidbody2D rb = missileGO.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.isKinematic)
            {
                rb.isKinematic = false;
            }
            rb.linearVelocity = direction * barrageMissileSpeed;
        }
        else
        {
            Debug.LogWarning("Missile Barrage: missilePrefab2D thiếu Rigidbody2D!");
        }
    }

    // --- SỬA ĐỔI HÀM NÀY (Req 3) ---
    /// <summary>
    /// (Hàm mới) Bắn 1 quả đạn pháo (Flak Shell) - ĐÃ SỬA
    /// </summary>
    void FireSingleFlakShell(Transform firePoint)
    {
        if (flakShellPrefab == null || playerTarget == null) return;

        // 1. Tạo 1 vị trí ngẫu nhiên gần người chơi
        Vector2 targetPos = (Vector2)playerTarget.position + (Random.insideUnitCircle * 5.0f); // Bắn vào 1 vòng tròn bán kính 3f quanh player

        // 2. Tính hướng
        Vector2 direction = (targetPos - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

        // 3. Tạo đạn pháo
        GameObject shellGO = Instantiate(flakShellPrefab, firePoint.position, rotation);

        // --- SỬA ĐỔI: GỌI HÀM INITIALIZE VÀ GÁN TRACKING LIST ---
        // 4. Lấy script và báo cho nó biết đang ở Giai đoạn nào
        FlakShell shellScript = shellGO.GetComponent<FlakShell>();
        if (shellScript != null)
        {
            // Báo cho đạn pháo biết nó nên nổ 4 hay 8 viên
            shellScript.Initialize(isEnraged);

            // Gán danh sách theo dõi của Boss cho quả đạn pháo này
            shellScript.SetTrackingList(activeFlakBullets);
        }
        // --------------------------------------------------------

        // 5. Gắn tốc độ
        Rigidbody2D rb = shellGO.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * flakShellSpeed; // Bay chậm
        }
    }
    // --------------------------------------------------------


    // (Các hàm GetRandomBottomTarget, FireSlowSpread,
    // GetSlowedWait, BossMove, OnTriggerEnter2D giữ nguyên)
    // ...
    Vector2 GetRandomBottomTarget()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera bị null!");
            return new Vector2(transform.position.x, transform.position.y - 10f);
        }
        Vector3 viewportPoint = new Vector3(Random.Range(0.0f, 1.0f), 0.0f, 0);
        float zDistance = playerTarget != null ? Mathf.Abs(playerTarget.position.z - mainCamera.transform.position.z) : 10f;
        viewportPoint.z = zDistance;
        return mainCamera.ViewportToWorldPoint(viewportPoint);
    }

    void FireSlowSpread()
    {
        if (slowBulletPrefab == null || slowSpreadFirePoint == null)
        {
            Debug.LogError("Chưa gán Prefab hoặc Fire Point cho đạn làm chậm!");
            return;
        }
        Debug.Log("Boss bắn đạn tỏa LÀM CHẬM!");
        float sogoctrungbinh = slowBulletAngle / (slowBulletCount - 1);
        float gocdobatdau = -slowBulletAngle / 2;
        for (int i = 0; i < (int)slowBulletCount; i++)
        {
            float gocdohientai = gocdobatdau + sogoctrungbinh * i;
            Quaternion rotation = Quaternion.Euler(0, 0, gocdohientai);
            Vector2 direction = rotation * Vector2.down;
            GameObject bullet = Instantiate(slowBulletPrefab, slowSpreadFirePoint.position, rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * slowBulletSpeed;
            }
            else
            {
                Debug.LogWarning("SlowBulletPrefab thiếu Rigidbody2D!");
            }
        }
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerController != null)
            {
                playerController.TakeDame(100f);
            }
            else
            {
                playerController = other.gameObject.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.TakeDame(100f);
                }
            }
        }
    }
}