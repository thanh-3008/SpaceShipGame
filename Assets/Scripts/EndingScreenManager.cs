using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; // <--- MỚI: Để tải scene

// GẮN SCRIPT NÀY VÀO MỘT CANVAS MỚI (VÍ DỤ: "Canvas_Ending")
public class EndingScreenManager : MonoBehaviour
{
    private const int LOADING_SCENE_INDEX = 4; // Thay 4 bằng index của Loading Scene
    public static EndingScreenManager instance;

    [Header("UI Components (Kéo vào)")]
    public GameObject endingCanvasPanel;
    public Image fadeOverlay;
    public TextMeshProUGUI largeText;
    public TextMeshProUGUI smallText;

    [Header("Cài đặt Credit Roll")] // <--- MỚI
    public GameObject creditRollCanvas;
    public float creditRollDuration = 30f; // <--- MỚI: Thời gian credit roll chạy (tính bằng giây)   

    [Header("Cài đặt Fading")]
    public float fadeDuration = 3.0f;
    public float textDelay = 1.5f;
    public float waitBeforeCredits = 10f; // Thời gian chờ 10s
    public Color trueEndingColor = Color.white;
    public Color badEndingColor = Color.black;
    public Color badEndingTextColor = Color.red;

    public enum EndingType
    {
        True,
        Bad
    }

    private void Awake()
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
        endingCanvasPanel.SetActive(false);
        if (creditRollCanvas != null)
            creditRollCanvas.SetActive(false);
    }

    public void StartEnding(EndingType type, string message)
    {
        TimeScaleManager.RequestPause(); // Dừng game
        endingCanvasPanel.SetActive(true);
        StartCoroutine(FadeAndShowText(type, message));
    }

    private IEnumerator FadeAndShowText(EndingType type, string message)
    {
        // ... (Toàn bộ code fade nền và fade text của bạn giữ nguyên) ...

        largeText.alpha = 0f;
        smallText.alpha = 0f;
        smallText.text = message;

        Color fadeColor;

        if (type == EndingType.True)
        {
            largeText.text = "<b>True Ending</b>";
            largeText.color = Color.green;
            smallText.color = Color.black;
            fadeColor = trueEndingColor;
        }
        else // Bad Ending
        {
            largeText.text = "<b>Bad Ending</b>";
            largeText.color = badEndingTextColor;
            smallText.color = badEndingTextColor;
            fadeColor = badEndingColor;
        }

        fadeOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0);

        // Chạy fade nền
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            fadeOverlay.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, alpha);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(textDelay);

        // Chạy fade text
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0, 1, timer / fadeDuration);
            largeText.alpha = alpha;
            smallText.alpha = alpha;
            yield return null;
        }

        // --- BẮT ĐẦU PHẦN CHUYỂN CẢNH ---

        // 1. Đợi 10 giây sau khi text đã hiện xong
        yield return new WaitForSecondsRealtime(waitBeforeCredits);

        // 2. Kích hoạt Credit Roll và Tắt màn hình Ending
        if (creditRollCanvas != null)
        {
            creditRollCanvas.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Chưa kéo Credit Roll Canvas vào EndingScreenManager!");
        }
        endingCanvasPanel.SetActive(false);


        // 3. Đợi cho Credit Roll chạy xong (DÙNG THỜI GIAN THỰC)
        yield return new WaitForSecondsRealtime(creditRollDuration); // <--- MỚI

        // 4. Tải lại màn hình Menu
        // Rất quan trọng: Phải reset Time.timeScale về 1 TRƯỚC KHI tải scene mới
        TimeScaleManager.ReleasePause();
        LoadingScreen.Next_Scene = 0; // Set scene tiếp theo là Menu (scene 0)
        SceneManager.LoadScene(LOADING_SCENE_INDEX); // Load Loading Scene
    }
}