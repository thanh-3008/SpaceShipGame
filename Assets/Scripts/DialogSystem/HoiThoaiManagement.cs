using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System; // (Quan trọng)

public class HoiThoaiManagement : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textTalk;
    public Image imageCharacter;
    public GameObject dialoguePanel;

    [Header("UI Lựa Chọn")]
    public GameObject choiceButtonContainer;
    public GameObject choiceButtonPrefab;
    public GameObject skipButton;

    private Queue<DialogueLine> cauHoiThoai;
    private DialogueLine currentLine;
    private Conversation currentConversation;
    private bool isTyping = false;
    private bool isChoiceActive = false;
    public static HoiThoaiManagement instance;

    public event Action<Conversation> OnHoiThoaiKetThuc;
    private FinalBossManager finalBossManager;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        cauHoiThoai = new Queue<DialogueLine>();
    }

    void Start()
    {
        finalBossManager = FindObjectOfType<FinalBossManager>();
        if (finalBossManager == null)
        {
            Debug.LogWarning("HoiThoaiManagement: Không tìm thấy FinalBossManager trong Scene!");
        }

        if (choiceButtonContainer != null)
        {
            choiceButtonContainer.SetActive(false);
        }
    }

    void Update()
    {
        if (!dialoguePanel.activeInHierarchy) return;

        if (Input.GetMouseButtonDown(0) && !isChoiceActive)
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textTalk.text = currentLine.text;
                isTyping = false;
            }
            else
            {
                DisplayHoiThoai();
            }
        }
    }

    public void StartHoiThoai(Conversation conversation)
    {
        TimeScaleManager.RequestPause();
        dialoguePanel.SetActive(true);
        cauHoiThoai.Clear();

        isChoiceActive = false;
        if (choiceButtonContainer != null) choiceButtonContainer.SetActive(false);
        if (skipButton != null) skipButton.SetActive(true);

        currentConversation = conversation;

        foreach (DialogueLine a in conversation.lines)
        {
            cauHoiThoai.Enqueue(a);
        }
        DisplayHoiThoai();
    }

    public void DisplayHoiThoai()
    {
        if (cauHoiThoai.Count == 0)
        {
            if (currentConversation != null && currentConversation.choices != null && currentConversation.choices.Count > 0)
            {
                isChoiceActive = true;
                ShowChoices(currentConversation.choices);
            }
            else
            {
                EndDialogue();
                TimeScaleManager.ReleasePause(); // <<< (Game SẼ CHẠY LẠI Ở ĐÂY)
            }
            return;
        }

        currentLine = cauHoiThoai.Dequeue();
        textName.text = currentLine.speaker.speakerName;
        StopAllCoroutines();
        StartCoroutine(hienThiCauThoai(currentLine));
        imageCharacter.sprite = currentLine.speaker.speakerPortrait;
    }

    public IEnumerator hienThiCauThoai(DialogueLine line)
    {
        isTyping = true;
        textTalk.text = "";
        foreach (char a in line.text.ToCharArray())
        {
            textTalk.text += a;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        isTyping = false;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        isChoiceActive = false;

        if (choiceButtonContainer != null)
        {
            // Xóa các nút cũ để chuẩn bị cho lần sau
            foreach (Transform child in choiceButtonContainer.transform)
            {
                Destroy(child.gameObject);
            }
            choiceButtonContainer.SetActive(false);
        }

        if (currentConversation != null)
        {
            OnHoiThoaiKetThuc?.Invoke(currentConversation);
            currentConversation = null;
        }
    }

    public void Skip()
    {
        StopAllCoroutines();
        isTyping = false;
        cauHoiThoai.Clear();

        if (currentConversation != null && currentConversation.choices != null && currentConversation.choices.Count > 0)
        {
            isChoiceActive = true;
            ShowChoices(currentConversation.choices);
        }
        else
        {
            EndDialogue();
            TimeScaleManager.ReleasePause();
        }
    }

    private void ShowChoices(List<Conversation.Choice> choices)
    {
        Debug.Log("Đang hiện các lựa chọn...");

        if (skipButton != null) skipButton.SetActive(false);
        if (choiceButtonContainer != null) choiceButtonContainer.SetActive(true);

        foreach (Transform child in choiceButtonContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject buttonGO = Instantiate(choiceButtonPrefab, choiceButtonContainer.transform);
            Button button = buttonGO.GetComponent<Button>();

            TextMeshProUGUI buttonText = buttonGO.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = choices[i].choiceText;
            }

            int choiceIndex = i;
            button.onClick.AddListener(() => OnChoiceButtonClicked(choiceIndex));
        }
    }

    // --- (HÀM NÀY ĐÃ SỬA LỖI) ---
    public void OnChoiceButtonClicked(int choiceIndex)
    {
        // 1. Lấy ID
        int id = currentConversation.choices[choiceIndex].choiceID;

        // 2. Kiểm tra Manager
        if (finalBossManager == null)
        {
            finalBossManager = FindObjectOfType<FinalBossManager>();
            if (finalBossManager == null)
            {
                Debug.LogError("Không tìm thấy FinalBossManager để ra lệnh!");
                return;
            }
        }

        // 3. Ra lệnh cho FinalBossManager (để BẮT ĐẦU HỘI THOẠI KHẲNG ĐỊNH)
        switch (id)
        {
            case 1: // ID 1 = Đánh Boss 5
                finalBossManager.Choice2_FightBoss5();
                break;
            case 2: // ID 2 = Đánh Boss 6
                finalBossManager.Choice1_FightBoss6();
                break;
            case 3: // ID 3 = Đánh Cả Hai
                finalBossManager.Choice3_FightBoth();
                break;
            default:
                Debug.LogWarning("Choice ID không hợp lệ: " + id);
                break;
        }

        // 4. (XÓA EndDialogue() VÀ ReleasePause() KHỎI ĐÂY)
        // Logic mới:
        // - Choice...() sẽ gọi StartHoiThoai(confirm).
        // - StartHoiThoai sẽ PAUSE.
        // - Khi hội thoại confirm kết thúc, DisplayHoiThoai() sẽ gọi EndDialogue().
        // - EndDialogue() sẽ phát tín hiệu OnHoiThoaiKetThuc.
        // - FinalBossManager.HandleConversationEnd() sẽ nhận tín hiệu, bắt đầu trận chiến, VÀ GỌI ReleasePause().
    }
}