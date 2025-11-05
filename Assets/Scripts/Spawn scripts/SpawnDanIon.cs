using UnityEngine;

public class SpawnDanIon : MonoBehaviour
{
    // Kéo các đối tượng vào đây trong Inspector
    public GameObject DanIon; // Prefab của đạn (PHẢI CÓ Rigidbody hoặc Rigidbody2D)
    public PlayerController player;
    public float projectileSpeed = 20f; // Tốc độ bắn của đạn

    private Camera mainCamera; // Dùng để chuyển đổi tọa độ chuột

    public void Start()
    {
        GameObject playergoj = GameObject.FindGameObjectWithTag("Player");
        if (playergoj != null)
        {
            player = playergoj.GetComponent<PlayerController>();
        }

        // Lấy camera chính
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        // Kiểm tra Player
        if (player == null)
        {
            Debug.LogError("Chưa gán Player vào script SpawnDanIon!");
            return;
        }

        // Kiểm tra Camera
        if (mainCamera == null)
        {
            Debug.LogError("Không tìm thấy Camera chính (MainCamera)!");
            return;
        }

        // Kiểm tra thanh nộ
        if (player.thanhNoHienTai >= player.thanhNoToiDa)
        {
            // --- LOGIC BẮN VỀ PHÍA CHUỘT ---

            // 1. Lấy vị trí chuột (Screen Space)
            Vector3 mouseScreenPos = Input.mousePosition;

            // 2. Chuyển vị trí chuột sang World Space
            // Chúng ta cần cho camera biết vị trí Z của mặt phẳng game
            Vector3 spawnPosition = transform.position;
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(
                mouseScreenPos.x,
                mouseScreenPos.y,
                mainCamera.transform.position.z - spawnPosition.z
            ));

            // *Fix cho 2D/Top-down: Đảm bảo Z của chuột = Z của điểm spawn
            mouseWorldPos.z = spawnPosition.z;

            // 3. Tính toán hướng bắn (từ điểm spawn TỚI chuột)
            Vector3 direction = (mouseWorldPos - spawnPosition).normalized;

            // 4. (Tùy chọn) Xoay viên đạn để nó hướng về phía chuột
            // Tính góc xoay (quanh trục Z, phổ biến cho 2D/Top-down)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            // 5. Tạo viên đạn
            GameObject projectile = Instantiate(DanIon, spawnPosition, rotation);

            // 6. AddForce cho viên đạn
            // Kiểm tra xem prefab dùng Rigidbody 2D hay 3D
            Rigidbody2D rb2D = projectile.GetComponent<Rigidbody2D>();
            if (rb2D != null)
            {
                // Dùng Rigidbody2D
                rb2D.AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
            }
            else
            {
                Rigidbody rb3D = projectile.GetComponent<Rigidbody>();
                if (rb3D != null)
                {
                    // Dùng Rigidbody 3D
                    rb3D.AddForce(direction * projectileSpeed, ForceMode.Impulse);
                }
                else
                {
                    Debug.LogError("Prefab 'DanIon' không có Rigidbody hoặc Rigidbody2D!");
                }
            }

            // --- KẾT THÚC LOGIC MỚI ---

            // Reset thanh nộ
            player.thanhNoHienTai -= 100f;

            // (Nên có) Cập nhật lại UI thanh nộ nếu có
            // player.UpdateRageUI(); 
        }
    }
}