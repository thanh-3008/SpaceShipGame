using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO BOSS 4 (Cùng với BossController)
public class Boss4Controller : MonoBehaviour, IBossAI
{
    [Header("Targeting")]
    public Transform playerTarget;
    public string playerTag = "Player";
    public PlayerController playerController;
    private SeraphMKII skillMKII;
    private Camera mainCamera; // Dùng cho Skill 3

    [Header("Balance")]
    public float slowDownFactor = 5f;

    [Header("AI Settings")]
    public float skillRestTimePhase1 = 3.0f;
    public float skillRestTimePhase2 = 2.0f;
    private bool isEnraged = false;
    private List<System.Func<IEnumerator>> skillList;

    [Header("Nội tại: Khiên Xoay")]
    [Tooltip("Kéo GameObject RỖNG (Container) chứa các tấm KHIÊN vào đây")]
    public Transform rotatingShieldContainer;
    public float shieldRotationSpeed = 80f;

    // --- (ĐÃ XÓA NỘI TẠI SÚNG XOAY) ---
    // (Đã xóa outerCannonContainer)
    // ------------------------------------

    [Header("Skill Settings")]
    [Tooltip("Kéo 4 Transform ĐIỂM BẮN (FirePoint) cố định vào đây")]
    public Transform[] firePoints;

    [Header("Skill 1: Crossfire (Tứ Xạ)")]
    public GameObject skill1_BulletPrefab;
    public int crossfireBurstCount = 3;
    public int crossfireBurstCount_P2 = 5;
    public float crossfireBurstDelay = 0.1f;
    [Tooltip("Góc tỏa của mỗi nòng súng (ví dụ 30 độ)")]
    public float crossfireSpreadAngle = 30f;

    [Header("Skill 2: Spin Cycle (Bắn Đuổi)")]
    public GameObject skill2_BulletPrefab;
    public int spinCycleShotCount = 20;
    public int spinCycleShotCount_P2 = 30;
    public float spinFireDelay = 0.1f;

    [Header("Skill 3: Missile Barrage (Mưa Đạn)")]
    public GameObject skill3_MissilePrefab;
    public int barrageShotCount = 5;
    public int barrageShotCount_P2 = 8;
    public float barrageFireDelay = 0.2f;

    [Header("Skill 4: Homing Burst (Bắn Đuổi Player)")]
    public GameObject skill4_HomingBulletPrefab;
    public int homingBurstCount = 3;
    public int homingBurstCount_P2 = 5;
    public float homingBurstDelay = 0.15f;

    [Header("Tùy chọn: Con Mắt")]
    public Transform bossEye;

    public bool isMove = true;
    // --- (Hàm Die, ActivateEnrage, Start giữ nguyên) ---
    public void Die()
    {
        StopAllCoroutines();
        this.enabled = false;
        Debug.Log("Boss 4 AI Đã Dừng");
    }
    public void ActivateEnrage()
    {
        if (isEnraged) return;
        isEnraged = true;
        Debug.Log("Boss 4 ENRAGED!");
        shieldRotationSpeed *= 1.5f;
        skillRestTimePhase1 = skillRestTimePhase2;
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
            Skill_Crossfire,
            Skill_SpinCycle,
            Skill_MissileBarrage,
            Skill_HomingBurst
        };

