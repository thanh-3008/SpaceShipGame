using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
    // Các đường cong để điều khiển hoạt ảnh
    public AnimationCurve opacityCurve; // Điều khiển độ mờ
    public AnimationCurve scaleCurve;   // Điều khiển kích thước
    public AnimationCurve heightCurve;  // Điều khiển độ cao bay lên

    public TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin; // Vị trí ban đầu

    // Thời gian tồn tại của popup
    public float duration = 1.5f;

    void Awake()
    {
        // Lấy component TextMeshPro từ chính đối tượng này
        
        origin = transform.position;
    }

    // Hàm này sẽ được gọi từ script khác để khởi tạo popup
    public void Setup(int damageAmount, bool isCriticalHit)
    {
        // 1. Hiển thị số sát thương
        tmp.text = damageAmount.ToString();

        // 2. Thiết lập màu sắc dựa trên loại sát thương
        if (isCriticalHit)
        {
            tmp.color = Color.red; // Hoặc một màu cam/vàng đậm
            tmp.fontSize *= 1.2f; // Tăng cỡ chữ cho crit
        }
        else
        {
            tmp.color = Color.white;
        }
    }

    void Update()
    {
        // Tăng biến thời gian
        time += Time.deltaTime;

        // --- ÁP DỤNG CÁC HIỆU ỨNG ---

        // 1. Hiệu ứng độ mờ (Opacity)
        float opacity = opacityCurve.Evaluate(time );
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacity);

        // 2. Hiệu ứng kích thước (Scale)
        float scale = scaleCurve.Evaluate(time);
        transform.localScale = Vector3.one * scale;

        // 3. Hiệu ứng bay lên (Height)
        float height = heightCurve.Evaluate(time );
        transform.position = origin + new Vector3(0, height, 0);

        // --- TỰ HỦY ĐỐI TƯỢNG ---
        if (time > duration)
        {
            // Sau khi hết thời gian, hủy đối tượng popup
            Destroy(gameObject);
        }
    }
}