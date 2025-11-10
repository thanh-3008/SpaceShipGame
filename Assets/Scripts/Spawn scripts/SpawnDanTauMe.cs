using UnityEngine;
using System.Collections;

public class SpawnDanTauMe : MonoBehaviour
{
    public GameObject dan;
    private float thoigianspawn;
    private float thoigianbatdauspawn;
    public float thoigiancachnhau;

    // THÊM BIẾN NÀY ĐỂ CHỈNH LỰC ĐẨY TRONG EDITOR
    [Tooltip("Lực đẩy ban đầu của viên đạn. Yêu cầu prefab 'dan' phải có Rigidbody 2D.")]
    public float lucDayCuaDan = 10f;

    void Start()
    {
        thoigianbatdauspawn = 5f;
        thoigianspawn = 0f;
    }

    void Update()
    {
        thoigianbatdauspawn -= Time.deltaTime;
        thoigiancachnhau += Time.deltaTime;

        if (thoigianbatdauspawn <= 0 && thoigianspawn <= 10f && thoigiancachnhau > 0.1f)
        {
            AudioManagement audio = FindAnyObjectByType<AudioManagement>();
            audio.PlaySfx(audio.tiengdan);

            // --- PHẦN CHỈNH SỬA ---
            // 1. Tạo ra viên đạn và lưu nó vào một biến tạm
            GameObject danMoi = Instantiate(dan, transform.position, dan.transform.rotation);

            // 2. Lấy component Rigidbody2D từ viên đạn vừa tạo
            Rigidbody2D rb = danMoi.GetComponent<Rigidbody2D>();

            // 3. Nếu có Rigidbody2D, thêm một lực đẩy về phía trước (transform.up)
            if (rb != null)
            {
                // Dùng AddForce với ForceMode2D.Impulse để tạo một lực đẩy tức thời
                rb.AddForce(transform.up * lucDayCuaDan, ForceMode2D.Impulse);
            }
            else
            {
                // Cảnh báo nếu bạn quên thêm Rigidbody 2D
                Debug.LogWarning("Prefab 'dan' thiếu Rigidbody2D! Không thể thêm lực đẩy.");
            }
            // --- HẾT PHẦN CHỈNH SỬA ---

            thoigiancachnhau = 0f;
        }
        thoigianspawn += Time.deltaTime;
    }
}