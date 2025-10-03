using UnityEngine;

// Đặt tên class trùng với tên file script: BlackHoleController
public class BlackHoleController : MonoBehaviour
{
    // --- BIẾN CÓ THỂ CHỈNH SỬA TRONG UNITY EDITOR ---

    [Tooltip("Tốc độ hố đen lớn dần. Giá trị càng cao, hố đen càng lớn nhanh.")]
    public float growthRate ; // Bạn có thể thay đổi giá trị này trong Inspector

    [Tooltip("Kích thước tối đa mà hố đen có thể đạt tới. Đặt là 0 nếu không muốn có giới hạn.")]
    public float maxScale = 5f;


    // --- HÀM CỦA UNITY ---

    // Hàm Update() được gọi một lần mỗi frame
    void Update()
    {
        // Kiểm tra xem hố đen đã đạt kích thước tối đa chưa
        // Nếu maxScale <= 0, tức là không có giới hạn, điều kiện này sẽ luôn sai và hố đen sẽ lớn mãi
        if (maxScale > 0 && transform.localScale.x >= maxScale)
        {
            return; // Dừng việc lớn lên nếu đã đạt kích thước tối đa
        }

        // Tính toán lượng scale sẽ tăng lên trong frame này
        // Nhân với Time.deltaTime để đảm bảo tốc độ tăng trưởng mượt mà,
        // không phụ thuộc vào cấu hình máy tính (FPS cao hay thấp)
        float growthAmount = growthRate * Time.deltaTime;

        // Tạo một Vector3 mới để cộng vào scale hiện tại
        // Chúng ta tăng cả trục x và y để hố đen lớn lên một cách đồng đều
        Vector3 scaleIncrease = new Vector3(growthAmount, growthAmount, 0);

        // Cộng giá trị vừa tính vào scale hiện tại của GameObject
        transform.localScale += scaleIncrease;
    }

    // --- XỬ LÝ VA CHẠM (TRIGGER) ---

    // Hàm này sẽ tự động được gọi khi một đối tượng khác (có Rigidbody2D) đi vào vùng trigger của hố đen
    private void OnTriggerEnter2D(Collider2D other)
    {
        // In ra Console để kiểm tra xem vật thể nào đã va chạm
        Debug.Log("Đối tượng '" + other.gameObject.name + "' đã bị hút vào hố đen!");

        // --- Thêm logic của bạn tại đây ---
        // Ví dụ: Phá hủy đối tượng đó
        // Bạn có thể kiểm tra tag của đối tượng trước khi phá hủy
        // if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        // {
        //     Destroy(other.gameObject);
        // }

        
    }
}