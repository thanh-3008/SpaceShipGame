using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public float moveSpeed = 6f;
    public float jumpForce = 12f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    Rigidbody2D rb;
    bool facingRight = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Di chuyển ngang
        float x = Input.GetAxisRaw("Horizontal"); // -1..1
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        // Lật hướng sprite nếu cần (nếu dùng sprite Renderer)
        if (x > 0.01f && !facingRight) Flip();
        if (x < -0.01f && facingRight) Flip();

        // Nhảy: kiểm tra chạm đất
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1f, transform.localScale.y, transform.localScale.z);
    }

    // Gizmos để thấy vị trí groundCheck
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}

