using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

// GẮN SCRIPT NÀY VÀO 1 GAMEOBJECT RỖNG (ví dụ: FinalBossManager)
public class FinalBossManager : MonoBehaviour
{
    [Header("Timer")]
    public float eventTimeInMinutes = 15f;
    private float eventTimeInSeconds;
    private float timer = 0f;
    private bool eventTriggered = false;

    [Header("Boss Prefabs")]
    public GameObject boss5Prefab;
    public GameObject boss6Prefab;

    [Header("Vị trí (Transforms)")]
    public Transform spawnPointBoss5;
    public Transform spawnPointBoss6;
    public Transform dialoguePointBoss5;
    public Transform dialoguePointBoss6;

    [Header("Tốc độ")]
    public float bossEntrySpeed = 3f;
    public float bossExitSpeed = 10f;

    [Header("Hội Thoại (Kéo vào)")]
    [Tooltip("Hội thoại KHI CHỈ CÓ BOSS 5 xuất hiện")]
    public Conversation boss5EntryConversation;
    [Tooltip("Hội thoại LỰA CHỌN (sau khi cả 2 đã xuất hiện)")]
    public Conversation choiceConversation;

    [Header("Hội Thoại (Khẳng định lựa chọn)")]
    [Tooltip("Hội thoại sau khi chọn 'Chỉ Đánh Boss 5'")]
    public Conversation confirmConversation_Boss5;
    [Tooltip("Hội thoại sau khi chọn 'Chỉ Đánh Boss 6'")]
    public Conversation confirmConversation_Boss6;
    [Tooltip("Hội thoại sau khi chọn 'Đánh Cả Hai'")]
    public Conversation confirmConversation_Both;

    [Header("Hội Thoại Kết Thúc (Thắng)")]
    public Conversation winConversation_Boss5;
    public Conversation winConversation_Boss6;
    public Conversation winConversation_Both;

    [Header("Audio (Kéo vào)")]
    public AudioManagement audio; // Kéo AudioManagement vào đây
    public AudioClip boss5EntrySound; // Tiếng boss 5 xuất hiện
    public AudioClip boss6EntrySound; // Tiếng boss 6 xuất hiện
    public AudioClip bossDismissSound; // Tiếng boss bay đi
    // finalVictoryMusic đã bị xóa

    // Biến nội bộ
    private GameObject spawnedBoss5;
    private GameObject spawnedBoss6;
    private int bossesToDefeat = 0;
    private int selectedChoiceID = 0;
    private EndingScreenManager endingScreen;

    void Start()
    {
        eventTimeInSeconds = eventTimeInMinutes * 60f;
        if (HoiThoaiManagement.instance != null)
        {
            HoiThoaiManagement.instance.OnHoiThoaiKetThuc += HandleConversationEnd;
        }

        endingScreen = FindObjectOfType<EndingScreenManager>();
        if (endingScreen == null)
        {
            Debug.LogError("FinalBossManager: Không tìm thấy EndingScreenManager trong Scene!");
        }

        if (audio == null)
        {
            audio = FindObjectOfType<AudioManagement>();
            if (audio == null)
            {
                Debug.LogWarning("FinalBossManager: Không tìm thấy AudioManagement!");
            }
        }
    }

    void OnDestroy()
    {
        if (HoiThoaiManagement.instance != null)
        {
            HoiThoaiManagement.instance.OnHoiThoaiKetThuc -= HandleConversationEnd;
        }
    }

    void Update()
    {
        if (eventTriggered) return;

        timer += Time.deltaTime;

        if (timer >= eventTimeInSeconds)
        {
            eventTriggered = true;
            Debug.Log("SỰ KIỆN 15 PHÚT: Kích hoạt!");
            StartCoroutine(BossEntrySequence());
        }
    }

    #region // --- Sequence Hội Thoại Ban Đầu ---
    IEnumerator BossEntrySequence()
    {
        spawnedBoss5 = Instantiate(boss5Prefab, spawnPointBoss5.position, spawnPointBoss5.rotation);

        if (audio != null && boss5EntrySound != null)
        {
            audio.PlaySfxto(boss5EntrySound);
        }

        DisableBossAI(spawnedBoss5);
        SubscribeToDeathEvent(spawnedBoss5);

        bool boss5InPlace = false;
        while (!boss5InPlace)
        {
            if (spawnedBoss5 != null)
            {
                spawnedBoss5.transform.position = Vector2.MoveTowards(spawnedBoss5.transform.position, dialoguePointBoss5.position, bossEntrySpeed * Time.deltaTime);
                if (Vector2.Distance(spawnedBoss5.transform.position, dialoguePointBoss5.position) < 0.1f) { boss5InPlace = true; }
            }
            else { yield break; }
            yield return null;
        }

        HoiThoaiManagement.instance.StartHoiThoai(boss5EntryConversation);
    }

