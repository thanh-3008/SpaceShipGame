using UnityEngine;

public class spawndancungtrang : MonoBehaviour
{
    // --- Các biến public được giữ lại theo yêu cầu ---
    public GameObject danprefap;
    public AudioManagement audioManager;
    public Transform diemBan;
    public float lucBan = 5f;
    public float tocdobangoc;
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

        float tocdobanhientai = tocdobangoc;

        // Nếu đủ thời gian và được phép bắn
        if (timer >= tocdobanhientai)
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
        // Tính toán hướng bắn cơ bản về phía chuột
        Vector2 viTriChuot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 huongCoBan = (viTriChuot - (Vector2)diemBan.position).normalized;
        float gocCoBan = Mathf.Atan2(huongCoBan.y, huongCoBan.x) * Mathf.Rad2Deg;
        // Tạo đạn - đây là cách làm hiệu quả nhất
        GameObject danez=Instantiate(danprefap, diemBan.position, Quaternion.Euler(0,0,gocCoBan));
        Rigidbody2D rb = danez.GetComponent<Rigidbody2D>();
        rb.AddForce(huongCoBan * lucBan, ForceMode2D.Impulse);
    }
}
