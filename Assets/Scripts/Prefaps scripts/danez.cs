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
        
        if (transform.position.y > 6f || transform.position.y < -6f || transform.position.x > 10 || transform.position.x < -10)
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
            other.GetComponent<thienthachdichuyen>().TakeDame((dameResult.damage+player.damebonus) * 3);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, ((dameResult.damage + player.damebonus) * 3),dameResult.isCrit);
        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var dameResult = player.CalculateDamage();
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, (dameResult.damage + player.damebonus) * 3,dameResult.isCrit);
            other.GetComponent<BossController>().TakeDame((dameResult.damage + player.damebonus) * 3);

        }

        if(other.CompareTag("Monster"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var dameResult = player.CalculateDamage();
            other.GetComponent<RatMonster>().TakeDame((dameResult.damage + player.damebonus) * 3);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, (dameResult.damage + player.damebonus) * 3,dameResult.isCrit);
        }
    }
}
