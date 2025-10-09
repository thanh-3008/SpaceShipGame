using UnityEngine;

public class SkillDamageBoss1 : MonoBehaviour
{
    public int damage = 20;
    public float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Giả sử player có script Health
            
            Destroy(gameObject);
        }
    }
}
