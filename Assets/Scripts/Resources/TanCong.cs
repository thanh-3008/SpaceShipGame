using UnityEngine;

public class TanCong : MonoBehaviour
{
    public float bulletSpeed = 10f;       // tốc độ bay của đạn
    public float lifeTime = 3f;           // thời gian tồn tại trước khi tự hủy
    public int damage = 1;                // sát thương gây ra cho kẻ địch

    void Start()
    {
        // Tự hủy đạn sau một khoảng thời gian
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Đạn bay lên theo trục Y
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
    }

    
}
