using System.Xml.Linq;
using UnityEngine;

public class Tenlua : MonoBehaviour
{
    public GameObject tenlua;
    // public float speed; // XÓA BỎ: Không cần nữa
    private Rigidbody2D rb;
    //public Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        // THÊM MỚI: Tự hủy sau 5 giây, giống như DanIon
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // XÓA BỎ: Dòng "transform.Translate(Vector2.right * speed * Time.deltaTime);"
        // XÓA BỎ: Dòng "if(transform.position.y > 6f)" (vì đã thay bằng Destroy ở trên)
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<thienthachdichuyen>().TakeDame(damageResult.damage * 100);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage * 100, damageResult.isCrit);
        }
        if (other.CompareTag("Boss"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            var damageResult = player.CalculateDamage();
            other.GetComponent<BossController>().TakeDame(damageResult.damage * 100);
            DamePopUpGenerator.Instance.CreatePopUp(transform.position, damageResult.damage * 100, damageResult.isCrit);
        }

        // CÂN NHẮC THÊM: Có thể bạn muốn tên lửa nổ và biến mất khi va chạm
        // if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        // {
        //     Destroy(gameObject);
        // }
    }
}