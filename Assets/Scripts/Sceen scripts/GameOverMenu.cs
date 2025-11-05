using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    public bool isGameOver = false;
    public TextMeshProUGUI textdiemhientai;
    public TextMeshProUGUI textdiemcaonhat;

    void Start()
    {
        gameOverMenuUI.SetActive(false);
    }

    public void showGameOverScreen(int diemdatduoc)
    {
        gameOverMenuUI.SetActive(true);

        // SỬA LỖI 1: Sử dụng TimeScaleManager
        TimeScaleManager.RequestPause();

        // SỬA LỖI 2: Cập nhật trạng thái isGameOver
        isGameOver = true;

        int diemcaonhat = PlayerPrefs.GetInt("DiemCaoNhat", 0);
        if (diemdatduoc > diemcaonhat)
        {
            diemcaonhat = diemdatduoc;
            PlayerPrefs.SetInt("DiemCaoNhat", diemcaonhat);
        }
        textdiemhientai.text = diemdatduoc.ToString();
        textdiemcaonhat.text = diemcaonhat.ToString();
    }

    // SỬA LỖI 3: Xóa hàm GameOver() riêng biệt vì nó bị thừa

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResetGame()
    {
        // SỬA LỖI 1: Sử dụng TimeScaleManager.Reset() khi tải lại scene
        TimeScaleManager.Reset();
        isGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void BackMenu()
    {
        // SỬA LỖI 1: Sử dụng TimeScaleManager.Reset() khi về menu
        TimeScaleManager.Reset();
        isGameOver = false;
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}