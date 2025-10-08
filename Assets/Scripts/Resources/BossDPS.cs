using UnityEngine;

public class BossController : MonoBehaviour
{
    [Header("Skill Settings")]
    public GameObject skillPrefab; // Prefab đạn hoặc skill
    public Transform firePoint;    // Vị trí bắn ra
    public float skillCooldown = 5f; // thời gian hồi
    public float skillForce = 10f;

    private float skillTimer = 0f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        skillTimer += Time.deltaTime;

        // Khi đủ thời gian hồi chiêu thì dùng skill
        if (skillTimer >= skillCooldown)
        {
            UseSkill();
            skillTimer = 0f;
        }
    }

    void UseSkill()
    {
        // Gọi animation (nếu có)
        if (animator != null)
            animator.SetTrigger("Skill");

        // Gọi hàm thi triển thật sự (delay bằng animation event nếu cần)
        Invoke(nameof(CastSkill), 0.5f); // delay 0.5s cho animation
    }

    void CastSkill()
    {
        GameObject skill = Instantiate(skillPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(firePoint.right * skillForce, ForceMode2D.Impulse);
        }
    }
}
