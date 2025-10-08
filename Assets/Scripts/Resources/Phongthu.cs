using UnityEngine;

public class PhongThu : MonoBehaviour
{
    public float speed = 5f;                 // tốc độ di chuyển tàu
    public GameObject bulletPrefab;          // prefab viên đạn
    public Transform firePoint;              // vị trí bắn đạn
    public float fireRate = 0.25f;           // thời gian giữa 2 phát bắn
    private float nextFireTime = 0f;

    void Update()
    {
        Move();
        Shoot();
    }

    // Điều khiển tàu di chuyển sang trái/phải
    void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");
        // A/D hoặc phím mũi tên trái/phải
        Vector3 movement = new Vector3(moveInput, 0, 0);
        transform.Translate(movement * speed * Time.deltaTime);

        // Giới hạn vùng di chuyển (ví dụ -8 đến 8)
        float xPos = Mathf.Clamp(transform.position.x, -8f, 8f);
        transform.position = new Vector3(xPos, transform.position.y, transform.position.z);
    }

    // Hàm bắn đạn
    void Shoot()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
