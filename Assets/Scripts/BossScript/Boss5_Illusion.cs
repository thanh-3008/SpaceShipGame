using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// GẮN SCRIPT NÀY VÀO PREFAB "BẢN SAO" (clonePrefab)
public class Boss5_Illusion : MonoBehaviour
{
    [Header("Tốc độ di chuyển")]
    public float moveSpeed = 6f; // Tốc độ bay đến góc

    [Header("Balance")]
    public float slowDownFactor = 5f;

    [Header("Các Prefab Đạn (Kéo vào)")]
    public GameObject skill1_BulletPrefab;
    public GameObject skill3_BulletPrefab;
    public GameObject skill5_DarkMatterPrefab;

    [Header("Các bộ phận (Kéo vào)")]
    public Transform[] podFirePoints;
    public SpriteRenderer cloneSpriteRenderer;
    public Transform bossEye;

    private Transform playerTarget;
    private Transform destination;
    private SeraphMKII skillMKII;
    private Camera mainCamera;
    private bool isActivated = false;
    private bool isDashing = false; // <<< (THÊM MỚI)

    void Start()
    {
        mainCamera = Camera.main;
        if (cloneSpriteRenderer != null)
        {
            cloneSpriteRenderer.color = Color.yellow;
        }
    }

    public void Activate(Transform target, Transform dest, SeraphMKII slowMoScript)
    {
        this.playerTarget = target;
        this.destination = dest;
        this.skillMKII = slowMoScript;
        this.isActivated = true;
    }

    public void Vanish()
    {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    void Update()
    {
        if (!isActivated || isDashing) return; // <<< (SỬA ĐỔI: Nếu đang Dash, không làm gì cả)

        if (destination != null)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, destination.position, step);

            if (Vector2.Distance(transform.position, destination.position) < 0.1f)
            {
                destination = null;
            }
        }

        LookAtPlayer();
    }

    // --- CÁC HÀM NHẬN LỆNH TỪ BOSS THẬT ---

    public void Start_Skill_FocusCannons(int burstCount, float burstDelay)
    {
        if (isDashing) return; // Không bắn nếu đang lao
        StartCoroutine(Skill_FocusCannons(burstCount, burstDelay));
    }
    public void Start_Skill_SpiralBarrage(int shotCount, float spinDelay)
    {
        if (isDashing) return; // Không bắn nếu đang lao
        StartCoroutine(Skill_SpiralBarrage(shotCount, spinDelay));
    }
    public void Start_Skill_DarkMatterEruption(int count, float radius, float force, float prepareTime, float activeTime)
    {
        if (isDashing) return; // Không bắn nếu đang lao
        StartCoroutine(Skill_DarkMatterEruption(count, radius, force, prepareTime, activeTime));
    }

    // --- (THÊM MỚI: HÀM NHẬN LỆNH DASH) ---
    public void Start_Skill_VoidDash(Vector2 s1, Vector2 e1, Vector2 s2, Vector2 e2, float moveSpd, float dashSpd, float trailDist, GameObject trailPrefab)
    {
        StartCoroutine(Skill_VoidDash(s1, e1, s2, e2, moveSpd, dashSpd, trailDist, trailPrefab));
    }

    // --- (SKILL 1, 3, 5 GIỮ NGUYÊN) ---
    #region // --- SKILL 1, 3, 5 (GIỮ NGUYÊN) ---
    private IEnumerator Skill_FocusCannons(int currentBurstCount, float skill1_BurstDelay)
    {
        Debug.Log("Clone: Dùng Skill Random Burst");
        if (skill1_BulletPrefab == null || podFirePoints.Length < 2 || playerTarget == null) yield break;
        for (int i = 0; i < currentBurstCount; i++)
        {
            Vector2 targetPos1 = GetRandomTargetViewport(); FireBulletAtTarget(podFirePoints[0], targetPos1, skill1_BulletPrefab);
            Vector2 targetPos2 = GetRandomTargetViewport(); FireBulletAtTarget(podFirePoints[1], targetPos2, skill1_BulletPrefab);
            yield return GetSlowedWait(skill1_BurstDelay);
        }
    }

    private IEnumerator Skill_SpiralBarrage(int currentShotCount, float skill3_SpinDelay)
    {
        Debug.Log("Clone: Dùng Skill Random Barrage");
        if (skill3_BulletPrefab == null || podFirePoints.Length < 2) yield break;
        for (int i = 0; i < currentShotCount; i++)
        {
            int podIndex = i % 2;
            Vector2 targetPos = GetRandomTargetViewport(); FireBulletAtTarget(podFirePoints[podIndex], targetPos, skill3_BulletPrefab);
            yield return GetSlowedWait(skill3_SpinDelay);
        }
    }

    private IEnumerator Skill_DarkMatterEruption(int currentEruptionCount, float skill5_EruptionRadius, float skill5_EruptionForce, float skill5_PrepareTime, float skill5_ActiveTime)
    {
        Debug.Log("Clone: Dùng Skill Dark Matter Eruption");
        if (skill5_DarkMatterPrefab == null) yield break;
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

    // --- (THÊM MỚI: SKILL 4 CHO CLONE) ---
    private IEnumerator Skill_VoidDash(Vector2 startPos1, Vector2 endPos1, Vector2 startPos2, Vector2 endPos2,
                                     float currentMoveSpeed, float dashSpeed, float distancePerTrail, GameObject skill4_TrailPrefab)
    {
        Debug.Log("Clone: Dùng Skill Void Dash (X Shape)");
        isDashing = true;

        // Vị trí quay về của Clone (luôn là góc phải, vị trí 'destination' ban đầu)
        Vector2 returnPosition = this.transform.position;

        // (Code bay của Clone, y hệt boss thật)
        while (Vector2.Distance(transform.position, startPos1) > 0.1f) { float step = currentMoveSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, startPos1, step); yield return null; }
        yield return GetSlowedWait(0.5f);
        Vector2 lastTrailPos = transform.position;
        while (Vector2.Distance(transform.position, endPos1) > 0.1f) { float step = dashSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, endPos1, step); if (Vector2.Distance(transform.position, lastTrailPos) >= distancePerTrail) { Instantiate(skill4_TrailPrefab, transform.position, Quaternion.identity); lastTrailPos = transform.position; } yield return null; }
        while (Vector2.Distance(transform.position, startPos2) > 0.1f) { float step = currentMoveSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, startPos2, step); yield return null; }
        yield return GetSlowedWait(0.5f);
        lastTrailPos = transform.position;
        while (Vector2.Distance(transform.position, endPos2) > 0.1f) { float step = dashSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, endPos2, step); if (Vector2.Distance(transform.position, lastTrailPos) >= distancePerTrail) { Instantiate(skill4_TrailPrefab, transform.position, Quaternion.identity); lastTrailPos = transform.position; } yield return null; }
        yield return GetSlowedWait(1.0f);

        // (Code bay trở về)
        while (Vector2.Distance(transform.position, returnPosition) > 0.1f) { float step = currentMoveSpeed * Time.deltaTime; transform.position = Vector2.MoveTowards(transform.position, returnPosition, step); yield return null; }

        isDashing = false;
    }

    // --- HÀM HỖ TRỢ (Copy y chang Boss Thật) ---
    #region // --- HÀM HỖ TRỢ ---
    void LookAtPlayer()
    {
        if (bossEye != null && playerTarget != null)
        {
            Vector2 direction = (playerTarget.position - bossEye.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bossEye.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
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
        if (mainCamera == null || playerTarget == null)
            return new Vector2(0, -10);
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
    #endregion
}