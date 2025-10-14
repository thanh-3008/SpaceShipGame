using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // cần cho UI Text & Button

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public int score = 0;
    public bool isGameOver = false;
    public bool isPaused = false;

    [Header("UI References")]
    public Text scoreText;          // hiển thị điểm
    public GameObject gameOverUI;   // panel game over
    public GameObject pauseUI;      // panel pause

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // giữ lại khi đổi scene
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (pauseUI != null) pauseUI.SetActive(false);
    }

    private void Update()
    {
        // Nhấn ESC để pause / resume
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Game Over!");
        if (gameOverUI != null) gameOverUI.SetActive(true);
        Time.timeScale = 0f; // dừng game
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; // reset lại time
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit();
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        if (pauseUI != null) pauseUI.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (pauseUI != null) pauseUI.SetActive(false);
    }
}
