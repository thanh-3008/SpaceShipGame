using UnityEngine;

public class PauseGameMenu : MonoBehaviour
{
    // !!! QUAN TRỌNG: Đặt index của Loading Scene của bạn vào đây
    private const int LOADING_SCENE_INDEX = 4; // Thay 4 bằng index của Loading Scene

    public GameObject pauseMenuUI;
    public bool isPaused = false;

    // ... (Hàm Start, Update, Resume, Pause, QuitGame không thay đổi) ...

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
        TimeScaleManager.ReleasePause();
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        TimeScaleManager.RequestPause();
        isPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // ĐÃ SỬA
    public void ResetGame()
    {
        TimeScaleManager.Reset();
        isPaused = false;

        // CŨ: UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        int currentSceneIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        LoadingScreen.Next_Scene = currentSceneIndex; // Set target là scene hiện tại
        UnityEngine.SceneManagement.SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }

    // ĐÃ SỬA
    public void BackMenu()
    {
        TimeScaleManager.Reset();
        isPaused = false;

        // CŨ: UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        LoadingScreen.Next_Scene = 0; // Set target là Menu (scene 0)
        UnityEngine.SceneManagement.SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }

    // ĐÃ SỬA
    public void Huongdan()
    {
        TimeScaleManager.Reset();
        isPaused = false;

        // CŨ: UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        LoadingScreen.Next_Scene = 2; // Set target là HuongDan (scene 2)
        UnityEngine.SceneManagement.SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }
}