using UnityEngine;

/// <summary>
/// Gắn script này vào bất kỳ GameObject nào bạn muốn nó
/// TỰ HỦY khi va chạm với một Trigger có tag "Khien".
/// (Ví dụ: Gắn vào Prefab đạn của Player).
/// </summary>
public class DestroyOnHitShield : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        GameObject audio = GameObject.Find("AudioManagement");
        AudioManagement audioManagement = audio.GetComponent<AudioManagement>();
        // Kiểm tra xem vật mà chúng ta va chạm
        // có tag là "Khien" hay không
        if (other.CompareTag("Khien"))
        {
            // --- THÊM MỚI ---
            // 1. Phát âm thanh "keng"
            if (audioManagement != null)
            {
                // Giả sử bạn có một AudioClip tên là 'amThanhKhien'
                // trong script AudioManagement
                audioManagement.PlaySfxto(audioManagement.blockShield);
            }
            // -----------------

            // 2. Hủy GameObject (viên đạn)
            Destroy(gameObject);
        }
    }
}