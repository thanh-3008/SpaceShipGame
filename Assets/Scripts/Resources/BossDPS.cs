using UnityEngine;

public class BossDPS : MonoBehaviour
{
    [Header("Skill Settings")]
    public GameObject skillPrefab;       // Prefab skill thường
    public GameObject specialSkillPrefab; // Prefab skill đặc biệt (nếu có)
    public Transform firePoint;          // Vị trí bắn ra
    public float skillCooldown = 5f;     
    public float specialCooldown = 12f;  
    public float skillForce = 10f;

    [Header("Boss Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Target")]
    public Transform player;             // Tham chiếu đến player để xoay hướng bắn
    public float rotateSpeed = 3f;

    private float skillTimer = 0f;
    private float specialTimer = 0f;
    private Animator animator;
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        skillTimer += Time.deltaTime;
        specialTimer += Time.deltaTime;

        RotateTowardsPlayer();

        // Skill thường
        if (skillTimer >= skillCooldown)
        {
            UseSkill();
            skillTimer = 0f;
        }

        // Skill đặc biệt
        if (specialTimer >= specialCooldown)
        {
            UseSpecialSkill();
            specialTimer = 0f;
        }
    }

    void RotateTowardsPlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
    }

    void UseSkill()
    {
        if (animator != null) animator.SetTrigger("Skill");

        Invoke(nameof(CastSkill), 0.3f);
    }

    void CastSkill()
    {
        GameObject skill = Instantiate(skillPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(firePoint.right * skillForce, ForceMode2D.Impulse);
        }

        if (audioSource != null) audioSource.Play(); // phát âm thanh nếu có
    }

    void UseSpecialSkill()
    {
        if (animator != null) animator.SetTrigger("Special");

        Invoke(nameof(CastSpecialSkill), 0.5f);
    }

    void CastSpecialSkill()
    {
        // Ví dụ: bắn 3 viên đạn theo hình nón
        for (int i = -1; i <= 1; i++)
        {
            Quaternion spread = Quaternion.Euler(0, 0, i * 15f);
            GameObject skill = Instantiate(specialSkillPrefab, firePoint.position, firePoint.rotation * spread);
            Rigidbody2D rb = skill.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(skill.transform.right * (skillForce * 1.2f), ForceMode2D.Impulse);
            }
        }

        if (audioSource != null) audioSource.Play();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (animator != null) animator.SetTrigger("Die");
        Destroy(gameObject, 1.5f); // hủy boss sau 1.5s
    }
}
