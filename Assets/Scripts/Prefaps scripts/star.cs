using UnityEngine;

public class star : MonoBehaviour
{
    public GameObject coinObject;
    
    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinObject.transform.Translate(Vector2.down * speed * Time.deltaTime);
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
