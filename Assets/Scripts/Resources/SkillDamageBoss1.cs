using UnityEngine;

public class SkillDamageBoss1 : MonoBehaviour
{
    [Header("Skill Settings")]
    public int damage = 20;        // Sát thương gây cho player
    public float lifeTime = 3f;    // Thời gian tồn tại trước khi tự hủy
    public GameObject hitEffect;   // Hiệu ứng va chạm (nếu có)

    void Start()
    {
        Destroy(gameObject, lifeTime); // tự hủy sau lifeTime giây
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Tìm script máu của Player
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage); // trừ máu player
            }

            // Spawn hiệu ứng nổ khi đạn trúng
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject); // hủy viên đạn sau khi va chạm
        }
        else if (collision.CompareTag("Wall"))
        {
            // Nếu chạm tường cũng hủy luôn
            Destroy(gameObject);
        }
    }
}
