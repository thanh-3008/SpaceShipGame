using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public PlayerController player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        transform.Rotate(Vector3.forward, 720*Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            var damageResult = player.CalculateDamage();
            collision.GetComponent<thienthachdichuyen>().TakeDame(damageResult.damage/2);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage / 2, damageResult.isCrit);
          
        }
        if (collision.CompareTag("Boss"))
        {
            var damageResult = player.CalculateDamage();
            collision.GetComponent<BossController>().TakeDame(damageResult.damage / 2);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage / 2, damageResult.isCrit);
            
        }
        if (collision.CompareTag("Monster"))
        {
            var damageResult = player.CalculateDamage();
            collision.GetComponent<RatMonster>().TakeDame(damageResult.damage / 2);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage / 2, damageResult.isCrit);
            
        }
    }

}
