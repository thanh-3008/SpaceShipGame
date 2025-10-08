using UnityEngine;

public class spawndancungtrang : MonoBehaviour
{
    // --- Các biến public được giữ lại theo yêu cầu ---
    public GameObject danprefap;
    public AudioManagement audioManager;
    // Biến timer nên là private vì chỉ script này cần dùng đến nó
    private float timer;

    void Start()
    {
        // Vẫn giữ lại phần tìm kiếm an toàn này để tránh lỗi khi bắt đầu
        // Nếu các biến này đã được kéo thả trong Inspector, code này sẽ không chạy
        if (audioManager == null)
        {
            GameObject audioObj = GameObject.FindWithTag("Audio");
            if (audioObj != null)
            {
                audioManager = audioObj.GetComponent<AudioManagement>();
            }
            else
            {
                Debug.LogWarning("Spawner không tìm thấy AudioManagement.", this.gameObject);
            }
        }
   
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Xác định thời gian chờ bắn
        float thoiGianChoHienTai = 0.3f; // Mặc định là 0.2 giây
    
        // Nếu đủ thời gian và được phép bắn
        if (timer >= thoiGianChoHienTai)
        {
            Fire();
            timer = 0f;
        }
    }

    /// <summary>
    /// Logic bắn đạn đã được tách ra một hàm riêng cho gọn gàng.
    /// </summary>
    private void Fire()
    {
        // Kiểm tra null cho prefab đạn
        if (danprefap == null)
        {
            Debug.LogError("Chưa gán Prefab đạn (danprefap) cho Spawner!", this.gameObject);
            return;
        }

        // Kiểm tra null cho audio manager
        if (audioManager != null)
        {
            // Giả sử hàm PlaySfx tồn tại, nếu không hãy đổi thành PlaySfxto
            audioManager.PlaySfxto(audioManager.amthanhdanez);
        }

        // Tạo đạn - đây là cách làm hiệu quả nhất
        Instantiate(danprefap, transform.position, danprefap.transform.rotation);
    }
}
