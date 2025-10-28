using UnityEngine;
using TMPro; // Bắt buộc: Thêm thư viện TextMeshPro

public class Timer : MonoBehaviour
{
    [Header("UI Component")]
    [Tooltip("Kéo đối tượng TextMeshPro (UI) của bạn vào đây")]
    public TextMeshProUGUI timerText;

    [Header("Timer Settings")]
    [Tooltip("Thời gian tối đa (tính bằng giây). 1800s = 30 phút.")]
    private float maxTime = 1800f;

    // --- BIẾN MỚI ĐỂ THEO DÕI BOSS ---
    [Header("Game Logic Reference")]
    [Tooltip("Kéo đối tượng chứa script SpawnMonster vào đây")]
    public SpawnMonster spawnMonster; // Tham chiếu đến script spawn

    private bool wasBossActive = false; // Cờ để theo dõi trạng thái boss
    // ---------------------------------

    // Biến lưu thời gian hiện tại
    private float currentTime = 0f;

    // Cờ (flag) để kiểm soát đồng hồ có đang chạy hay không
    private bool isTimerRunning = false;

    /// <summary>
    /// Bắt đầu chạy đồng hồ từ 0.
    /// </summary>
    public void StartTimer()
    {
        currentTime = 0f;
        isTimerRunning = true;
        UpdateTimerDisplay(); // Cập nhật hiển thị thành "00:00" ngay lập tức
    }
    public void Start()
    {
        StartTimer();
    }

    /// <summary>
    /// Tạm dừng đồng hồ (dùng cho Pause hoặc Game Over).
    /// </summary>
    public void PauseTimer()
    {
        isTimerRunning = false;
    }

    /// <summary>
    /// Tiếp tục chạy đồng hồ (dùng khi un-pause).
    /// </summary>
    public void ResumeTimer()
    {
        // Chỉ tiếp tục nếu thời gian chưa đạt mốc tối đa
        if (currentTime < maxTime)
        {
            isTimerRunning = true;
        }
    }

    /// <summary>
    /// Trả về thời gian hiện tại (tính bằng giây).
    /// </summary>
    public float GetCurrentTime()
    {
        return currentTime;
    }

    // --- HÀM UPDATE ĐÃ SỬA ĐỔI ---
    void Update()
    {
        // 1. Kiểm tra trạng thái của Boss (nếu đã gán script SpawnMonster)
        if (spawnMonster != null)
        {
            // Lấy trạng thái boss (cần thêm hàm GetBossActiveState() vào SpawnMonster)
            bool isBossCurrentlyActive = spawnMonster.GetBossActiveState();

            if (isBossCurrentlyActive && !wasBossActive)
            {
                // Boss vừa xuất hiện -> Dừng timer
                PauseTimer();
                wasBossActive = true;
            }
            else if (!isBossCurrentlyActive && wasBossActive)
            {
                // Boss vừa bị tiêu diệt -> Tiếp tục timer
                ResumeTimer();
                wasBossActive = false;
            }
        }

        // 2. Chạy logic timer (tách ra từ code gốc)
        UpdateTimerLogic();
    }

    /// <summary>
    /// Logic chính để chạy đồng hồ (được tách ra từ Update)
    /// </summary>
    private void UpdateTimerLogic()
    {
        // 1. Kiểm tra xem đồng hồ có được phép chạy không
        // 2. Kiểm tra xem đã đạt mốc 30 phút chưa
        // 3. Time.deltaTime sẽ tự động bằng 0 nếu bạn set Time.timeScale = 0 (khi pause game)
        if (isTimerRunning && currentTime < maxTime)
        {
            // Cộng thêm thời gian trôi qua giữa các frame
            currentTime += Time.deltaTime;

            // Cập nhật lên UI
            UpdateTimerDisplay();

            // Nếu vừa đạt mốc 30 phút, dừng lại
            if (currentTime >= maxTime)
            {
                currentTime = maxTime; // Chốt thời gian ở mốc 30:00
                isTimerRunning = false;
                Debug.Log("Đã đạt mốc 30 phút!");
            }
        }
    }

    /// <summary>
    /// Định dạng thời gian và hiển thị lên TextMeshPro.
    /// </summary>
    private void UpdateTimerDisplay()
    {
        if (timerText == null) return; // An toàn nếu lỡ quên kéo text vào

        // Tính toán phút và giây
        // Mathf.FloorToInt để làm tròn xuống số nguyên gần nhất
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Định dạng chuỗi "MM:SS" (ví dụ: "05:03" thay vì "5:3")
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}