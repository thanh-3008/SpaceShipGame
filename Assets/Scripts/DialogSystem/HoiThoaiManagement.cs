using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class HoiThoaiManagement : MonoBehaviour
{
    public TextMeshProUGUI textName;
    public TextMeshProUGUI textTalk;
    public Image imageCharacter;

    public GameObject dialoguePanel;

    private Queue<DialogueLine> cauHoiThoai;
    private DialogueLine currentLine;
    private bool isTyping = false;
    public static HoiThoaiManagement instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cauHoiThoai = new Queue<DialogueLine>();
    }

    void Update()
    {
        // Chỉ cho phép click nếu hội thoại đang chạy (bảng hội thoại đang bật)
        if (!dialoguePanel.activeInHierarchy) return;

        if (Input.GetMouseButtonDown(0))
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
        // YÊU CẦU DỪNG GAME
        TimeScaleManager.RequestPause();

        dialoguePanel.SetActive(true);
        cauHoiThoai.Clear();

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
            EndDialogue();
            // YÊU CẦU CHẠY LẠI GAME
            TimeScaleManager.ReleasePause();
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
            // Dùng WaitForSecondsRealtime là chính xác vì nó không bị ảnh hưởng bởi Time.timeScale
            yield return new WaitForSecondsRealtime(0.05f);
        }
        isTyping = false;
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void Skip()
    {
        // 1. Dừng mọi coroutine (dừng gõ chữ)
        StopAllCoroutines();
        isTyping = false;

        // 2. Xóa hàng đợi
        cauHoiThoai.Clear();

        // 3. Gọi EndDialogue để ẩn panel
        EndDialogue();

        // 4. Yêu cầu chạy lại game ngay lập tức
        TimeScaleManager.ReleasePause();
    }
}