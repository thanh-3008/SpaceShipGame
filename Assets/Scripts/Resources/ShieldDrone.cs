using UnityEngine;

public class ShieldDrone : MonoBehaviour
{
    public Transform player;
    public float orbitRadius = 1.7f;
    public float orbitSpeed = 100f;
    public int maxHealth = 3;
    public float respawnTime = 3f;

    private int currentHealth;
    private float currentAngle;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        currentAngle = Random.Range(0f, 360f);
    }

    void Update()
    {
        if (!isDead)
        {
            RotateAroundPlayer();
        }
    }

    void RotateAroundPlayer()
    {
        currentAngle += orbitSpeed * Time.deltaTime;
        float rad = currentAngle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * orbitRadius;
        transform.position = player.position + offset;

        transform.up = (transform.position - player.position).normalized;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            StartCoroutine(Respawn());
        }
    }

    System.Collections.IEnumerator Respawn()
    {
        isDead = true;
        currentHealth = maxHealth;

        // Hiệu ứng biến mất
        gameObject.SetActive(false);

        yield return new WaitForSeconds(respawnTime);

        // Hồi sinh
        gameObject.SetActive(true);
        isDead = false;
    }
}
