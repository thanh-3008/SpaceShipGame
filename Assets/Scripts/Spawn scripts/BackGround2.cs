using UnityEngine;

public class BackGround2 : MonoBehaviour
{
    // Tốc độ mà nền cuộn xuống.
    [Tooltip("Tốc độ mà nền cuộn xuống.")]
    public float scrollSpeed = 0.5f;

    // Chiều cao của ảnh nền. Chúng ta sẽ tự động tính toán giá trị này.
    public float backgroundHeight;

    // Vị trí bắt đầu của tấm nền này.
    private Vector3 startPosition;

    void Start()
    {
        // Lưu lại vị trí ban đầu của đối tượng background này.
        startPosition = transform.position;

        // Tính toán chiều cao của background dựa trên component SpriteRenderer.
        // Điều này giúp script tự động hoạt động với mọi kích thước ảnh.
        backgroundHeight = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update()
    {
        // Di chuyển background xuống dưới trong mỗi frame.
        // Time.deltaTime giúp chuyển động mượt mà và không phụ thuộc vào frame rate.
        transform.Translate(Vector2.down * scrollSpeed * Time.deltaTime);

        // Kiểm tra xem background đã di chuyển hoàn toàn ra khỏi màn hình chưa.
        // Chúng ta làm điều này bằng cách xem nó đã di chuyển xuống một khoảng bằng chiều cao của chính nó chưa.
        if (transform.position.y < startPosition.y - backgroundHeight *2)
        {
            // Nếu rồi, dịch chuyển nó lên trên một khoảng bằng hai lần chiều cao.
            // Điều này sẽ đặt nó ngay phía trên tấm nền còn lại, tạo ra một vòng lặp liền mạch.
            transform.position += new Vector3(0, backgroundHeight * 2f, 0);
        }
    }
}