    IEnumerator Boss6EntrySequence()
    {
        spawnedBoss6 = Instantiate(boss6Prefab, spawnPointBoss6.position, spawnPointBoss6.rotation);

        if (audio != null && boss6EntrySound != null)
        {
            audio.PlaySfxto(boss6EntrySound);
        }

        DisableBossAI(spawnedBoss6);
        SubscribeToDeathEvent(spawnedBoss6);

        bool boss6InPlace = false;
        while (!boss6InPlace)
        {
            if (spawnedBoss6 != null)
            {
                spawnedBoss6.transform.position = Vector2.MoveTowards(spawnedBoss6.transform.position, dialoguePointBoss6.position, bossEntrySpeed * Time.deltaTime);
                if (Vector2.Distance(spawnedBoss6.transform.position, dialoguePointBoss6.position) < 0.1f) { boss6InPlace = true; }
            }
            else { yield break; }
            yield return null;
        }

        HoiThoaiManagement.instance.StartHoiThoai(choiceConversation);
    }
    #endregion

    // --- (HÀM XỬ LÝ TRUNG TÂM) ---
    private void HandleConversationEnd(Conversation conversation)
    {
        // 1. KIỂM TRA: Hội thoại "Chào sân"
        if (conversation == boss5EntryConversation)
        {
            Debug.Log("Hội thoại Boss 5 kết thúc. Spawn Boss 6.");
            StartCoroutine(Boss6EntrySequence());
        }

        // 2. KIỂM TRA: Hội thoại "Khẳng định" (Để BẮT ĐẦU TRẬN CHIẾN)
        else if (conversation == confirmConversation_Boss5)
        {
            Debug.Log("Hội thoại khẳng định Boss 5 kết thúc. BẮT ĐẦU TRẬN CHIẾN!");
            if (spawnedBoss6 != null)
            {
                if (audio != null && bossDismissSound != null) { audio.PlaySfx(bossDismissSound); }
                spawnedBoss6.GetComponent<BossDismissable>().Dismiss(Vector2.right, bossExitSpeed);
            }

            if (audio != null) { audio.PlayBossMusic(); }
            EnableBossAI(spawnedBoss5);
            TimeScaleManager.ReleasePause();
        }
        else if (conversation == confirmConversation_Boss6)
        {
            Debug.Log("Hội thoại khẳng định Boss 6 kết thúc. BẮT ĐẦU TRẬN CHIẾN!");
            if (spawnedBoss5 != null)
            {
                if (audio != null && bossDismissSound != null) { audio.PlaySfx(bossDismissSound); }
                spawnedBoss5.GetComponent<BossDismissable>().Dismiss(Vector2.left, bossExitSpeed);
            }

            if (audio != null) { audio.PlayBossMusic(); }
            EnableBossAI(spawnedBoss6);
            TimeScaleManager.ReleasePause();
        }
        else if (conversation == confirmConversation_Both)
        {
            Debug.Log("Hội thoại khẳng định Cả Hai kết thúc. BẮT ĐẦU TRẬN CHIẾN!");

            if (audio != null) { audio.PlayBossMusic(); }
            EnableBossAI(spawnedBoss5);
            EnableBossAI(spawnedBoss6);
            TimeScaleManager.ReleasePause();
        }

        // 3. KIỂM TRA: Hội thoại "Thắng" (Để BẮT ĐẦU ENDING SCREEN)
        else if (conversation == winConversation_Boss5)
        {
            if (endingScreen == null) { endingScreen = FindObjectOfType<EndingScreenManager>(); }
            endingScreen.StartEnding(EndingScreenManager.EndingType.Bad,
            "Ta phải phục tùng ngài ấy... Không kẻ nào được phép làm hỏng nhiệm vụ của ta.....");

        }
        else if (conversation == winConversation_Boss6)
        {
            if (endingScreen == null) { endingScreen = FindObjectOfType<EndingScreenManager>(); }
            endingScreen.StartEnding(EndingScreenManager.EndingType.Bad,
                "Cơ thể này đúng là tuyệt vời! Hoàn mỹ... thật hoàn mỹ!");

        }
        else if (conversation == winConversation_Both)
        {
            if (endingScreen == null) { endingScreen = FindObjectOfType<EndingScreenManager>(); }
            endingScreen.StartEnding(EndingScreenManager.EndingType.True,
                "Cuối cùng ! Ta cũng quay trở lại được rồi.......");
        }
    }

