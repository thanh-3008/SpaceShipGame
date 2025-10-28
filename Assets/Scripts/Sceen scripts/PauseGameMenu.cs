using UnityEngine;

public class PauseGameMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        TimeScaleManager.ReleasePause(); // SỬ DỤNG BỘ QUẢN LÝ
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        TimeScaleManager.RequestPause(); // SỬ DỤNG BỘ QUẢN LÝ
        // Dòng 'fixedDeltaTime' đã bị xóa vì nó không cần thiết khi timescale = 0
        isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Các hàm reset/chuyển scene nên dùng TimeScaleManager.Reset()
    // để đảm bảo timeScale trở về 1 và bộ đếm về 0.
    public void ResetGame()
    {
        TimeScaleManager.Reset(); // RESET BỘ QUẢN LÝ
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void BackMenu()
    {
        TimeScaleManager.Reset(); // RESET BỘ QUẢN LÝ
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Huongdan()
    {
        TimeScaleManager.Reset(); // RESET BỘ QUẢN LÝ
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}