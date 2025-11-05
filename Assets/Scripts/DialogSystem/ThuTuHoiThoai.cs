using UnityEngine;

public class ThuTuHoiThoai : MonoBehaviour
{
    [Tooltip("Kéo đối tượng có script SpawnMonster vào đây")]
    public SpawnMonster spawnMonster; // <-- THÊM BIẾN NÀY

    [Header("Danh sách hội thoại")]
    public Conversation HoiThoaiBatDau;
    public Conversation gapBoss1;
    public Conversation tieuDietBoss1;
    // Thêm hội thoại cho các boss khác ở đây (ví dụ: gapBoss2, tieuDietBoss2...)

    void Start()
    {
        // Chạy hội thoại đầu game
        HoiThoaiManagement.instance.StartHoiThoai(HoiThoaiBatDau);

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

    // Hàm này sẽ được tự động gọi khi SpawnMonster phát tín hiệu OnBossSpawned
    private void HandleBossSpawn(int bossIndex)
    {
        // bossIndex là vị trí của boss trong list bossesToSpawn (bắt đầu từ 0)
        if (bossIndex == 0) // Boss 1
        {
            HoiThoaiManagement.instance.StartHoiThoai(gapBoss1);
        }
        // else if (bossIndex == 1) // Nếu có Boss 2
        // {
        //     HoiThoaiManagement.instance.StartHoiThoai(gapBoss2);
        // }
    }

    // Hàm này sẽ được tự động gọi khi SpawnMonster phát tín hiệu OnBossDefeated
    private void HandleBossDefeat(int bossIndex)
    {
        if (bossIndex == 0) // Boss 1
        {
            HoiThoaiManagement.instance.StartHoiThoai(tieuDietBoss1);
        }
        // else if (bossIndex == 1) // Nếu có Boss 2
        // {
        //     HoiThoaiManagement.instance.StartHoiThoai(tieuDietBoss2);
        // }
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

    // Bỏ hàm Update() nếu không dùng
    // void Update()
    // {
    // }
}