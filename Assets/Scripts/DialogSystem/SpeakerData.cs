using UnityEngine;

[CreateAssetMenu(fileName = "New Speaker", menuName = "Dialogue/Speaker")]
public class SpeakerData : ScriptableObject
{
    public string speakerName;      // Tên hiển thị (ví dụ: "Nhân vật A")
    public Sprite speakerPortrait;  // Ảnh đại diện (gán vào Image)
}