using System.Collections;
using UnityEngine;

public class RatMonster : MonoBehaviour
{
    public MonsterData monsterData; // Kéo ScriptableObject vào đây
    private PlayerController playerController;
    private PlayerLevel playerLevel;
    private Animator animator;

    private float currentHealth;   // Máu hiện tại của mỗi con quái, không sửa trực tiếp MonsterData
    private bool canAttack = true; // Cờ kiểm tra xem quái có thể tấn công không (cooldown)
    private bool playerInRange = false; // Cờ kiểm tra xem người chơi có trong tầm tấn công không
    public ThanhMauThienThach monster;
    void Start()
    {
        // Lấy các component cần thiết
        animator = GetComponent<Animator>();
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            playerLevel = playerObject.GetComponent<PlayerLevel>();
        }

        // Khởi tạo máu cho từng con quái
        currentHealth = monsterData.thanhMauToiDa;
        monster.capnhatthanhmau(currentHealth, monsterData.thanhMauToiDa);


    }

    void Update()
    {
        // Nếu không tìm thấy người chơi thì không làm gì cả
        if (playerController == null) return;

        // Nếu người chơi không ở trong tầm tấn công, quái sẽ di chuyển tới
        if (!playerInRange)
        {
            MonsterMove(playerController.transform.position);
        }
        else // Nếu người chơi trong tầm, quái sẽ ngừng di chuyển và cố gắng tấn công
        {
            animator.SetBool("IsRunning", false); // Dừng animation chạy
            // Cố gắng tấn công nếu có thể
            AttemptAttack();
        }
    }

    // --- LOGIC VA CHẠM ---

    // Khi người chơi BẮT ĐẦU đi vào vùng trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
        }
        if (collision.CompareTag("TauMe"))
        {
            Die();
        }
    }

    // Khi người chơi THOÁT KHỎI vùng trigger
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // --- LOGIC TẤN CÔNG ---

    void AttemptAttack()
    {
        if (canAttack)
        {
            canAttack = false; // Chặn các lần tấn công tiếp theo ngay lập tức
            animator.SetTrigger("Attack"); // Chỉ kích hoạt animation           
            // Việc trừ máu sẽ được gọi bởi Animation Event
        }
    }

    // *** HÀM NÀY SẼ ĐƯỢC GỌI TỪ ANIMATION EVENT ***
    // Đây là hàm thực sự gây sát thương
    public void DealDamageEvent()
    {
        // Kiểm tra lại lần nữa để chắc chắn người chơi vẫn còn trong tầm
        if (playerInRange && playerController != null)
        {
            Debug.Log("Quái vật gây sát thương!");
            playerController.TakeDame(monsterData.satThuong);
        }
        // Bắt đầu coroutine hồi chiêu sau khi đã gây sát thương
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        // Đợi theo thời gian nghỉ
        yield return new WaitForSeconds(monsterData.tgianNghiTanCong);       
        // Cho phép tấn công trở lại
        canAttack = true;
    }


    // --- LOGIC NHẬN SÁT THƯƠNG VÀ CHẾT ---

    public void TakeDame(float dame)
    {
        currentHealth -= dame;
        monster.capnhatthanhmau(currentHealth, monsterData.thanhMauToiDa);
        if (currentHealth <= 0)
        {
            animator.SetBool("IsRunning", false);
            animator.SetTrigger("Die");
            Die();
        }
    }

    void Die()
    {
        if (playerLevel != null)
        {
            playerLevel.AddXP(monsterData.diemKinhNghiem);
        }       

        // Vô hiệu hóa script và collider để nó không thể di chuyển hay tấn công nữa
        this.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        // Hủy GameObject sau 2 giây để animation chết kịp chạy
        Destroy(gameObject, 0.5f);
    }

    // --- LOGIC DI CHUYỂN ---

    public void MonsterMove(Vector3 playerTransform)
    {
        animator.SetBool("IsRunning", true); // Dùng SetBool cho trạng thái chạy
        Vector3 direction = (playerTransform - transform.position).normalized;
        transform.position += direction * monsterData.tocDoDiChuyen * Time.deltaTime;

        // Lật sprite
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