using UnityEngine;

public class FireBallMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D rb;
    public float speed = 5f;
    public PlayerController playerController;
    public GameObject explosionEffectPrefab;
    public AudioManagement audioManagement;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject objaudio = GameObject.Find("AudioManagement");
        audioManagement = objaudio.GetComponent<AudioManagement>();
    }

    // Update is called once per frame
    void Update()
    { 
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        if (transform.position.y < -6f)
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (rb != null)
        {
            if (other.CompareTag("Enemy"))
            {
                audioManagement.PlaySfx(audioManagement.vachamfireball);
                var resultDamaged = playerController.CalculateDamage();
                other.GetComponent<thienthachdichuyen>().TakeDame(resultDamaged.damage * 50f);
                DamePopUpGenerator.Instance.CreatePopUp(transform.position, resultDamaged.damage * 50f, resultDamaged.isCrit);
                Debug.Log("gay dame len thien thach"+playerController.damehientai*50);
                if (explosionEffectPrefab != null)
                {
                    GameObject no = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(no, 0.15f);
                }
                Destroy(gameObject);
            }
            if (other.CompareTag("Boss"))
            {
                audioManagement.PlaySfx(audioManagement.vachamfireball);
                var resultDamaged = playerController.CalculateDamage();
                other.GetComponent<BossController>().TakeDame(resultDamaged.damage * 50f);
                DamePopUpGenerator.Instance.CreatePopUp(transform.position, resultDamaged.damage * 50f, resultDamaged.isCrit);
                Debug.Log("gay dame len boss" + playerController.damehientai * 50);
                if (explosionEffectPrefab != null)
                {
                    GameObject no = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                    Destroy(no, 0.15f);
                }
                Destroy(gameObject);
            }
        }
     
    }
    
}