    #region // --- (Hàm Lựa Chọn - Đã sửa lỗi) ---
    public void Choice1_FightBoss6()
    {
        Debug.Log("Lựa chọn 1: Đánh Boss 6.");
        bossesToDefeat = 1;
        selectedChoiceID = 2; // Ghi nhớ
        if (confirmConversation_Boss6 != null)
        {
            HoiThoaiManagement.instance.StartHoiThoai(confirmConversation_Boss6);
        }
        else
        {
            Debug.LogWarning("Thiếu hội thoại 'confirmConversation_Boss6'. Bắt đầu đánh luôn.");
            if (spawnedBoss5 != null)
            {
                if (audio != null && bossDismissSound != null) { audio.PlaySfx(bossDismissSound); }
                spawnedBoss5.GetComponent<BossDismissable>().Dismiss(Vector2.left, bossExitSpeed);
            }
            if (audio != null) { audio.PlayBossMusic(); }
            EnableBossAI(spawnedBoss6);
            TimeScaleManager.ReleasePause();
        }
    }
    public void Choice2_FightBoss5()
    {
        Debug.Log("Lựa chọn 2: Đánh Boss 5.");
        bossesToDefeat = 1;
        selectedChoiceID = 1; // Ghi nhớ
        if (confirmConversation_Boss5 != null)
        {
            HoiThoaiManagement.instance.StartHoiThoai(confirmConversation_Boss5);
        }
        else
        {
            Debug.LogWarning("Thiếu hội thoại 'confirmConversation_Boss5'. Bắt đầu đánh luôn.");
            if (spawnedBoss6 != null)
            {
                if (audio != null && bossDismissSound != null) { audio.PlaySfx(bossDismissSound); }
                spawnedBoss6.GetComponent<BossDismissable>().Dismiss(Vector2.right, bossExitSpeed);
            }
            if (audio != null) { audio.PlayBossMusic(); }
            EnableBossAI(spawnedBoss5);
            TimeScaleManager.ReleasePause();
        }
    }
    public void Choice3_FightBoth()
    {
        Debug.Log("Lựa chọn 3: Đánh Cả Hai.");
        bossesToDefeat = 2;
        selectedChoiceID = 3; // Ghi nhớ
        if (confirmConversation_Both != null)
        {
            HoiThoaiManagement.instance.StartHoiThoai(confirmConversation_Both);
        }
        else
        {
            Debug.LogWarning("Thiếu hội thoại 'confirmConversation_Both'. Bắt đầu đánh luôn.");
            if (audio != null) { audio.PlayBossMusic(); }
            EnableBossAI(spawnedBoss5);
            EnableBossAI(spawnedBoss6);
            TimeScaleManager.ReleasePause();
        }
    }
    #endregion

    #region // --- (Hàm Xử Lý Thắng/Thua) ---
    private void SubscribeToDeathEvent(GameObject boss)
    {
        if (boss == null) return;
        BossController bc = boss.GetComponent<BossController>();
        if (bc != null)
        {
            bc.OnBossDieEvent.AddListener(OnFinalBossDefeated);
        }
    }

    // *** THAY ĐỔI 1: Hàm này giờ chỉ gọi Coroutine ***
    private void OnFinalBossDefeated()
    {
        bossesToDefeat--;
        Debug.Log($"Một boss đã bị hạ. Còn lại: {bossesToDefeat}");

        if (bossesToDefeat <= 0)
        {
            Debug.Log("Tất cả boss đã bị hạ! Bắt đầu đếm 3 giây...");
            // Bắt đầu Coroutine để chờ
            StartCoroutine(VictorySequence());
        }
    }

    // *** THAY ĐỔI 2: Hàm mới để xử lý việc chờ ***
    private IEnumerator VictorySequence()
    {
        // 1. Chờ 3 giây
        yield return new WaitForSeconds(1f);

        // 2. Chạy logic thắng
        Debug.Log("CHIẾN THẮNG! Kích hoạt hội thoại kết thúc.");

        // DỪNG NHẠC BOSS VÀ CHẠY NHẠC NỀN CŨ
        if (audio != null)
        {
            audio.PlayDefaultMusic();
        }

        Conversation winConversation = null;
        switch (selectedChoiceID)
        {
            case 1: winConversation = winConversation_Boss5; break;
            case 2: winConversation = winConversation_Boss6; break;
            case 3: winConversation = winConversation_Both; break;
        }

        if (winConversation != null)
        {
            HoiThoaiManagement.instance.StartHoiThoai(winConversation);
        }
        else
        {
            Debug.LogError("Đã thắng nhưng không tìm thấy hội thoại kết thúc cho choiceID: " + selectedChoiceID);
        }
    }


    private void DisableBossAI(GameObject boss)
    {
        if (boss == null) return;
        IBossAI ai = boss.GetComponent<IBossAI>();
        if (ai != null) { (ai as MonoBehaviour).enabled = false; }
    }
    private void EnableBossAI(GameObject boss)
    {
        if (boss == null) return;
        IBossAI ai = boss.GetComponent<IBossAI>();
        if (ai != null) { (ai as MonoBehaviour).enabled = true; }
    }
    #endregion
}