using JetBrains.Annotations;
using System.Collections;
using System.Threading;
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

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;

    [Header("Attacks")]
    public float homingBulletSpeed = 10f;
    public float spreadBulletSpeed = 5f;
    public float slowDownFactor = 5f;

    // Tham chiếu
    public Transform player;
    public SeraphMKII skillMKII;

    void Start()
    {
        currentHealth = maxHealth;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            skillMKII = playerObject.GetComponent<SeraphMKII>();
            player = playerObject.transform;
        }

        StartCoroutine(BossAI_Pattern());
    }

    void Update()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                skillMKII = playerObject.GetComponent<SeraphMKII>();
            }
            else
            {
                return;
            }
        }
        BossMove();
    }

    // --- HÀM MỚI ---
    private WaitForSeconds GetSlowedWait(float normalDuration)
    {
        float waitTime = normalDuration;
        if (skillMKII != null && skillMKII.lamchamthoigian)
        {
            waitTime *= slowDownFactor;
        }
        return new WaitForSeconds(waitTime);
    }

    // --- BỘ NÃO AI CỦA BOSS (ĐÃ CẬP NHẬT) ---
    IEnumerator BossAI_Pattern()
    {
        yield return GetSlowedWait(2f);

        while (currentHealth > 0)
        {
            Debug.Log("AI: Bắn đạn tỏa (nhẹ)");
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(DanVomCung(8f, 120f));
                yield return GetSlowedWait(1.5f);
            }

            Debug.Log("AI: Di chuyển");
            yield return GetSlowedWait(3f);

            Debug.Log("AI: Bắn đạn tỏa (nặng)");
            StartCoroutine(DanVomCung2(24f, 120f));
            yield return GetSlowedWait(6.8f);

            Debug.Log("AI: Bắn đạn đuổi");
            for (int i = 0; i < 5; i++)
            {
                ShootFire();
                yield return GetSlowedWait(0.5f);
            }

            Debug.Log("AI: Nghỉ ngơi");
            yield return GetSlowedWait(4f);
        }
    }

    // --- CÁC HÀNH ĐỘNG CỦA BOSS ---
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
        thanhMau.capnhatthanhmau(currentHealth,maxHealth);
        if (currentHealth < 0)
        {
            Destroy(gameObject);
        }
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
        // Chú ý: Thời gian chờ giữa các viên đạn trong skill này đã được xử lý làm chậm rồi
        yield return GetSlowedWait(0.2f);

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
            yield return GetSlowedWait(0.2f); // Dùng hàm mới ở đây
        }
        yield return null;
    }
}