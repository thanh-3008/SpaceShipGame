using UnityEngine;

// GẮN SCRIPT NÀY LÊN PREFAB ĐẠN VẢY
[RequireComponent(typeof(Rigidbody2D))]
public class Boss3_ScatterBullet : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float damage = 10f;
    public float lifetime = 3f; // Tự hủy sau 3 giây

    void Start()
    {
        // Bay thẳng theo hướng 'up' (đã được xoay bởi Boss)
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * moveSpeed;
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Gây sát thương cho Player (dùng hàm TakeDame của bạn)
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDame(damage);
            }
            Destroy(gameObject); // Hủy đạn
        }
    }
}