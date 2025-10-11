using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Stats")]
    public ThanhMauThienThach thanhMau;
    public float maxHealth = 100000f;
    public float currentHealth;
    public float moveSpeed = 2f;

    [Header("Movement")]
    public float leftPoint;
    public float rightPoint;
    private bool movingRight = true;
    public bool isMove=true;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Attacks")]
    public float predictiveBulletSpeed = 10f;
    public float homingBulletSpeed = 10f;
    public float spreadBulletSpeed = 5f;
    public float slowDownFactor = 5f;

    [Header("DashAttack")]
    public float dashSpeed = 10f;
    public float dashWarningTime = 1.5f;
    public float dashPauseTime = 0.5f;

    // Tham chiếu
    public Transform player;
    public PlayerController playerController;
    private Rigidbody2D playerRb;
    public SeraphMKII skillMKII;
    public AudioManagement audioManagement;
    public GameObject Spawn;
    void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            skillMKII = playerObject.GetComponent<SeraphMKII>();
            player = playerObject.transform;
            playerRb = playerObject.GetComponent<Rigidbody2D>();
        }
        GameObject objaudio = GameObject.Find("AudioManagement");
        audioManagement = objaudio.GetComponent<AudioManagement>();      
        StartCoroutine(BossAI_Pattern());
    }

    void Update()
    {
        if (player == null)
        {
            // Tìm lại player nếu bị null
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                playerController = playerObject.GetComponent<PlayerController>();
                skillMKII = playerObject.GetComponent<SeraphMKII>();
                playerRb = playerObject.GetComponent<Rigidbody2D>();
            }
            else
            {
                // Nếu không tìm thấy player, không làm gì cả
                return;
            }
        }

        if (isMove == true)
        {
            transform.Translate(Vector2.down * 1f * Time.deltaTime);
            if(transform.position.y <= 3)
            {
                isMove = false;
            }
        }

        BossMove();
    }

    // ===================================================================
    // *** PHẦN AI ĐÃ ĐƯỢC SỬA LỖI VÀ CẢI TIẾN ***
    // ===================================================================
    IEnumerator BossAI_Pattern()
    {
        yield return GetSlowedWait(5f); // Chờ lúc bắt đầu

        var skillList = new List<System.Func<IEnumerator>>()
        {
            startvomcung1,
            startdanvomcung,
            startxenke,
            startlaoxuong
        };

        int lastSkillIndex = -1; // Biến để lưu kỹ năng vừa dùng

        while (currentHealth > 0)
        {
            int currentSkillIndex;

            // Chọn kỹ năng mới, đảm bảo không trùng với kỹ năng cũ
            do
            {
                currentSkillIndex = Random.Range(0, skillList.Count);
            } while (skillList.Count > 1 && currentSkillIndex == lastSkillIndex);

            lastSkillIndex = currentSkillIndex; // Cập nhật kỹ năng vừa chọn

            // *** SỬA LỖI QUAN TRỌNG NHẤT ***
            // Phải "yield return" để chờ cho kỹ năng thực hiện xong
            yield return StartCoroutine(skillList[currentSkillIndex].Invoke());

            // Thêm một khoảng nghỉ ngắn giữa các kỹ năng để tạo nhịp độ
            yield return GetSlowedWait(2.0f);
        }
    }

    public IEnumerator startvomcung1()
    {
        Debug.Log("AI: Bắn đạn tỏa (nhẹ)");
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(DanVomCung(8f, 150f));
            yield return GetSlowedWait(1.5f);
        }
    }
    public IEnumerator startlaoxuong()
    {
        Debug.Log("AI: Lao xuống");
        this.enabled = false; // Tắt Update() (bao gồm cả BossMove)
        yield return StartCoroutine(LaoXuong());
        this.enabled = true; // Bật lại Update()
        yield return GetSlowedWait(1f);
    }
    public IEnumerator startdanvomcung()
    {
        Debug.Log("AI: Bắn đạn tỏa (nặng)");
        yield return StartCoroutine(DanVomCung2(24f, 120f)); // Dùng yield return ở đây để đảm bảo nó chạy xong
    }

    public IEnumerator startxenke()
    {
        var skillban = new List<System.Action>()
        {
            ShootPredictive,ShootFire
        };
        int skill = Random.Range(0, skillban.Count);

        Debug.Log("AI: Bắn đạn xen kẽ (Dự đoán & Thường)");
        for (int i = 0; i < 13; i++)
        {
            skillban[skill].Invoke();
            yield return GetSlowedWait(0.6f);
        }
    }

    // ===================================================================
    // *** CÁC HÀM GỐC (GIỮ NGUYÊN) ***
    // ===================================================================

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

    public void TakeDame(float damage)
    {
        currentHealth -= damage;
        thanhMau.capnhatthanhmau(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ShootPredictive()
    {
        if (player == null || playerRb == null) { ShootFire(); return; }
        Vector2 playerPos = player.position;
        Vector2 playerVel = playerRb.linearVelocity; // Nên dùng velocity
        Vector2 bossPos = firePoint.position;
        float bulletSpeed = predictiveBulletSpeed;
        Vector2 deltaPos = playerPos - bossPos;
        float a = Vector2.Dot(playerVel, playerVel) - bulletSpeed * bulletSpeed;
        float b = 2f * Vector2.Dot(deltaPos, playerVel);
        float c = Vector2.Dot(deltaPos, deltaPos);
        float delta = b * b - 4f * a * c;

        if (delta >= 0)
        {
            float t1 = (-b - Mathf.Sqrt(delta)) / (2f * a);
            float t2 = (-b + Mathf.Sqrt(delta)) / (2f * a);
            float timeToImpact = (t1 > 0 && (t1 < t2 || t2 < 0)) ? t1 : t2; // logic chọn thời gian tốt hơn

            if (timeToImpact > 0)
            {
                Vector2 predictedPos = playerPos + playerVel * timeToImpact;
                Vector2 fireDirection = (predictedPos - bossPos).normalized;
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                bullet.GetComponent<BossBulletController>().normalSpeed = predictiveBulletSpeed;
                bullet.GetComponent<Rigidbody2D>().linearVelocity = fireDirection * predictiveBulletSpeed;
                return;
            }
        }
        ShootFire();
    }

    public void ShootFire()
    {
        if (player != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Vector2 direction = (player.position - firePoint.position).normalized;
            bullet.GetComponent<BossBulletController>().normalSpeed = homingBulletSpeed;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * homingBulletSpeed;
        }
    }

    public IEnumerator DanVomCung(float sodan, float gocdo)
    {
        float sogoctrungbinh = gocdo / (sodan - 1);
        float gocdobatdau = -gocdo / 2;
        for (int i = 0; i < (int)sodan; i++)
        {
            float gocdohientai = gocdobatdau + sogoctrungbinh * i;
            Quaternion rotation = Quaternion.Euler(0, 0, gocdohientai);
            Vector2 direction = rotation * Vector2.down;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<BossBulletController>().normalSpeed = spreadBulletSpeed;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * spreadBulletSpeed;
        }
        yield return null;
    }

    public IEnumerator DanVomCung2(float sodan, float gocdo)
    {
        yield return GetSlowedWait(0.2f);
        float sogoctrungbinh = gocdo / (sodan - 1);
        float gocdobatdau = -gocdo / 2;
        for (int i = 0; i < (int)sodan; i++)
        {
            float gocdohientai = gocdobatdau + sogoctrungbinh * i;
            Quaternion rotation = Quaternion.Euler(0, 0, gocdohientai);
            Vector2 direction = rotation * Vector2.down;
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, rotation);
            bullet.GetComponent<BossBulletController>().normalSpeed = spreadBulletSpeed * 2f;
            bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * spreadBulletSpeed * 2f;
            yield return GetSlowedWait(0.2f);
        }
        yield return null;
    }

    public IEnumerator LaoXuong()
    {
        audioManagement.PlaySfxto(audioManagement.bossDashskill);
        Debug.Log("Skill lao xuong bat dau");
        Vector3 startposition = transform.position;
        Vector3 targetposition = new Vector3(player.position.x, player.position.y, transform.position.z);

        yield return GetSlowedWait(dashWarningTime);

        // Lao tới
        while (Vector3.Distance(transform.position, targetposition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetposition, dashSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetposition;
        yield return GetSlowedWait(dashPauseTime);

        // Quay về
        while (Vector3.Distance(transform.position, startposition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, startposition, dashSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = startposition;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController.TakeDame(100f);
        }
    }
    public void OnDestroy()
    {
        Spawn = GameObject.Find("Spawn");
        if (Spawn != null)
        {
            Transform spawnthienthach = Spawn.transform.Find("thienthachxuathien");
            if(spawnthienthach != null)
            {
                GameObject obj = spawnthienthach.gameObject;
                obj.SetActive(true);
            }
        }
    }
}