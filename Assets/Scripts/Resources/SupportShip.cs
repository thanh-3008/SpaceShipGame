using UnityEngine;

public class SupportShip : MonoBehaviour
{
    public float speed = 5f;
    public float duration = 10f;
    public GameObject bulletPrefab;
    public float fireRate = 0.3f;
    public bool healPlayer = false;
    public bool aoeAttack = false;
    public float healAmount = 20f;
    public float aoeDamage = 50f;
    public float aoeRange = 5f;

    private float fireTimer;
    private float lifeTimer;

    void Update()
    {
        // Bay ngang qua màn hình
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Bắn đạn
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            Fire();
            fireTimer = 0f;
        }

        // Hiệu ứng hỗ trợ
       

        // Tự biến mất sau vài giây
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= duration)
            Destroy(gameObject);
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = Vector2.up * 15f;
    }

   

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRange);
    }
}
