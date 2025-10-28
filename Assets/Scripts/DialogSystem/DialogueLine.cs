using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public SpeakerData speaker; // Tham chiếu đến SO Speaker

    [TextArea(3, 10)] // Giúp gõ chữ trong Inspector dễ hơn
    public string text;       // Nội dung câu thoại
}