        StartCoroutine(BossAI_Pattern());
    }

    void Update()
    {
        if (playerTarget == null) { return; }
        HandlePassiveRotation(); // Giờ chỉ xoay khiên
        LookAtPlayer();
        if (isMove == true)
        {
            transform.Translate(Vector2.down * 1f * Time.deltaTime);
            if (transform.position.y <= 3)
            {
                isMove = false;
            }
        }
    }

    void HandlePassiveRotation()
    {
        float slowSpeed = 1f;
        if (skillMKII != null && skillMKII.lamchamthoigian)
        {
            slowSpeed = 1f / slowDownFactor;
        }
        if (rotatingShieldContainer != null)
        {
            rotatingShieldContainer.Rotate(0, 0, shieldRotationSpeed * slowSpeed * Time.deltaTime);
        }
    }

    // --- (LookAtPlayer và BossAI_Pattern giữ nguyên) ---
    #region // --- CÁC HÀM NỀN TẢNG (GIỮ NGUYÊN) ---
    void LookAtPlayer()
    {
        if (bossEye != null && playerTarget != null)
        {
            Vector2 direction = (playerTarget.position - bossEye.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bossEye.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
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
            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());
            float restTime = isEnraged ? skillRestTimePhase2 : skillRestTimePhase1;
            yield return GetSlowedWait(restTime);
        }
    }
    #endregion

    // --- CÁC SKILL (ĐÃ CẬP NHẬT) ---

    /// <summary>
    /// SKILL 1 (VIẾT LẠI): Tứ Xạ Tỏa (Spread Shot)
    /// Cả 4 nòng súng CÙNG LÚC bắn ra 1 chùm đạn tỏa (shotgun)
    /// </summary>
    private IEnumerator Skill_Crossfire()
    {
        Debug.Log("Boss 4: Dùng Skill Tứ Xạ Tỏa");
        if (skill1_BulletPrefab == null || firePoints == null || firePoints.Length < 4) yield break;

        int currentBurstCount = isEnraged ? crossfireBurstCount_P2 : crossfireBurstCount;

        for (int i = 0; i < currentBurstCount; i++)
        {
            // Bắn từ CẢ 4 NÒNG CÙNG LÚC
            foreach (Transform firePoint in firePoints)
            {
                // Bắn 1 chùm 3 viên (hoặc 5 viên)
                FireSpreadShot(firePoint, 3, crossfireSpreadAngle, skill1_BulletPrefab);
            }
            yield return GetSlowedWait(crossfireBurstDelay);
        }
    }

    /// <summary>
    /// SKILL 2 (VIẾT LẠI): Đạn Xoắn Ốc (Spiral)
    /// Bắn đạn LẦN LƯỢT từ nòng 1 -> 2 -> 3 -> 4
    /// </summary>
    private IEnumerator Skill_SpinCycle()
    {
        Debug.Log("Boss 4: Dùng Skill Đạn Xoắn Ốc");
        if (skill2_BulletPrefab == null || firePoints == null || firePoints.Length < 4) yield break;

        int currentShotCount = isEnraged ? spinCycleShotCount_P2 : spinCycleShotCount;

        for (int i = 0; i < currentShotCount; i++)
        {
            // Bắn lần lượt 0, 1, 2, 3, 0, 1, 2, 3...
            int pointIndex = i % 4;

            // Bắn 1 viên đạn theo hướng cố định của nòng súng
            FireBullet(firePoints[pointIndex], skill2_BulletPrefab);

            yield return GetSlowedWait(spinFireDelay);
        }
    }

    /// <summary>
    /// SKILL 3: Mưa Tên Lửa (Giữ nguyên)
    /// </summary>
    private IEnumerator Skill_MissileBarrage()
    {
        Debug.Log("Boss 4: Dùng Skill Mưa Tên Lửa");
        if (skill3_MissilePrefab == null || firePoints == null || firePoints.Length < 4 || mainCamera == null) yield break;

        int currentShotCount = isEnraged ? barrageShotCount_P2 : barrageShotCount;

        for (int i = 0; i < currentShotCount; i++)
        {
            foreach (Transform point in firePoints)
            {
                Vector2 targetPos = GetRandomBottomTarget();
                FireBulletAtTarget(point, targetPos, skill3_MissilePrefab);
            }
            yield return GetSlowedWait(barrageFireDelay);
        }
    }

    /// <summary>
    /// SKILL 4: Bắn Đuổi Player (Giữ nguyên)
    /// </summary>
    private IEnumerator Skill_HomingBurst()
    {
        Debug.Log("Boss 4: Dùng Skill Bắn Đuổi");
        if (skill4_HomingBulletPrefab == null || firePoints == null || firePoints.Length == 0 || playerTarget == null) yield break;

        int currentBurstCount = isEnraged ? homingBurstCount_P2 : homingBurstCount;

        for (int i = 0; i < currentBurstCount; i++)
        {
            // Chọn 1 nòng súng ngẫu nhiên
            Transform randomFirePoint = firePoints[Random.Range(0, firePoints.Length)];
            // Lấy vị trí Player
            Vector2 targetPos = playerTarget.position;
            // Bắn
            FireBulletAtTarget(randomFirePoint, targetPos, skill4_HomingBulletPrefab);

            yield return GetSlowedWait(homingBurstDelay);
        }
    }

    // --- HÀM HỖ TRỢ (ĐÃ CẬP NHẬT) ---

    /// <summary>
    /// Bắn 1 prefab đạn theo HƯỚNG XOAY của nòng súng
    /// </summary>
    void FireBullet(Transform firePoint, GameObject prefab)
    {
        if (firePoint == null || prefab == null) return;
        Instantiate(prefab, firePoint.position, firePoint.rotation);
    }

    /// <summary>
    /// Bắn 1 prefab đạn TỚI 1 MỤC TIÊU
    /// </summary>
    void FireBulletAtTarget(Transform firePoint, Vector2 targetPosition, GameObject prefab)
    {
        if (firePoint == null || prefab == null) return;

        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);

        Instantiate(prefab, firePoint.position, bulletRotation);
    }

    /// <summary>
    /// (HÀM MỚI) Bắn 1 chùm đạn tỏa (shotgun)
    /// </summary>
    void FireSpreadShot(Transform firePoint, int bulletCount, float spreadAngle, GameObject prefab)
    {
        if (firePoint == null || prefab == null) return;

        // Lấy góc bắn chính (hướng của nòng súng)
        float baseAngle = firePoint.eulerAngles.z;
        float startAngle = baseAngle - spreadAngle / 2;
        float angleStep = spreadAngle / (bulletCount - 1);

        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + (i * angleStep);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Instantiate(prefab, firePoint.position, rotation);
        }
    }


    /// <summary>
    /// Lấy 1 vị trí ngẫu nhiên ở mép dưới camera
    /// </summary>
    Vector2 GetRandomBottomTarget()
    {
        if (mainCamera == null || playerTarget == null)
            return new Vector2(0, -10);

        float z = Mathf.Abs(mainCamera.transform.position.z - playerTarget.position.z);
        Vector3 viewportPoint = new Vector3(Random.value, 0, z);

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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDame(80f);
            }
        }
    }
}