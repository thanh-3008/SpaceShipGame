using UnityEngine;

public class star : MonoBehaviour
{
    public GameObject coinObject;
    
    public float speed = 5f;

    private GameObject player;
    public SeraphMKII skillMKII;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject objskill = GameObject.FindWithTag("Player");
        skillMKII = objskill.GetComponent<SeraphMKII>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skillMKII != null)
        {
            if (skillMKII.lamchamthoigian == false)
            {
                // SỬA: Dùng "transform" trực tiếp, không cần "thienthach.transform"
                coinObject.transform.Translate(Vector2.down * speed / 2 * Time.deltaTime);
            }
            else
            {
                coinObject.transform.Translate(Vector2.down * speed / 5 * Time.deltaTime);
            }
        }
        else
        {
            coinObject.transform.Translate(Vector2.down * speed  * Time.deltaTime);
        }
        if (coinObject.transform.position.y < -6f)
        {
            Destroy(coinObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(coinObject);
        }
    }
}
