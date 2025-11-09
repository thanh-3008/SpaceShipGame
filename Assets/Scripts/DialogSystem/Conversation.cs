using UnityEngine;
using UnityEngine.Events; // <-- THÊM DÒNG NÀY
using System.Collections.Generic; // <-- THÊM DÒNG NÀY
[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject
{
    public DialogueLine[] lines; // Một mảng các câu thoại
    // --- (SỬA LẠI CLASS NÀY) ---
    [System.Serializable]
    public class Choice
    {
        [Tooltip("Nội dung sẽ hiện trên nút")]
        public string choiceText;

        [Tooltip("Mã số của lựa chọn này (1=Đánh Boss 5, 2=Đánh Boss 6, 3=Đánh Cả Hai)")]
        public int choiceID; // <-- THAY THẾ UnityEvent BẰNG CÁI NÀY
    }
    // -------------------------

    [Header("Lựa chọn (Chỉ điền nếu đây là hội thoại lựa chọn)")]
    public List<Choice> choices;
}