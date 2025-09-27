using UnityEngine;

public class DanPro : MonoBehaviour
{
    public GameObject danpro;
    public float speed;
    private Rigidbody2D rb;
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        danpro.transform.Rotate(0, 0, -500 * Time.deltaTime);
    }
}
