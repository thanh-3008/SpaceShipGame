using System.Collections;
using UnityEngine;

public class RatMonster : MonoBehaviour
{
    public MonsterData monsterData; // Kéo ScriptableObject vào đây
    private PlayerController playerController;
    private PlayerLevel playerLevel;
    private Animator animator;

    private float currentHealth;   // Máu hiện tại của mỗi con quái
    private bool canAttack = true;
    private bool playerInRange = false;
    public ThanhMauThienThach monster;

    // --- BIẾN MỚI ---
    private bool isDying = false; // Cờ để tránh gọi Die() nhiều lần

    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            playerLevel = playerObject.GetComponent<PlayerLevel>();
        }
        currentHealth = monsterData.thanhMauToiDa;
        monster.capnhatthanhmau(currentHealth, monsterData.thanhMauToiDa);
    }

    void Update()
    {
        // Nếu không tìm thấy người chơi hoặc quái đang chết, thì không làm gì
        if (playerController == null || isDying) return;

        if (!playerInRange)
        {
            MonsterMove(playerController.transform.position);
        }
        else
        {
            animator.SetBool("IsRunning", false);
            AttemptAttack();
        }
    }

    // --- LOGIC VA CHẠM ---
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDying) return; // Nếu đang chết, bỏ qua va chạm

        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
        if (collision.CompareTag("TauMe"))
        {
            // Gọi Die(true) vì va chạm tàu mẹ vẫn nên cho XP? 
            // Hoặc Die(false) nếu không muốn cho XP.
            // Ở đây tôi mặc định là cho XP.
            Die(true);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (isDying) return;

        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // --- LOGIC TẤN CÔNG ---
    void AttemptAttack()
    {
        if (canAttack && !isDying)
        {
            canAttack = false;
            animator.SetTrigger("Attack");
        }
    }

    public void DealDamageEvent()
    {
        if (playerInRange && playerController != null && !isDying)
        {
            Debug.Log("Quái vật gây sát thương!");
            playerController.TakeDame(monsterData.satThuong);
        }
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(monsterData.tgianNghiTanCong);
        canAttack = true;
    }

    // --- LOGIC NHẬN SÁT THƯƠNG VÀ CHẾT ---

    public void TakeDame(float dame)
    {
        if (isDying) return; // Không nhận thêm sát thương nếu đã chết

        currentHealth -= dame;
        monster.capnhatthanhmau(currentHealth, monsterData.thanhMauToiDa);
        if (currentHealth <= 0)
        {
            animator.SetBool("IsRunning", false);
            animator.SetTrigger("Die");
            Die(true); // Chết do nhận sát thương -> có cộng EXP
        }
    }

    // --- HÀM DIE ĐÃ ĐƯỢC SỬA ĐỔI ---
    // Thêm tham số 'giveXP' (mặc định là true)
    void Die(bool giveXP = true)
    {
        // Kiểm tra cờ isDying để đảm bảo hàm này chỉ chạy 1 lần
        if (isDying) return;
        isDying = true;

        // Chỉ cộng EXP nếu 'giveXP' là true
        if (giveXP && playerLevel != null)
        {
            playerLevel.AddXP(monsterData.diemKinhNghiem);
        }

        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 0.5f);
    }

    // --- HÀM MỚI ĐỂ BOSS GỌI ---
    /// <summary>
    /// Hàm này được gọi bởi SpawnMonster khi boss xuất hiện.
    /// Quái sẽ chết ngay lập tức và KHÔNG cho EXP.
    /// </summary>
    public void DieFromBoss()
    {
        if (isDying) return; // Đã chết rồi thì thôi

        animator.SetBool("IsRunning", false);
        animator.SetTrigger("Die");
        Die(false); // Gọi hàm Die và truyền 'false' để không cộng EXP
    }

    // --- LOGIC DI CHUYỂN ---
    public void MonsterMove(Vector3 playerTransform)
    {
        if (isDying) return;

        animator.SetBool("IsRunning", true);
        Vector3 direction = (playerTransform - transform.position).normalized;
        transform.position += direction * monsterData.tocDoDiChuyen * Time.deltaTime;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }
    }
}