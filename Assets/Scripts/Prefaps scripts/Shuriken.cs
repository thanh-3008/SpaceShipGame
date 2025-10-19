using UnityEngine;

public class Shuriken : MonoBehaviour
{
    public PlayerController player;
    public float tocDoQuay =720f;
    public float dameGayRa;
    public bool isThienThach;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
       
        transform.Rotate(Vector3.forward, tocDoQuay*Time.deltaTime);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            var damageResult = player.CalculateDamage();
            if(!isThienThach)
            {
                dameGayRa = damageResult.damage/2;
            }else
            {
                dameGayRa = damageResult.damage * 3;
            }
            collision.GetComponent<thienthachdichuyen>().TakeDame(dameGayRa);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, dameGayRa, damageResult.isCrit);
          
        }
        if (collision.CompareTag("Boss"))
        {
            var damageResult = player.CalculateDamage();
            if (!isThienThach)
            {
                dameGayRa = damageResult.damage / 2;
            }
            else
            {
                dameGayRa = damageResult.damage * 3;
            }
            collision.GetComponent<BossController>().TakeDame(dameGayRa);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, dameGayRa, damageResult.isCrit);
            
        }
        if (collision.CompareTag("Monster"))
        {
            var damageResult = player.CalculateDamage();
            if (isThienThach==false)
            {
                dameGayRa = damageResult.damage / 2;
            }
            else
            {
                dameGayRa = damageResult.damage * 3;
            }
            collision.GetComponent<RatMonster>().TakeDame(dameGayRa);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, dameGayRa, damageResult.isCrit);
            
        }
    }

}
