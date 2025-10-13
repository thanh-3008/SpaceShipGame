
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState
{
    Starting,   
    Playing,    
    Paused,     
    GameOver,   
    LevelComplete 
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("Game State")]
    [Tooltip("Trạng thái hiện tại của màn chơi")]
    public GameState currentState;

    [Header("Player Stats")]
    [Tooltip("Điểm số hiện tại")]
    public int score = 0;
    [Tooltip("Số mạng khởi đầu")]
    public int startingLives = 3;
    private int currentLives;

    [Header("Level Timing")]
    [Tooltip("Thời gian đếm ngược trước khi bắt đầu")]
    public float countdownDuration = 3.0f;
    private float levelTimer = 0f;

    [Header("Scene Management")]
    [Tooltip("Tên của scene menu chính")]
    public string mainMenuSceneName = "MainMenu";

    public static event System.Action<GameState> OnGameStateChanged;
    public static event System.Action<int> OnScoreChanged;
    public static event System.Action<int> OnLivesChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            levelTimer += Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    private IEnumerator GameLoop()
    {
        yield return StartCoroutine(LevelStarting());

        yield return StartCoroutine(LevelPlaying());

        yield return StartCoroutine(LevelEnding());
    }

    private IEnumerator LevelStarting()
    {
        ChangeState(GameState.Starting);
        Time.timeScale = 1f; // Đảm bảo thời gian chạy bình thường

        score = 0;
        currentLives = startingLives;
        OnScoreChanged?.Invoke(score);
        OnLivesChanged?.Invoke(currentLives);

        float countdown = countdownDuration;
        while (countdown > 0)
        {
            Debug.Log("Bắt đầu trong " + Mathf.Ceil(countdown));
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        Debug.Log("BẮT ĐẦU!");
    }

    private IEnumerator LevelPlaying()
    {
        ChangeState(GameState.Playing);
        while (currentState == GameState.Playing)
        {
            yield return null;
        }
    }

    private IEnumerator LevelEnding()
    {
        if (currentState == GameState.GameOver)
        {
            Debug.Log("GAME OVER");
        }
        else if (currentState == GameState.LevelComplete)
        {
            Debug.Log("CHIẾN THẮNG!");
        }

        yield return new WaitForSeconds(3f); // Chờ vài giây trước khi tải lại

        RestartLevel();
    }


    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
        Debug.Log("Trạng thái game đổi thành: " + newState);
    }


    public void AddScore(int amount)
    {
        if (currentState != GameState.Playing) return;
        score += amount;
        OnScoreChanged?.Invoke(score);
    }

    public void PlayerDied()
    {
        if (currentState != GameState.Playing) return;

        currentLives--;
        OnLivesChanged?.Invoke(currentLives);

        if (currentLives <= 0)
        {
            ChangeState(GameState.GameOver);
        }
        else
        {
            Debug.Log("Người chơi còn " + currentLives + " mạng.");
        }
    }

    public void TogglePause()
    {
        if (currentState == GameState.Playing)
        {
            ChangeState(GameState.Paused);
            Time.timeScale = 0f; // Dừng thời gian
        }
        else if (currentState == GameState.Paused)
        {
            ChangeState(GameState.Playing);
            Time.timeScale = 1f; // Cho thời gian chạy lại
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Rất quan trọng: phải reset timeScale trước khi tải scene mới
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}