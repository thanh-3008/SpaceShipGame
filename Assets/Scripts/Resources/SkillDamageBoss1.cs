using UnityEngine;

public class SkillDamageBoss1 : MonoBehaviour
{
    [Header("Skill Settings")]
    public int damage = 20;            // Sát thương gây ra
    public float lifeTime = 3f;        // Tồn tại trong 3 giây trước khi biến mất
    public GameObject hitEffect;       // Hiệu ứng khi va chạm (nếu có)

    void Start()
    {
        // Tự hủy sau lifeTime giây để tránh rác game object
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Tìm script máu của Player
            PlayerHealth player = collision.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage); // Trừ máu Player
            }

            // Sinh hiệu ứng va chạm nếu có
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            // Nếu chạm tường thì cũng hủy skill
            Destroy(gameObject);
        }
    }
}
