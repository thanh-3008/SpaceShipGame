using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// ĐÃ SỬA: Đổi tên class từ LoadingSceen -> LoadingScreen
public class LoadingScreen : MonoBehaviour
{
    public static int Next_Scene = 0;

    public GameObject loadingBar; // Nên là một Image với Fill Method = Horizontal
    public TextMeshProUGUI loadingValue;
    public float fixedLoadingTime = 3f; // Thời gian loading tối thiểu

    public void Start()
    {
        // Bắt đầu coroutine, dùng unscaledDeltaTime để nó chạy ngay cả khi game đang pause (Time.timeScale = 0)
        StartCoroutine(LoadSceneAsync(Next_Scene));
    }

    // ĐÃ SỬA: Coroutine này đã được viết lại hoàn toàn để chạy đúng
    public IEnumerator LoadSceneAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false; // Ngăn scene tự động kích hoạt khi load xong

        float elapsedTime = 0f;

        // Loop cho đến khi scene load xong (operation.progress sẽ dừng ở 0.9f)
        while (operation.progress < 0.9f)
        {
            elapsedTime += Time.unscaledDeltaTime;

            // Cập nhật progress bar dựa trên tiến trình load thực tế
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingBar != null)
            {
                loadingBar.GetComponent<Image>().fillAmount = progress;
            }
            if (loadingValue != null)
            {
                loadingValue.text = (progress * 100).ToString("0") + "%";
            }

            yield return null; // Chờ frame tiếp theo
        }

        // Khi load xong (progress >= 0.9f), chúng ta sẽ hiển thị 100%
        if (loadingBar != null)
        {
            loadingBar.GetComponent<Image>().fillAmount = 1f;
        }
        if (loadingValue != null)
        {
            loadingValue.text = "100%";
        }

        // Bây giờ, chờ cho đến khi đủ thời gian loading tối thiểu (fixedLoadingTime)
        // Điều này đảm bảo màn hình loading không biến mất quá nhanh
        while (elapsedTime < fixedLoadingTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        // Đã load xong VÀ đã đủ thời gian, cho phép kích hoạt scene mới
        operation.allowSceneActivation = true;
    }
}