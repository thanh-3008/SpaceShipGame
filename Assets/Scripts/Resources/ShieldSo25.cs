using UnityEngine;

public class ShieldSo25 : MonoBehaviour
{
    [SerializeField]
    public float lifetime = 8f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject); // Chặn đạn
        }
    }
}
