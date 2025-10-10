using UnityEngine;

public class PauseGameMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pauseMenuUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
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
        Time.timeScale = 1f;
        isPaused = false;
    }
    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
        isPaused = true;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ResetGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void BackMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    public void Huongdan()
    {
        Time.timeScale = 1f;
        isPaused = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
