using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    GameOver
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameState currentState;
    public int score = 0;
    private float timeElapsed = 0f;

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
        ChangeState(GameState.Playing);
        score = 0;
    }

    void Update()
    {
        if (currentState == GameState.Playing)
        {
            timeElapsed += Time.deltaTime;
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log("Game state changed to: " + newState.ToString());
    }

    public void AddScore(int amount)
    {
        if (currentState != GameState.Playing) return;
        score += amount;
        Debug.Log("Score: " + score);
    }



    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}