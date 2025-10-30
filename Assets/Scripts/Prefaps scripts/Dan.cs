using UnityEngine;

public class Dan : MonoBehaviour
{

    public float speed = 5f;
    public float damex = 0; 
    public Animator anim;
    public PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 6f || transform.position.y < -6f || transform.position.x>10 || transform.position.x<-10)
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
            
        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<BossController>().TakeDame(damageResult.damage + damex);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage + damex, damageResult.isCrit);
            
        }

        if (other.CompareTag("Monster"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<RatMonster>().TakeDame(damageResult.damage + damex);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage + damex, damageResult.isCrit);
                   
        }
    }
}
