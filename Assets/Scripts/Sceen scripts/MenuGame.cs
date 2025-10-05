
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuGame : MonoBehaviour
{

    public void StartGame()
    {
        SceneManager.LoadScene(3);
    }

    public void HuongDan()
    {
        SceneManager.LoadScene(2);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }

}
