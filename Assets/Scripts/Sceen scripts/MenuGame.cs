using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour
{
    // !!! QUAN TRỌNG: Đặt index của Loading Scene của bạn vào đây
    private const int LOADING_SCENE_INDEX = 4; // Thay 4 bằng index của Loading Scene

    public void StartGame()
    {
        // SceneManager.LoadScene(3); // CŨ
        LoadingScreen.Next_Scene = 3; // Set scene tiếp theo là ChonTau (scene 3)
        SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }

    public void HuongDan()
    {
        // SceneManager.LoadScene(2); // CŨ
        LoadingScreen.Next_Scene = 2; // Set scene tiếp theo là HuongDan (scene 2)
        SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackMenu()
    {
        // SceneManager.LoadScene(0); // CŨ
        LoadingScreen.Next_Scene = 0; // Set scene tiếp theo là Menu (scene 0)
        SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }
}