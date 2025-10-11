using UnityEngine;

public class Dan : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public float damex = 0; 
    public Animator anim;
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
            var damageResult = player.CalculateDamage();
            other.GetComponent<thienthachdichuyen>().TakeDame(damageResult.damage+damex);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage + damex, damageResult.isCrit);
            anim.SetTrigger("hit");
            Destroy(gameObject,0.3f);
        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<BossController>().TakeDame(damageResult.damage + damex);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage + damex, damageResult.isCrit);
            anim.SetTrigger("hit");
            Destroy(gameObject, 0.3f);
        }
    }
}
