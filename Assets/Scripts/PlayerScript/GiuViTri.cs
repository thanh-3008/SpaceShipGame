using UnityEngine;

public class GiuViTri : MonoBehaviour
{
    private Vector3 initialPosition;

    void Start()
    {
        // Lưu lại vị trí toàn cục ban đầu của object
        initialPosition = transform.position;
    }

    void LateUpdate()
    {
        // Trong mỗi frame, đặt lại vị trí của object về vị trí ban đầu
        // Sử dụng LateUpdate để đảm bảo nó chạy sau khi cha đã di chuyển
        transform.position = initialPosition;
    }
}