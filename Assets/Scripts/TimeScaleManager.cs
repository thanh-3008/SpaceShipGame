using UnityEngine;

public static class TimeScaleManager
{
    // Dùng một biến static để đếm số lượng "yêu cầu" pause
    private static int pauseRequests = 0;

    // Gọi hàm này khi bạn muốn game DỪNG LẠI (ví dụ: mở hội thoại, mở pause menu)
    public static void RequestPause()
    {
        pauseRequests++;

        // Chỉ set = 0 nếu đây là yêu cầu đầu tiên
        if (pauseRequests == 1)
        {
            Time.timeScale = 0f;
            Debug.Log("Game Paused. Requests: " + pauseRequests);
        }
    }

    // Gọi hàm này khi bạn muốn game CHẠY LẠI (ví dụ: đóng hội thoại, đóng pause menu)
    public static void ReleasePause()
    {
        pauseRequests--;

        // Đảm bảo không bao giờ bị số âm
        if (pauseRequests < 0)
        {
            pauseRequests = 0;
        }

        // Chỉ chạy lại game khi không còn AI yêu cầu pause nữa
        if (pauseRequests == 0)
        {
            Time.timeScale = 1f;
            Debug.Log("Game Resumed. Requests: " + pauseRequests);
        }
    }

    // (Tùy chọn) Hàm này để 'reset' lại khi tải màn chơi mới, đề phòng bị kẹt
    public static void Reset()
    {
        pauseRequests = 0;
        Time.timeScale = 1f;
    }
}