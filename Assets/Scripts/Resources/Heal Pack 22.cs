using UnityEngine;

public class HealPack22 : MonoBehaviour
{
    [SerializeField]
    private int healAmount = 30;  // Số máu hồi
    [SerializeField]
    private float rotationSpeed = 50f; // Quay cho đẹp

    private void Update()
    {
        // Quay vòng tròn cho dễ thấy
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Kiểm tra nếu đối tượng chạm vào là Player
        if (collision.CompareTag("Player"))
        {
            PlayerHealth22 playerHealth = collision.GetComponent<PlayerHealth22>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
            }

            // Hiệu ứng & biến mất
            Destroy(gameObject);
        }
    }
}
