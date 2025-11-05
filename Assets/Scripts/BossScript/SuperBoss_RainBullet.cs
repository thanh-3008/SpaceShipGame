using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class SuperBoss_RainBullet : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float damage = 10f;
    public float lifetime = 5f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * moveSpeed; // Bay thẳng xuống
        }
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null) { player.TakeDame(damage); }
            Destroy(gameObject);
        }
    }
}