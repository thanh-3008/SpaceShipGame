using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameOverMenu : MonoBehaviour
{

    public GameObject gameOverMenuUI;
    public bool isGameOver = false;
    public TextMeshProUGUI textdiemhientai;
    public TextMeshProUGUI textdiemcaonhat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverMenuUI.SetActive(false);
    }
    // Update is called once per frame
    public void showGameOverScreen(int diemdatduoc)
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        int diemcaonhat = PlayerPrefs.GetInt("DiemCaoNhat", 0);
        if(diemdatduoc > diemcaonhat)
        {
            diemcaonhat = diemdatduoc;
            PlayerPrefs.SetInt("DiemCaoNhat", diemcaonhat);
        }
        textdiemhientai.text =  diemdatduoc.ToString();
        textdiemcaonhat.text =  diemcaonhat.ToString();
    }
    void GameOver()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ResetGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    public void BackMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}