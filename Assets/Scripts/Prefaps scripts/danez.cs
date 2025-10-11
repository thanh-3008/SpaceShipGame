using UnityEngine;

public class danez : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            var dameResult = player.CalculateDamage();
            other.GetComponent<thienthachdichuyen>().TakeDame((dameResult.damage+player.damebonus) * 4);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, ((dameResult.damage + player.damebonus) * 4),dameResult.isCrit);
        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var dameResult = player.CalculateDamage();
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, (dameResult.damage + player.damebonus) * 4,dameResult.isCrit);
            other.GetComponent<BossController>().TakeDame((dameResult.damage + player.damebonus) * 4);

        }
    }
}
