using UnityEngine;

public class bufftienlua : MonoBehaviour
{
    public GameObject bufftenlua;

    public float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bufftenlua.transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (bufftenlua.transform.position.y < -6f)
        {
            Destroy(bufftenlua);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(bufftenlua);
        }
    }
}
