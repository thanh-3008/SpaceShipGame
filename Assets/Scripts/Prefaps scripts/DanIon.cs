using UnityEngine;

public class DanIon : MonoBehaviour
{   
    private Rigidbody2D rb;
    public float speed = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
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
            
                other.GetComponent<thienthachdichuyen>().TakeDame(player.damehientai*320);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, player.damehientai*320);
        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();

            other.GetComponent<BossController>().TakeDame(player.damehientai * 320);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, player.damehientai * 320);

        }
    }
}

