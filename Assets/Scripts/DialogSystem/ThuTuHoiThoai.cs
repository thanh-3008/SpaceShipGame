using UnityEngine;

public class ThuTuHoiThoai : MonoBehaviour
{
    [Tooltip("Kéo đối tượng có script SpawnMonster vào đây")]
    public SpawnMonster spawnMonster;

    [Header("Danh sách hội thoại")]
    public Conversation HoiThoaiBatDau;
    public Conversation gapBoss1;
    public Conversation tieuDietBoss1;
    // Thêm hội thoại cho các boss khác ở đây (ví dụ: gapBoss2, tieuDietBoss2...)

    // --- (THÊM MỚI) ---
    [Header("Sự Kiện Hẹn Giờ")]
    [Tooltip("Hội thoại sẽ tự động chạy lúc 14 phút 55 giây")]
    public Conversation hoiThoaiPhut14_55;

    private float gameTimer = 0f;
    private bool daKichHoatSuKien1455 = false;
    // 14 phút * 60 giây + 55 giây = 895 giây
    private const float THOI_GIAN_KICH_HOAT = 895f;
    // ------------------

    void Start()
    {
        // Chạy hội thoại đầu game
        if (HoiThoaiBatDau != null)
        {
            HoiThoaiManagement.instance.StartHoiThoai(HoiThoaiBatDau);
        }

        // Tự tìm SpawnMonster nếu chưa kéo vào
        if (spawnMonster == null)
        {
            spawnMonster = FindObjectOfType<SpawnMonster>();
        }

        // Đăng ký lắng nghe các sự kiện từ SpawnMonster
        if (spawnMonster != null)
        {
            spawnMonster.OnBossSpawned += HandleBossSpawn;
            spawnMonster.OnBossDefeated += HandleBossDefeat;
        }
        else
        {
            Debug.LogError("CHƯA KẾT NỐI VỚI SPAWNMONSTER!", this.gameObject);
        }
    }

    // --- (THÊM MỚI: HÀM UPDATE ĐỂ ĐẾM GIỜ) ---
    void Update()
    {
        // Chỉ đếm giờ nếu sự kiện CHƯA được kích hoạt
        if (daKichHoatSuKien1455) return;

        // (Giả sử game không bị pause, nếu có pause thì cần dùng Time.unscaledDeltaTime)
        gameTimer += Time.deltaTime;

        if (gameTimer >= THOI_GIAN_KICH_HOAT)
        {
            daKichHoatSuKien1455 = true;
            Debug.Log("Đã đạt 14:55! Kích hoạt hội thoại cảnh báo.");

            // Kiểm tra xem đã gán hội thoại vào chưa
            if (hoiThoaiPhut14_55 != null)
            {
                HoiThoaiManagement.instance.StartHoiThoai(hoiThoaiPhut14_55);
            }
            else
            {
                Debug.LogWarning("Đã đến 14:55 nhưng 'hoiThoaiPhut14_55' chưa được gán!");
            }
        }
    }
    // ------------------------------------------

    // Hàm này sẽ được tự động gọi khi SpawnMonster phát tín hiệu OnBossSpawned
    private void HandleBossSpawn(int bossIndex)
    {
        // bossIndex là vị trí của boss trong list bossesToSpawn (bắt đầu từ 0)
        if (bossIndex == 0) // Boss 1
        {
            HoiThoaiManagement.instance.StartHoiThoai(gapBoss1);
        }
    }

    // Hàm này sẽ được tự động gọi khi SpawnMonster phát tín hiệu OnBossDefeated
    private void HandleBossDefeat(int bossIndex)
    {
        if (bossIndex == 0) // Boss 1
        {
            HoiThoaiManagement.instance.StartHoiThoai(tieuDietBoss1);
        }
    }

    // Rất quan trọng: Hủy đăng ký khi đối tượng này bị phá hủy
    void OnDestroy()
    {
        if (spawnMonster != null)
        {
            spawnMonster.OnBossSpawned -= HandleBossSpawn;
            spawnMonster.OnBossDefeated -= HandleBossDefeat;
        }
    }
}