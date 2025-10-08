using UnityEngine;
using System.Collections.Generic;

// Đặt tên class trùng với tên file script
public class HoDenController : MonoBehaviour
{
    // ----- CÀI ĐẶT LỰC HÚT -----
    [Header("Cài Đặt Lực Hút")]
    [Tooltip("Sức mạnh của lực hấp dẫn. Giá trị càng cao, hút càng mạnh.")]
    public float lucHapDan = 100f;

    [Tooltip("Bán kính mà tại đó Enemy sẽ bị 'giữ lại'. Nên để giá trị này nhỏ.")]
    public float banKinhGiuLai = 0.5f; // Đổi tên từ banKinhNuotChung để rõ nghĩa hơn

    // ----- CÀI ĐẶT TĂNG TRƯỞNG -----
    [Header("Cài Đặt Tăng Trưởng")]
    [Tooltip("Tốc độ hố đen lớn dần. Đặt là 0 nếu không muốn hố đen tự lớn.")]
    public float tocDoPhatTrien = 0.1f;

    [Tooltip("Kích thước tối đa mà hố đen có thể đạt tới. Đặt là 0 nếu không muốn có giới hạn.")]
    public float kichThuocToiDa = 5f;

    // Danh sách lưu trữ các vật thể đang bị hút
    private List<Rigidbody2D> danhSachVatTheBiHut = new List<Rigidbody2D>();

    // Hàm được gọi một lần mỗi frame, dùng cho logic không liên quan đến vật lý
    void Update()
    {
        XuLyPhatTrien();
    }

    // Hàm được gọi mỗi khung hình vật lý, dùng cho các tính toán vật lý
    void FixedUpdate()
    {
        ApDungLucHut();
    }

    /// <summary>
    /// Xử lý logic lớn dần của hố đen.
    /// </summary>
    void XuLyPhatTrien()
    {
        if (tocDoPhatTrien <= 0) return;
        if (kichThuocToiDa > 0 && transform.localScale.x >= kichThuocToiDa)
        {
            return;
        }
        float luongPhatTrien = tocDoPhatTrien * Time.deltaTime;
        Vector3 tangKichThuoc = new Vector3(luongPhatTrien, luongPhatTrien, 0);
        transform.localScale += tangKichThuoc;
    }

    /// <summary>
    /// Tác động lực hút lên tất cả các vật thể trong vùng ảnh hưởng.
    /// </summary>
    void ApDungLucHut()
    {
        for (int i = danhSachVatTheBiHut.Count - 1; i >= 0; i--)
        {
            Rigidbody2D vatThe = danhSachVatTheBiHut[i];
            if (vatThe == null)
            {
                danhSachVatTheBiHut.RemoveAt(i);
                continue;
            }

            float khoangCach = Vector2.Distance(vatThe.position, (Vector2)transform.position);

            // ----- THAY ĐỔI 2: Logic "Giữ Lại" thay vì "Nuốt Chửng" -----
            if (khoangCach < banKinhGiuLai)
            {
                // Vô hiệu hóa vận tốc và gia tốc để ngăn quán tính
                vatThe.linearVelocity = Vector2.zero;
                vatThe.angularVelocity = 0f;

                // Giữ chặt đối tượng tại tâm của hố đen
                vatThe.position = transform.position;

                // Không cần tác động lực hút nữa vì đã giữ lại rồi
                continue;
            }

            // --- Logic "Hút" ---
            Vector2 huongHut = (Vector2)transform.position - vatThe.position;
            huongHut.Normalize();
            float doLonLucHut = (lucHapDan * vatThe.mass) / (khoangCach * khoangCach);
            vatThe.AddForce(huongHut * doLonLucHut);
        }
    }

    // Được gọi khi một vật thể (có Rigidbody2D) đi vào vùng trigger
    private void OnTriggerEnter2D(Collider2D doiTuongKhac)
    {
        // ----- THAY ĐỔI 1: Chỉ kiểm tra đối tượng có tag "Enemy" -----
        if (doiTuongKhac.CompareTag("Enemy"))
        {
            Rigidbody2D vatThe = doiTuongKhac.GetComponent<Rigidbody2D>();
            // Chỉ thêm vào danh sách nếu nó có Rigidbody2D và chưa có trong danh sách
            if (vatThe != null && !danhSachVatTheBiHut.Contains(vatThe))
            {
                danhSachVatTheBiHut.Add(vatThe);
                Debug.Log("Enemy '" + doiTuongKhac.name + "' đã đi vào vùng hấp dẫn.");
            }
        }
    }

    // Được gọi khi một vật thể rời khỏi vùng trigger
    private void OnTriggerExit2D(Collider2D doiTuongKhac)
    {
        // Cũng nên kiểm tra tag ở đây để logic nhất quán
        if (doiTuongKhac.CompareTag("Enemy"))
        {
            Rigidbody2D vatThe = doiTuongKhac.GetComponent<Rigidbody2D>();
            if (vatThe != null && danhSachVatTheBiHut.Contains(vatThe))
            {
                danhSachVatTheBiHut.Remove(vatThe);
                Debug.Log("Enemy '" + doiTuongKhac.name + "' đã thoát khỏi vùng hấp dẫn.");
            }
        }
    }
}