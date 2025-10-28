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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cauHoiThoai = new Queue<DialogueLine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                textTalk.text = currentLine.text;
                isTyping =false;
            }
            else
            {
                DisplayHoiThoai();
            }
        }
        
    }

    public void StartHoiThoai(Conversation conversation)
    {
        Time.timeScale = 0f;
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
        if (cauHoiThoai.Count==0)
        {
            EndDialogue();
            Time.timeScale = 1f;
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
            yield return new WaitForSecondsRealtime(0.1f);
        }
        isTyping = false;
    }    

    public void EndDialogue() { 
    
        dialoguePanel.SetActive(false);
    }
}
