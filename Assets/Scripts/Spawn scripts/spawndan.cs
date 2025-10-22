using System;
using UnityEngine;

public class spawndan : MonoBehaviour
{
    [Header("Cài đặt cơ bản")]
    public GameObject danprefap;       // Kéo Prefab của viên đạn vào đây
    public Transform diemBan;          // Vị trí đầu nòng súng (QUAN TRỌNG)
    public bool isshot = true;
    public AudioManagement audioManager;
    public SeraphMKII skillMKII;
    public float thoiGianChoGoc = 0.6f; // Đổi tên để rõ ràng hơn, đây là thời gian chờ gốc
    public bool isLastUpgrade = false;
    public int soLanChonNangCap = 0;
    [Header("Thông số vũ khí thông minh")]
    [Range(1, 50)]
    public int soDan = 1;

    [Tooltip("Tổng góc tỏa ra tối đa của loạt đạn")]
    public float gocToaToiDa = 45f;

    [Tooltip("Mỗi viên đạn (sau viên đầu tiên) sẽ cộng thêm bao nhiêu độ vào tổng góc bắn")]
    public float gocTangMoiVienDan = 5f;
   

    public float lucBan = 20f;

    // Biến private để xử lý nội bộ
    private float timer;

    void Start()
    {
        // Nếu không gán điểm bắn, mặc định lấy vị trí của chính spawner này
        if (diemBan == null)
        {
            diemBan = this.transform;
        }

        // --- Phần tìm kiếm an toàn vẫn được giữ lại ---
        if (audioManager == null)
        {
            GameObject audioObj = GameObject.FindWithTag("Audio");
            if (audioObj != null) audioManager = audioObj.GetComponent<AudioManagement>();
            else Debug.LogWarning("Spawner không tìm thấy AudioManagement.", this.gameObject);
        }

        if (skillMKII == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null) skillMKII = playerObj.GetComponent<SeraphMKII>();
            else Debug.LogWarning("Spawner không tìm thấy Player.", this.gameObject);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        // Xác định thời gian chờ bắn thực tế
        float thoiGianChoHienTai = thoiGianChoGoc;

        // Kiểm tra an toàn và áp dụng hiệu ứng từ skill
        if (skillMKII != null && skillMKII.lamchamthoigian == true)
        {
            // Khi skill được kích hoạt, tốc độ bắn nhanh hơn (thời gian chờ giảm đi)
            thoiGianChoHienTai /= 2f;
        }

        // Nếu đủ thời gian và được phép bắn
        if (timer >= thoiGianChoHienTai && isshot)
        {
            Fire();
            timer = 0f;
        }
    }

    /// <summary>
    /// Logic bắn đạn thông minh, được tích hợp từ script BanDanThongMinh.
    /// </summary>
    private void Fire()
    {
        // --- CÁC BƯỚC KIỂM TRA VÀ CHUẨN BỊ ---
        if (danprefap == null)
        {
            Debug.LogError("Chưa gán Prefab đạn (danprefap) cho Spawner!", this.gameObject);
            return;
        }

        if (audioManager != null)
        {
            audioManager.PlaySfx(audioManager.tiengdan);
        }

        // Tính toán hướng bắn cơ bản về phía chuột
        Vector2 viTriChuot = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 huongCoBan = (viTriChuot - (Vector2)diemBan.position).normalized;
        float gocCoBan = Mathf.Atan2(huongCoBan.y, huongCoBan.x) * Mathf.Rad2Deg;

        // --- XỬ LÝ TỪNG TRƯỜNG HỢP THEO SỐ LƯỢNG ĐẠN ---

        // TRƯỜNG HỢP 1: BẮN 1 VIÊN
        if (soDan == 1)
        {
            TaoRaDan(diemBan.position, huongCoBan, gocCoBan);
            return;
        }

        

        // TRƯỜNG HỢP 3: BẮN TỎA RA (CHO 3 VIÊN TRỞ LÊN)
        if (soDan >= 2)
        {
            float tongGocHienTai = Mathf.Min(gocToaToiDa, (soDan - 1) * gocTangMoiVienDan);
            float buocNhayGoc = tongGocHienTai / (soDan - 1);
            float gocBatDau = -tongGocHienTai / 2;

            for (int i = 0; i < soDan; i++)
            {
                float gocHienTai = gocBatDau + i * buocNhayGoc;
                Vector2 huongDaXoay = Quaternion.Euler(0, 0, gocHienTai) * huongCoBan;
                float gocXoayDan = Mathf.Atan2(huongDaXoay.y, huongDaXoay.x) * Mathf.Rad2Deg;
                TaoRaDan(diemBan.position, huongDaXoay, gocXoayDan);
            }
        }
    }

    /// <summary>
    /// Hàm phụ trợ, chỉ có một nhiệm vụ là tạo ra một viên đạn, gán tốc độ và xoay nó đúng hướng.
    /// </summary>
    void TaoRaDan(Vector3 viTriSpawn, Vector2 huong, float gocXoay)
    {
        // Tạo viên đạn và xoay hình ảnh của nó cho đúng hướng
        // Thường phải trừ 90 độ nếu hình ảnh viên đạn của bạn có chiều mặc định là hướng lên trên
        GameObject dan = Instantiate(danprefap, viTriSpawn, Quaternion.Euler(0, 0, gocXoay ));

        if (isLastUpgrade)
        {
            SpriteRenderer renderer = dan.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.color = Color.red;
            }
        }
        // Lấy Rigidbody2D và tác dụng lực để bắn đi
        Rigidbody2D rb = dan.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(huong * lucBan, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogWarning("Prefab đạn không có Rigidbody2D!", dan);
        }
    }

    public void NangCapThemDan()
    {
        thoiGianChoGoc -= 0.05f;
        soLanChonNangCap++;
        if(soLanChonNangCap % 2 == 0)
        {
            soDan++;
        }
    }
    public void NangCapCuoi()
    {
        isLastUpgrade = true;
        soDan = 5;
    }
}