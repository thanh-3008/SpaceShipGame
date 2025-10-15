using TMPro;
using UnityEngine;

public class AICompanionControllerSo25 : MonoBehaviour
{
    private Transform player;
    private TextMeshProUGUI companionText;             // UI để hiển thị lời nói
    private GameObject shieldPrefab;
    [SerializeField]
    private float followDistance = 2.5f;
    [SerializeField]
    private float shieldCooldown = 20f;

    private float shieldTimer;
    private bool shieldActive = false;

    void Start()
    {
        Say("Xin chào chỉ huy! Tôi sẽ đồng hành cùng bạn! 🚀");
    }

    void Update()
    {
        FollowPlayer();

        shieldTimer += Time.deltaTime;

        // Giả lập máu player thấp để demo (sau này bạn có thể lấy từ PlayerHealth)
        float fakePlayerHealth = Mathf.PingPong(Time.time * 10, 100);

        if (fakePlayerHealth < 30 && !shieldActive && shieldTimer >= shieldCooldown)
        {
            ActivateShield();
        }

        // Thỉnh thoảng nói ngẫu nhiên
        if (Random.Range(0, 1000) < 2)
        {
            RandomTalk();
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        Vector3 targetPos = player.position + new Vector3(1.5f, 1.2f, 0);
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 3);
    }

    private void ActivateShield()
    {
        Instantiate(shieldPrefab, player.position, Quaternion.identity, player);
        Say("Kích hoạt khiên năng lượng bảo vệ bạn! 🛡");
        shieldActive = true;
        shieldTimer = 0;
        Invoke(nameof(ResetShield), 8f);
    }

     private void ResetShield()
    {
        shieldActive = false;
    }

    private void Say(string message)
    {
        if (companionText != null)
        {
            companionText.text = message;
            CancelInvoke(nameof(ClearText));
            Invoke(nameof(ClearText), 4f);
        }
        Debug.Log("[AI Companion]: " + message);
    }

    private void ClearText()
    {
        companionText.text = "";
    }

    private void RandomTalk()
    {
        string[] randomLines = {
            "Cẩn thận, radar phát hiện địch phía trước 👀",
            "Hệ thống năng lượng ổn định 🔋",
            "Tôi đang quét khu vực... 🔎",
            "Đạn laser của bạn sắp hết, hãy cẩn thận!",
            "Chúng ta là một đội tuyệt vời!"
        };

        Say(randomLines[Random.Range(0, randomLines.Length)]);
    }
}
