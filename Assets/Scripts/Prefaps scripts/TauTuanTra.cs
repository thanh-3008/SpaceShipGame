using UnityEngine;

public class TauTuanTra : MonoBehaviour
{
    public PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 15f || transform.position.y < -15f || transform.position.x > 20 || transform.position.x < -20)
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
            other.GetComponent<thienthachdichuyen>().TakeDame(damageResult.damage * 3);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage * 3 , damageResult.isCrit);

        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<BossController>().TakeDame(damageResult.damage * 3);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage * 3, damageResult.isCrit);

        }
        if (other.CompareTag("Monster"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<RatMonster>().TakeDame(damageResult.damage * 3);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage * 3, damageResult.isCrit);

        }
    }


}
