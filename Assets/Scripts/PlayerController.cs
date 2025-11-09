using TMPro;
using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour
{
    // ... (các biến của bạn giữ nguyên) ...
    private Rigidbody2D rigidbody2D;
    private float traiphai;
    public float speed = 5f;
    private float lenxuong;
    public bool khoadichuyen = false;

    // Các tham chiếu sẽ được tự động tìm
    public ThanhMau thanhmau;
    public float thanhmauhientai = 100f;
    public float thanhmauToiDa = 100f;

    public TextMeshProUGUI textScore;
    public GameObject danprefap;

    [Header("Chỉ số Tấn Công")]
    public float damehientai => damegoc * damecongthem;
    public float damegoc;
    public float damecongthem = 1f;
    public float damebonus = 1f;

    public TextMeshProUGUI soTenLuaText;
    public GameObject[] spawndan;
    public GameObject[] spawndanpro;
    public AudioManagement audioManager;
    public ThanhNo thanhno;

    public float thanhNoToiDa = 100f;
    public float thanhNoHienTai;
    private SpriteRenderer spriteRenderer;
    public float timeFlash = 0.8f;

    public float gocNghiengToiDa = 15f;    // Góc nghiêng tối đa
    public float tocDoNghieng = 20f;      // Tốc độ nghiêng
    public int score;
    public Boolean kimcangbathoai = false;

    [Header("Update chi so khac")]
    [Tooltip("Tỉ lệ ra đòn chí mạng. Ví dụ: 0.2 là 20%")]
    public float critRate;
    [Tooltip("Hệ số nhân sát thương khi chí mạng. Ví dụ: 1.5 là 150% sát thương")]
    public float critDame;
    [Tooltip("Chỉ số giáp giúp giảm sát thương nhận vào")]
    public float Giap;

    // --- ADDED: Code cho Trợ thủ ---
    [Header("Quan Ly Tro Thu")]
    public GameObject troThu_1; // Kéo object TroThu_1 vào đây
    public GameObject troThu_2; // Kéo object TroThu_2 vào đây

    private TroThuController controller1;
    private TroThuController controller2;

    private float originalMoveSpeed; // Biến lưu tốc độ gốc
    private bool isPermanentlySlowed = false; // Cờ đánh dấu bị làm chậm
                                              // (Bên dưới các biến public/private khác)
    private bool daNhanSatThuongDocFrameNay = false;
    // -------------------------------


    // ... (Hàm Start() và các hàm khác giữ nguyên) ...
    #region Auto-Find and Lifecycle Methods
    public void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMoveSpeed = speed; // Lưu tốc độ gốc

        #region Auto-Find Components
        // --- Tìm ThanhMau ---
        if (thanhmau == null)
        {
            Debug.Log("Searching for 'ThanhMau' component...");
            GameObject obj = GameObject.Find("ThanhMau");
            if (obj != null)
            {
                thanhmau = obj.GetComponent<ThanhMau>();
                if (thanhmau == null)
                {
                    Debug.LogError("GameObject 'ThanhMau' was found, but it's missing the 'ThanhMau' component.");
                }
                else
                {
                    Debug.Log("'ThanhMau' assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'ThanhMau' in the scene.");
            }
        }

        // --- Tìm TextMeshPro cho Score ---
        if (textScore == null)
        {
            Debug.Log("Searching for 'Score' component...");
            GameObject obj = GameObject.Find("Score");
            if (obj != null)
            {
                textScore = obj.GetComponent<TextMeshProUGUI>();
                if (textScore == null)
                {
                    Debug.LogError("GameObject 'Score' was found, but it's missing the 'TextMeshProUGUI' component.");
                }
                else
                {
                    Debug.Log("'Score' text assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'Score' in the scene.");
            }
        }

        // --- Tìm TextMeshPro cho SoTenLua ---
        if (soTenLuaText == null)
        {
            Debug.Log("Searching for 'SoTenLua' component...");
            GameObject obj = GameObject.Find("SoTenLua");
            if (obj != null)
            {
                soTenLuaText = obj.GetComponent<TextMeshProUGUI>();
                if (soTenLuaText == null)
                {
                    Debug.LogError("GameObject 'SoTenLua' was found, but it's missing the 'TextMeshProUGUI' component.");
                }
                else
                {
                    Debug.Log("'SoTenLua' text assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'SoTenLua' in the scene.");
            }
        }

        // --- Tìm các Spawner ---
        if (spawndan == null || spawndan.Length == 0)
        {
            Debug.Log("Searching for GameObjects with tag 'spawndan'...");
            spawndan = GameObject.FindGameObjectsWithTag("spawndan");
            if (spawndan != null && spawndan.Length > 0)
            {
                Debug.Log($"Found and assigned {spawndan.Length} spawner(s).");
            }
            else
            {
                Debug.LogWarning("No GameObjects with the tag 'spawndan' were found in the scene.");
            }
        }

        if (spawndanpro == null || spawndanpro.Length == 0)
        {
            Debug.Log("Searching for GameObjects with tag 'spawndanpro'...");
            spawndanpro = GameObject.FindGameObjectsWithTag("spawndanpro");
            if (spawndanpro != null && spawndanpro.Length > 0)
            {
                Debug.Log($"Found and assigned {spawndanpro.Length} spawner(s).");
            }
            else
            {
                Debug.LogWarning("No GameObjects with the tag 'spawndanpro' were found in the scene.");
            }
        }

        // --- Tìm AudioManagement ---
        if (audioManager == null)
        {
            Debug.Log("Searching for 'AudioManagement' component...");
            GameObject obj = GameObject.Find("AudioManagement");
            if (obj != null)
            {
                audioManager = obj.GetComponent<AudioManagement>();
                if (audioManager == null)
                {
                    Debug.LogError("GameObject 'AudioManagement' was found, but it's missing the 'AudioManagement' component.");
                }
                else
                {
                    Debug.Log("'AudioManagement' assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'AudioManagement' in the scene.");
            }
        }

        // --- Tìm ThanhNo ---
        if (thanhno == null)
        {
            Debug.Log("Searching for 'ThanhNo' component...");
            GameObject obj = GameObject.Find("ThanhNo");
            if (obj != null)
            {
                thanhno = obj.GetComponent<ThanhNo>();
                if (thanhno == null)
                {
                    Debug.LogError("GameObject 'ThanhNo' was found, but it's missing the 'ThanhNo' component.");
                }
                else
                {
                    Debug.Log("'ThanhNo' assigned successfully!");
                }
            }
            else
            {
                Debug.LogError("Could not find GameObject named 'ThanhNo' in the scene.");
            }
        }
        #endregion

        // --- ADDED: Logic khởi tạo Trợ thủ ---
        if (troThu_1 != null)
        {
            controller1 = troThu_1.GetComponent<TroThuController>();
            troThu_1.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Chưa gán 'TroThu_1' vào PlayerController!");
        }

        if (troThu_2 != null)
        {
            controller2 = troThu_2.GetComponent<TroThuController>();
            troThu_2.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Chưa gán 'TroThu_2' vào PlayerController!");
        }
        // ------------------------------------
    }

    void Update()
    {
        if (khoadichuyen == false)
        {
            traiphai = Input.GetAxis("Horizontal");
            lenxuong = Input.GetAxis("Vertical");
        }
        else
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            traiphai = 0; lenxuong = 0;
        }

        rigidbody2D.linearVelocity = new Vector2(traiphai * speed, lenxuong * speed);

        // Hồi thanh nộ
        if (thanhNoHienTai <= thanhNoToiDa * 3)
        {
            thanhNoHienTai += 2f * Time.deltaTime;
            thanhno.capnhatthanhno(thanhNoHienTai, thanhNoToiDa);
        }

        // Xử lý nghiêng tàu khi di chuyển
        float gocMucTieu = -traiphai * gocNghiengToiDa;
        Quaternion gocXoayMucTieu = Quaternion.Euler(0, 0, gocMucTieu);
        transform.rotation = Quaternion.Lerp(transform.rotation, gocXoayMucTieu, tocDoNghieng * Time.deltaTime);
    }
    #endregion

    /// <summary>
    /// Tính toán sát thương gây ra. Trả về một bộ đôi gồm (sát thương cuối cùng, bool có phải chí mạng không).
    /// </summary>
    // << THAY ĐỔI 1: Kiểu trả về của hàm
    public (float damage, bool isCrit) CalculateDamage()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue <= critRate)
        {
            // Là đòn chí mạng
            float finalDamage = damehientai * critDame;
            Debug.Log($"<color=orange>CHÍ MẠNG!</color> Sát thương: {finalDamage}");
            // << THAY ĐỔI 2: Trả về cả sát thương VÀ true
            return (finalDamage, true);
        }
        else
        {
            // Là đòn đánh thường
            // << THAY ĐỔI 3: Trả về cả sát thương VÀ false
            return (damehientai, false);
        }
    }

    public void TakeDame(float dame)
    {
        // Tính toán sát thương giảm bởi giáp
        float damageSauKhiGiamTru = dame * (100f / (100f + Giap));
        // Làm tròn để sát thương trông đẹp hơn (tùy chọn)
        damageSauKhiGiamTru = Mathf.Round(damageSauKhiGiamTru);

        if (kimcangbathoai == true)
        {
            float reducedDamage = damageSauKhiGiamTru / 4;
            Debug.Log("Kim cang bất hoại giảm dame: " + reducedDamage);
            thanhmauhientai -= reducedDamage;
            DamePopUpGenerator.Instance.CreateHealthLossPopUp(transform.position, reducedDamage);
        }
        else
        {
            Debug.Log($"Sát thương gốc: {dame}, Giáp: {Giap}, Sát thương nhận vào: {damageSauKhiGiamTru}");
            thanhmauhientai -= damageSauKhiGiamTru;
            StartFlashRed();
            DamePopUpGenerator.Instance.CreateHealthLossPopUp(transform.position, damageSauKhiGiamTru);
        }

        thanhmau.capnhatthanhmau(thanhmauhientai, thanhmauToiDa);
        audioManager.PlaySfxto(audioManager.tiengvacham);

        if (thanhmauhientai <= 0)
        {
            if (thanhmauhientai <= 0)
            {
                // --- THÊM MỚI: LƯU TỔNG TIỀN ---
                int finalScore = score; // Lấy điểm của lần chơi này

                // Lấy tổng tiền đã lưu
                int totalCurrency = PlayerPrefs.GetInt("TotalCurrency", 0);

                // Cộng dồn điểm vừa chơi vào
                totalCurrency += finalScore;

                // Lưu tổng tiền mới lại
                PlayerPrefs.SetInt("TotalCurrency", totalCurrency);
                PlayerPrefs.Save(); // Lưu ngay lập tức
                                    // ---------------------------------

                // Gọi màn hình Game Over
                FindObjectOfType<GameOverMenu>().showGameOverScreen(finalScore);
            }
        }
    }

    // ... (Các hàm còn lại giữ nguyên) ...
    #region Other Methods
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int sotenlua = int.Parse(soTenLuaText.text);

        if (collision.CompareTag("star"))
        {
            CongDiem(1);
            damegoc += damebonus;
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("bufftenlua") && sotenlua <= 9)
        {
            sotenlua += 1;
            soTenLuaText.text = sotenlua.ToString();
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("buffdanpro"))
        {
            Debug.Log("an buff dan pro");
            foreach (GameObject spawner in spawndanpro)
            {
                spawndanpro danproSpawner = spawner.GetComponent<spawndanpro>();
                danproSpawner.ActivateBuff(10f);
            }
            audioManager.PlaySfxto(audioManager.tiengancoin);
            Destroy(collision.gameObject);
        }
    }
    private void UpdateScoreUI()
    {
        if (textScore != null)
        {
            // Cập nhật text
            textScore.text = score.ToString();

            // Gợi ý: Tương lai bạn có thể thêm hiệu ứng nảy chữ hoặc đổi màu ở đây
            // Ví dụ: textScore.transform.localScale = Vector3.one * 1.2f;
        }
    }
    public void CongDiem(int diem)
    {
        // Kiểm tra an toàn, không cộng điểm âm
        if (diem <= 0) return;

        // 1. Cộng điểm vào biến dữ liệu
        score += diem;

        // 2. Gọi hàm để cập nhật giao diện
        UpdateScoreUI();

        // 3. Tạo phản hồi cho người chơi (âm thanh)
        if (audioManager != null)
        {
            audioManager.PlaySfxto(audioManager.tiengancoin);
        }
    }
    public void StartFlashRed()
    {
        StopCoroutine("FlashRedCoroutine"); // Dừng coroutine cũ nếu đang chạy
        StartCoroutine(FlashRedCoroutine(timeFlash));
    }

    // --- HÀM NÀY ĐÃ ĐƯỢC CẬP NHẬT ---
    private IEnumerator FlashRedCoroutine(float thoigianduytri)
    {
        float elapsedTime = 0f;

        // 1. Xác định màu gốc (base color) dựa trên trạng thái bị làm chậm
        Color baseColor = isPermanentlySlowed ? Color.green : Color.white;

        while (elapsedTime < thoigianduytri)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);

            // 2. Quay trở lại màu gốc (thay vì luôn là màu trắng)
            spriteRenderer.color = baseColor;

            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.2f;
        }

        // 3. Đảm bảo khi kết thúc, màu sắc trở lại đúng trạng thái
        spriteRenderer.color = baseColor;
    }

    void LateUpdate()
    {
        Vector3 currentPosition = transform.position;
        float clampedX = Mathf.Clamp(currentPosition.x, -7.5f, 7.5f);
        float clampedY = Mathf.Clamp(currentPosition.y, -4.5f, 4.5f);
        transform.position = new Vector3(clampedX, clampedY, currentPosition.z);
        daNhanSatThuongDocFrameNay = false;
    }

    // --- ADDED: BA HAM MOI DE UPGRADEMANAGEMENT GOI ---

    // Lần đầu chọn skill: Bật con trợ thủ 1 lên
    public void KichHoatTroThu()
    {
        if (troThu_1 != null)
        {
            troThu_1.SetActive(true);
        }
    }

    // Những lần sau: Nâng cấp cả 2 con nếu chúng đang bật
    public void NangCapTroThu()
    {
        if (controller1 != null && troThu_1.activeSelf)
        {
            controller1.NangCapTroThu();
        }
        if (controller2 != null && troThu_2.activeSelf)
        {
            controller2.NangCapTroThu();
        }
    }

    // Nâng cấp cuối: Bật con trợ thủ 2 và cho nó level = con 1
    public void KichHoatTroThuCuoi()
    {
        if (troThu_2 != null && controller1 != null && controller2 != null)
        {
            troThu_2.SetActive(true);
            // Copy level của con 1 cho con 2
            controller2.SetLevel(controller1.GetCurrentLevel());
        }
    }

    // --- HÀM NÀY ĐÃ ĐƯỢC CẬP NHẬT ---
    public void ApplySlow(float slowFactor)
    {
        // Nếu đã bị chậm rồi, không cần chạy lại
        if (isPermanentlySlowed) return;

        isPermanentlySlowed = true;
        // Tốc độ gốc đã được lưu ở Start()
        speed = originalMoveSpeed * slowFactor;
        Debug.Log("Player bị LÀM CHẬM vĩnh viễn!");

        // --- CODE MỚI ---
        if (spriteRenderer != null)
        {
            // Dừng mọi hiệu ứng nháy đỏ (nếu có) và chuyển sang màu xanh
            StopCoroutine("FlashRedCoroutine");
            spriteRenderer.color = Color.green;
        }
        // ---------------
    }

    // --- HÀM NÀY ĐÃ ĐƯỢC CẬP NHẬT ---
    public void RemoveSlow()
    {
        // Chỉ hồi phục nếu đang bị chậm
        if (!isPermanentlySlowed) return;

        Debug.Log("Player hồi phục tốc độ.");
        speed = originalMoveSpeed;
        isPermanentlySlowed = false;

        // --- CODE MỚI ---
        if (spriteRenderer != null)
        {
            // Dừng mọi hiệu ứng nháy đỏ (nếu có) và trở về màu trắng
            StopCoroutine("FlashRedCoroutine");
            spriteRenderer.color = Color.white;
        }
        // ---------------
    }

    /// <summary>
    /// Hàm này được gọi bởi các vũng độc (VenomTrail).
    /// Nó kiểm tra để đảm bảo Player chỉ nhận sát thương độc 1 lần/frame.
    /// </summary>
    public void TakePoisonDamage(float damage)
    {
        // Nếu frame này đã nhận sát thương độc rồi, thì BỎ QUA
        if (daNhanSatThuongDocFrameNay)
        {
            return;
        }

        // Nếu chưa, thì nhận sát thương (và phát âm thanh 1 LẦN)
        TakeDame(damage); // Gọi hàm TakeDame gốc của bạn

        // Và đặt cờ, để các vũng độc khác không thể gây sát thương
        // trong frame này nữa
        daNhanSatThuongDocFrameNay = true;
    }
    // ---------------------------------------------------

    #endregion
}